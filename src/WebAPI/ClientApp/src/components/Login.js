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
import { Alert } from 'reactstrap'
import { ApplicationPaths } from '../services/AuthorizeConstants'
import { AuthenticationResultStatus } from '../services/AuthorizeService'
import { LoginActions } from '../services/AuthorizeConstants'
import { QueryParameterNames } from '../services/AuthorizeConstants'
import authService from '../services/AuthorizeService'
import React, { useEffect, useState } from 'react'

export function Login (props)
{
  const [ message, setMessage ] = useState (null)
  const { action } = props

  const returnUrl = (state) =>
    {
      const params = new URLSearchParams (window.location.search)
      const fromQuery = params.get (QueryParameterNames.ReturnUrl)

      if (fromQuery && !fromQuery.startsWith (`${window.location.origin}/`))
        {
          // This is an extra check to prevent open redirects.
          throw new Error ("Invalid return url. The return url needs to have the same origin as the current page.")
        }
      return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`
    } 

  const callback = async () =>
    {
      const url = window.location.href
      const result = await authService.completeSignIn (url)

      switch (result.status)
        {
          default:
            throw new Error (`Invalid status ${result.status}`)

          case AuthenticationResultStatus.Fail:
            setMessage (result.message)
            break;
          case AuthenticationResultStatus.Redirect:
            throw new Error ('Should not redirect')
          case AuthenticationResultStatus.Success:
            window.location.replace (returnUrl (result.state))
            break;
        }
    }

  const login = async (returnUrl) =>
    {
      const state = { returnUrl }
      const result = authService.signIn (state)

      switch (result.status)
        {
          default:
            throw new Error (`Invalid status ${result.status}`)

          case AuthenticationResultStatus.Fail:
            setMessage (result.message)
            break;
          case AuthenticationResultStatus.Redirect:
            break;
          case AuthenticationResultStatus.Success:
            window.location.replace (returnUrl)
            break;
        }
    }

  useEffect (() =>
    {
      switch (action)
        {
          default:
            throw new Error (`Unknown action ${action}`)

          case LoginActions.Login:
            login ()
            break;
          case LoginActions.LoginCallback:
            callback ()
            break;
          case LoginActions.LoginFailed:
            const params = new URLSearchParams (window.location.search)
            setMessage (params.get ('message'))
            break;
          case LoginActions.Profile:
            {
              const redirectUrl = `${window.location.origin}${ApplicationPaths.IdentityManagePath}`
              window.location.replace (redirectUrl)
            }
            break;
          case LoginActions.Register:
            {
              const registerUrl = `${ApplicationPaths.IdentityRegisterPath}?${QueryParameterNames.ReturnUrl}=${encodeURI(ApplicationPaths.Login)}`
              const redirectUrl = `${window.location.origin}${registerUrl}`
              window.location.replace (redirectUrl)
            }
            break;
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

  if (!!message)
      
    return (<Alert color='danger'>{message}</Alert>)
  else
    {
      switch (action)
        {
          default:
            throw new Error (`Invalid action '${action}'`)
          case LoginActions.Login:
            return (<Alert color='notice'>Processing login</Alert>)
          case LoginActions.LoginCallback:
            return (<Alert color='notice'>Processing login callback</Alert>)
          case LoginActions.Profile:
          case LoginActions.Register:
            return (<div></div>)
        }
    }
}
