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
import { Navigate } from 'react-router-dom'
import { QueryParameterNames } from '../services/AuthorizeConstants'
import { useAuthorize } from '../services/AuthorizeProvider'
import React from 'react'

export function RequireAuth (props)
{
  const { role, children } = props
  // eslint-disable-next-line no-unused-vars
  const [ isReady, isAuthorized, userProfile ] = useAuthorize ()

  const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI(window.location.href)}`

  return (
    !isReady
    ? (<div></div>)
    : (!isAuthorized
      ? (<Navigate to={redirectUrl} />)
    : (role !== undefined && (userProfile.role !== role))
      ? (<Alert color='danger'>Restricted to '{ role }' users</Alert>)
    : children)
  )
}
