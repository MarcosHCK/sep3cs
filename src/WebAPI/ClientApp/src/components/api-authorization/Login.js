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
import { ApplicationPaths } from './Constants'
import { AuthenticationResultStatus } from './AuthorizeService'
import { Component } from 'react'
import { LoginActions } from './Constants'
import { QueryParameterNames } from './Constants'
import authService from './AuthorizeService'
import React from 'react'

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
class LoginPlain extends Component
{
  constructor (props)
    {
      super (props)
      this.state = { message : undefined }
    }

  getReturnUrl (state)
    {
        const params = new URLSearchParams(window.location.search);
        const fromQuery = params.get(QueryParameterNames.ReturnUrl);
        if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
        }
        return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`;
    }

  navigateToReturnUrl (returnUrl)
    {
      // It's important that we do a replace here so that we remove the callback uri with the
      // fragment containing the tokens from the browser history.
      window.location.replace (returnUrl)
    }

  redirectToApiAuthorizationPath(apiAuthorizationPath)
    {
      const redirectUrl = `${window.location.origin}${apiAuthorizationPath}`
      // It's important that we do a replace here so that when the user hits the back arrow on the
      // browser he gets sent back to where it was on the app instead of to an endpoint on this
      // component.
      window.location.replace (redirectUrl)
    }

  redirectToProfile ()
    {
      this.redirectToApiAuthorizationPath(ApplicationPaths.IdentityManagePath)
    }

  redirectToRegister ()
    {
      this.redirectToApiAuthorizationPath (`${ApplicationPaths.IdentityRegisterPath}?${QueryParameterNames.ReturnUrl}=${encodeURI(ApplicationPaths.Login)}`)
    }

  async login(returnUrl)
    {
      const state = { returnUrl }
      const result = await authService.signIn (state)

      switch (result.status)
        {
          default:
            throw new Error (`Invalid status result ${result.status}.`)
          case AuthenticationResultStatus.Fail:
            this.setState ({ message: result.message })
            break;
          case AuthenticationResultStatus.Redirect:
            break;
          case AuthenticationResultStatus.Success:
            await this.navigateToReturnUrl (returnUrl)
            break;
        }
    }

  async processLoginCallback ()
    {
      const url = window.location.href
      const result = await authService.completeSignIn(url)

      switch (result.status)
        {
          default:
            throw new Error(`Invalid authentication result status '${result.status}'.`)
          case AuthenticationResultStatus.Fail:
            this.setState ({ message: result.message })
            break;
          case AuthenticationResultStatus.Redirect:
            // There should not be any redirects as the only time completeSignIn finishes
            // is when we are doing a redirect sign in flow.
            throw new Error ('Should not redirect.')
          case AuthenticationResultStatus.Success:
            await this.navigateToReturnUrl (this.getReturnUrl (result.state))
            break;
        }
    }

  componentDidMount ()
    {
      switch (this.props.action)
        {
          default:
            throw new Error (`Invalid action '${this.props.action}'`)
          case LoginActions.Login:
              this.login (this.getReturnUrl ())
              break;
          case LoginActions.LoginCallback:
              this.processLoginCallback ()
              break;
          case LoginActions.LoginFailed:
              const params = new URLSearchParams (window.location.search)
              const error = params.get (QueryParameterNames.Message)
              this.setState ({ message: error })
              break;
          case LoginActions.Profile:
              this.redirectToProfile ()
              break;
          case LoginActions.Register:
              this.redirectToRegister ()
              break;
        }
    }

  render ()
    {
      const action = this.props.action
      const { message } = this.state

      if (!!message)
      
        return (<div>{message}</div>)
      else
        {
          switch (action)
            {
              default:
                throw new Error (`Invalid action '${action}'`)
              case LoginActions.Login:
                return (<div>Processing login</div>)
              case LoginActions.LoginCallback:
                return (<div>Processing login callback</div>)
              case LoginActions.Profile:
              case LoginActions.Register:
                return (<div></div>)
            }
        }
    }
}

export const Login = LoginPlain;
