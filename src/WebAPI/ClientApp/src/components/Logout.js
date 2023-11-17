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
import { ApplicationPaths } from '../services/AuthorizeConstants'
import { AuthenticationResultStatus } from '../services/AuthorizeService'
import { Component } from 'react'
import { LogoutActions } from '../services/AuthorizeConstants'
import { QueryParameterNames } from '../services/AuthorizeConstants'
import authService from '../services/AuthorizeService'
import React from 'react'

// The main responsibility of this component is to handle the user's logout process.
// This is the starting point for the logout process, which is usually initiated when a
// user clicks on the logout button on the LoginMenu component.
export class Logout extends Component
{
  constructor (props)
    {
      super (props)

      this.state =
        {
          authenticated : false,
          isReady : false,
          message : undefined,
        }
    }

  getReturnUrl(state)
    {
      const params = new URLSearchParams (window.location.search)
      const fromQuery = params.get (QueryParameterNames.ReturnUrl)

      if (fromQuery && !fromQuery.startsWith (`${window.location.origin}/`))
        throw new Error ("Invalid return url. The return url needs to have the same origin as the current page.")
    return (state && state.returnUrl) || fromQuery || `${window.location.origin}${ApplicationPaths.LoggedOut}`
    }

  navigateToReturnUrl(returnUrl)
    {
      return window.location.replace (returnUrl)
    }

  async logout (returnUrl)
    {
      const state = { returnUrl }
      const isauthenticated = await authService.isAuthenticated ()

      if (!isauthenticated)

        this.setState ({ message : "You successfully logged out!" })
      else
        {
          const result = await authService.signOut (state)

          switch (result.status)
            {
              default:
                throw new Error("Invalid authentication result status.")
              case AuthenticationResultStatus.Redirect:
                break;
              case AuthenticationResultStatus.Success:
                await this.navigateToReturnUrl (returnUrl)
                break;
              case AuthenticationResultStatus.Fail:
                this.setState ({ message : result.message })
                break;
            }
        }
    }

  async populateAuthenticationState ()
    {
      const authenticated = await authService.isAuthenticated ()
      this.setState ({ isReady : true, authenticated })
    }

  async processLogoutCallback ()
    {
      const url = window.location.href
      const result = await authService.completeSignOut (url)

      switch (result.status)
        {
          default:
            throw new Error ("Invalid authentication result status.")
          case AuthenticationResultStatus.Redirect:
            // There should not be any redirects as the only time completeAuthentication finishes
            // is when we are doing a redirect sign in flow.
            throw new Error('Should not redirect.')
          case AuthenticationResultStatus.Success:
            await this.navigateToReturnUrl (this.getReturnUrl (result.state))
            break;
          case AuthenticationResultStatus.Fail:
            this.setState({ message : result.message })
             break;
        }
    }

  componentDidMount ()
    {
      switch (this.props.action)
        {
          default:
            throw new Error(`Invalid action '${this.props.action}'`)
          case LogoutActions.Logout:
            if (!!window.history.state.state.local)

              this.logout (this.getReturnUrl ())
            else
              {
                // This prevents regular links to <app>/authentication/logout from triggering a logout
                this.setState ({ isReady : true, message : "The logout was not initiated from within the page." })
              }
            break;
          case LogoutActions.LogoutCallback:
            this.processLogoutCallback ()
            break;
          case LogoutActions.LoggedOut:
             this.setState ({ isReady : true, message : "You successfully logged out!" })
            break;
        }

      this.populateAuthenticationState ()
    }

  render ()
    {
      const { isReady, message } = this.state

      if (!isReady)
        return <div></div>
      if (!!message)
        return (<div>{message}</div>);
      else
        {
          switch (this.props.action)
            {
              default:
                throw new Error (`Invalid action '${this.props.action}'`)
              case LogoutActions.Logout:
                return (<div>Processing logout</div>)
              case LogoutActions.LogoutCallback:
                return (<div>Processing logout callback</div>)
              case LogoutActions.LoggedOut:
                return (<div>{message}</div>)
            }
        }
    }
}
