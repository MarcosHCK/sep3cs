/* Copyright (c) 2023-2025
 * This file is part of sep3cs.
 *
 * sep3cs is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * sep3cs is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with sep3cs. If not, see <http://www.gnu.org/licenses/>.
 */
import { ApplicationPaths, ApplicationName } from './AuthorizeConstants'
import { User, UserManager, WebStorageStateStore } from 'oidc-client'

class Subscription
{
  public callback : Function;
  public subscriptionId : number;
}

export const AuthenticationResultStatus =
{
  Fail : 'fail',
  Redirect : 'redirect',
  Success : 'success',
};

export class AuthorizeService
{
  private _callbacks : Array<Subscription> = []
  private _isAuthenticated : boolean = false
  private _nextSubscriptionId : number = 0
  private _popUpDisabled : boolean = true
  private _user : User | undefined = undefined

  public userManager : UserManager;

  createArguments (state? : Object)
    {
      return { useReplaceToNavigate : true, data : state }
    }

  error (message : string)
    {
      return { status : AuthenticationResultStatus.Fail, message }
    }

  notifySubscribers ()
    {
      for (let i = 0; i < this._callbacks.length; i++)
        {
          const callback = this._callbacks[i].callback
          callback ()
        }
    }

  redirect ()
    {
      return { status : AuthenticationResultStatus.Redirect }
    }

  subscribe (callback : Function)
    {
      this._callbacks.push ({ callback, subscriptionId : this._nextSubscriptionId++ })
      return this._nextSubscriptionId - 1
    }

  success (state : Object)
    {
      return { status : AuthenticationResultStatus.Success, state }
    }

  unsubscribe (subscriptionId : number)
    {
      const subscriptionIndex = this._callbacks
        .map ((element, index) => element.subscriptionId === subscriptionId ? { found : true, index } : { found : false })
        .filter (element => element.found === true)
  
      if (subscriptionIndex.length !== 1)
        throw new Error(`Found an invalid number of subscriptions ${subscriptionIndex.length}`);

      this._callbacks = this._callbacks.splice (subscriptionIndex[0].index!, 1)
    }

  updateState (user : User | undefined)
    {
      this._user = user
      this._isAuthenticated = !!this._user
      this.notifySubscribers ()
    }

  async completeSignIn (url : string)
    {
      try
        {
          await this.ensureUserManagerInitialized ()
          const user = await this.userManager.signinCallback (url)
          this.updateState (user)
          return this.success (user && user.state)
        }
      catch (error)
        {
          console.log ('There was an error signing in: ', error)
          return this.error ('There was an error signing in.')
        }
    }

  async completeSignOut (url : string)
    {
      await this.ensureUserManagerInitialized ()

      try
        {
          const response = await this.userManager.signoutCallback (url)
          this.updateState (undefined)
          return this.success (response && response.state)
        }
      catch (error)
        {
          console.log (`There was an error trying to log out '${error}'.`)
          return this.error (error)
        }
    }

  async ensureUserManagerInitialized ()
    {
      if (this.userManager !== undefined)
        return

      let response = await fetch (ApplicationPaths.ApiAuthorizationClientConfigurationUrl)

      if (!response.ok)
        throw new Error (`Could not load settings for '${ApplicationName}'`)

      let settings = await response.json ()

      settings.automaticSilentRenew = true
      settings.includeIdTokenInSilentRenew = true
      settings.userStore = new WebStorageStateStore (
        {
          prefix : ApplicationName
        })

      this.userManager = new UserManager (settings)
      this.userManager.events.addUserSignedOut (async () =>
        {
          await this.userManager.removeUser ()
          this.updateState (undefined)
        })
    }

  async getAccessToken ()
    {
      await this.ensureUserManagerInitialized ()
      const user = await this.userManager.getUser ()
    return user && user.access_token
    }

  async getUser ()
    {
      if (this._user && this._user.profile)
        return this._user.profile

      await this.ensureUserManagerInitialized ()
      const user = await this.userManager.getUser ()
    return user && user.profile
    }

	async hasRole (role : string)
    {
		  const user = await this.getUser ();

		  if (!user)
        return false
      else
        {
          if (user.role instanceof Array)
            return user.role.includes (role)
          else
            return user.role === role
        }
	  }

  async isAuthenticated ()
    {
      const user = await this.getUser ()
      return !!user
    }

  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.
  async signIn (state : Object)
    {
      await this.ensureUserManagerInitialized ()

      try
        {
          const silentUser = await this.userManager.signinSilent (this.createArguments ())
          this.updateState (silentUser)
          return this.success (state)
        }
      catch (silentError)
        {
          // User might not be authenticated, fallback to popup authentication
          console.log ("Silent authentication error: ", silentError)

          try
            {
              if (this._popUpDisabled)
                throw new Error ('Popup disabled. Change \'AuthorizeService.js:AuthorizeService._popupDisabled\' to false to enable it.')

              const popUpUser = await this.userManager.signinPopup (this.createArguments ())
              this.updateState (popUpUser)
              return this.success (state)
            }
          catch (popUpError)
            {
              if (popUpError.message === "Popup window closed")
                return this.error("The user closed the window.")
              else if (!this._popUpDisabled)
                console.log("Popup authentication error: ", popUpError)

				      try
                {
                  await this.userManager.signinRedirect (this.createArguments (state))
                  return this.redirect ()
                }
              catch (redirectError)
                {
                  console.log ("Redirect authentication error: ", redirectError)
                  return this.error (redirectError)
                }
            }
        }
    }

  // We try to sign out the user in two different ways:
  // 1) We try to do a sign-out using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 2) If the method above fails, we redirect the browser to the IdP to perform a traditional
  //    post logout redirect flow.
  async signOut (state : Object)
    {
      await this.ensureUserManagerInitialized ()

      try
        {
          if (this._popUpDisabled)
            throw new Error ('Popup disabled. Change \'AuthorizeService.js:AuthorizeService._popupDisabled\' to false to enable it.')

          await this.userManager.signoutPopup (this.createArguments ())
          this.updateState (undefined)
          return this.success (state)
        }
      catch (popupSignOutError)
        {
          console.log ("Popup signout error: ", popupSignOutError)

          try
            {
              await this.userManager.signoutRedirect (this.createArguments (state))
              return this.redirect ()
            }
          catch (redirectSignOutError)
            {
              console.log ("Redirect signout error: ", redirectSignOutError)
              return this.error (redirectSignOutError)
            }
        }
    }

  static get instance() { return authService }
}

const authService = new AuthorizeService ()
export default authService
