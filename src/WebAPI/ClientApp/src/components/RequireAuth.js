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
import { QueryParameterNames } from '../services/AuthorizeConstants'
import { Component } from 'react'
import { Navigate } from 'react-router-dom'
import authService from '../services/AuthorizeService'
import React from 'react'
import { Alert } from 'reactstrap'

export class RequireAuth extends Component
{
  constructor (props)
    {
      super (props)

      this.takesRoles = props.role
      this.state = { authenticated : false, hasRole : false, ready : false }
    }

  takesRoles = undefined

  async authenticationChanged ()
    {
      this.setState ({ ready : false, authenticated : false });
      await this.populateAuthenticationState ();
    }

  async populateAuthenticationState ()
    {
      let authenticated = await authService.isAuthenticated ();
      let hasRole = false

      if (authenticated)
        {
          if (this.takesRoles === undefined)
            hasRole = true
          else
            hasRole = await authService.hasRole (this.takesRoles)
        }

      this.setState ({ ready : true, authenticated, hasRole });
    }

  componentDidMount ()
    {
      this._subscription = authService.subscribe (() => this.authenticationChanged ())
      this.populateAuthenticationState ()
    }

  componentWillUnmount ()
    {
      authService.unsubscribe (this._subscription)
    }

  render ()
    {
      const { authenticated, hasRole, ready } = this.state;
      const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI (window.location.href)}`

      if (!ready)
        return <div></div>;
      else
        {
          if (authenticated === false)
            return <Navigate to={redirectUrl} />
          else if (hasRole === false)
            return <Alert color='danger'>
              Restricted to '{ this.takesRoles }' users
            </Alert>
          else return this.props.children
        }
    }
}
