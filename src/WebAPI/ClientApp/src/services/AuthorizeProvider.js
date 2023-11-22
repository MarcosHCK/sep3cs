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
import { createContext, useContext } from 'react'
import { useEffect, useState } from 'react'
import authService from './AuthorizeService.ts'
const authContext = createContext ([ false, false, {} ])

export function useAuthorize ()
{
  return useContext (authContext)
}

export function AuthorizeProvider (props)
{
  const { children } = props
  const [ inRole, setInRole ] = useState (undefined)
  const [ isAuthorized, setIsAuthorized ] = useState (false)
  const [ isReady, setIsReady ] = useState (false)
  const [ userProfile, setUserProfile ] = useState (undefined)

  useEffect (() =>
    {
      const populateAuthenticationState = async () =>
        {
          const promises = [ authService.isAuthenticated(), authService.getUser() ]
          const [ isAuthorized, userProfile ] = await Promise.all (promises)

          const roles = typeof userProfile.role === 'string'
                          ? { [userProfile.role] : true }
                          : userProfile.role.reduce ((acc, role, index) =>
                              {
                                return { ...acc, [ role[index] ] : true }
                              })

          setIsAuthorized (isAuthorized)
          setIsReady (true)
          setInRole (roles)
          setUserProfile (userProfile)
        }

      const authenticationChanged = async () =>
        {
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

      return () => { authService.unsubscribe(subscription) }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

  return (
    <authContext.Provider
      value={[ isReady, isAuthorized, inRole, userProfile ]}>
      {children}
    </authContext.Provider>)
}
