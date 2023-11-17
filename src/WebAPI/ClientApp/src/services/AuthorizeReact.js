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
import authService from './AuthorizeService'
import { useEffect, useState } from 'react'

export function useAuthorize (roles)
{
  const [hasRole, setHasRole] = useState (undefined)
  const [isAuthorized, setIsAuthorized] = useState (false)
  const [isReady, setIsReady] = useState (false)
  const [userProfile, setUserProfile] = useState (undefined)

  if (typeof roles === 'undefined')
    roles = [ ]
  else if (typeof roles === 'string')
    roles = [ roles, ]
  else if ((Array.isArray (roles) && roles.every ((i) => typeof i == 'string')) === false)
    throw TypeError ('useAuthorize takes a role or an array of roles')

  useEffect (() =>
    {
      const populateAuthenticationState = async () =>
        {
          const promises = [
            authService.isAuthenticated(),
            authService.getUser(),
            ...roles.map ((role) => authService.hasRole (role))]
          const [ isAuthorized, userProfile,
            ...hasRolesList ] = await Promise.all (promises)
          const hasRoles = roles.reduce ((acc, curr, index) =>
            ({ ...acc, [curr] : hasRolesList[index] }), {})

          setHasRole (hasRoles)
          setIsAuthorized (isAuthorized)
          setIsReady (true)
          setUserProfile (userProfile)
        }

      const authenticationChanged = async () =>
        {
          setHasRole (undefined)
          setIsAuthorized (false)
          setIsReady (false)
          setUserProfile (undefined)

          await populateAuthenticationState ()
        }

      const subscription = authService.subscribe (() =>
        {
          authenticationChanged ()
        })

      populateAuthenticationState ()
      return () => { authService.unsubscribe (subscription) }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])
return [ isReady, isAuthorized, userProfile, hasRole ]
}

export function useAuthToken ()
{
  const [ authToken, setAuthToken ] = useState (null)
  const [ isReady, isAuthorized ] = useAuthorize ()

  useEffect (() =>
    {
      setAuthToken (null)

      if (isReady && isAuthorized)
        {
          authService.getAccessToken ().then ((token) =>
      {
              setAuthToken (token)
      })
        }
    }, [isReady, isAuthorized])
return [ authToken ]
}
