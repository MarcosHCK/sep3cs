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
import { LogoutActions } from '../services/AuthorizeConstants'
import { QueryParameterNames } from '../services/AuthorizeConstants'
import { WaitSpinner } from './WaitSpinner'
import authService from '../services/AuthorizeService'
import React, { useEffect, useState } from 'react'

export function Logout (props)
{
  const [message, setMessage] = useState (null)
  const { action } = props

  const returnUrl = (state) =>
    {
      const params = new URLSearchParams (window.location.search)
      const fromQuery = params.get (QueryParameterNames.ReturnUrl)

      if (fromQuery && !fromQuery.startsWith (`${window.location.origin}/`))
        throw new Error ("Invalid return url. The return url needs to have the same origin as the current page.")
    return (state && state.returnUrl) || fromQuery || `${window.location.origin}${ApplicationPaths.LoggedOut}`
    }

  const callback = async () =>
    {
      const url = window.location.href
      const result = await authService.completeSignOut (url)

      switch (result.status)
        {
          default:
            throw new Error (`Invalid status ${result.status}`)
          case AuthenticationResultStatus.Fail:
            setMessage (result.message)
            break;
          case AuthenticationResultStatus.Redirect:
            throw new Error('Should not redirect.')
          case AuthenticationResultStatus.Success:
            window.location.replace (returnUrl (result.state))
            break;
        }
    }

  const logout = async (returnUrl) =>
    {
      if (!await authService.isAuthenticated ())

        setMessage ('Logged out')
      else
        {
          const state = { returnUrl }
          const result = await authService.signOut (state)

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
    }

  useEffect (() =>
    {
      switch (action)
        {
          default:
            throw new Error (`Invalid action ${action}`)
          case LogoutActions.LoggedOut:
            window.location.replace ('/')
            break;
          case LogoutActions.Logout:
            logout ()
            break;
          case LogoutActions.LogoutCallback:
            callback ()
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
            throw new Error (`Invalid action ${action}`)
          case LogoutActions.LoggedOut:
          case LogoutActions.Logout:
          case LogoutActions.LogoutCallback:
            return (<WaitSpinner />)
        }
    }
}
