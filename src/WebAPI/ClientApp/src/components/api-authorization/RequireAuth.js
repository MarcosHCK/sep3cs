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
import { QueryParameterNames } from './Constants'
import { Component } from 'react'
import { Navigate } from 'react-router-dom'
import authService from './AuthorizeService'
import React from 'react'

export class RequireAuth extends Component
{
  constructor (props)
    {
      super (props)

      this.state =
        {
          authenticated : false,
          ready : false,
        }
    }

  async authenticationChanged ()
    {
      this.setState ({ ready : false, authenticated : false });
      await this.populateAuthenticationState ();
    }

  async populateAuthenticationState ()
    {
      const authenticated = await authService.isAuthenticated ();
      this.setState ({ ready : true, authenticated });
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
      const { ready, authenticated } = this.state;
      const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI (window.location.href)}`

      if (!ready)
        return <div></div>;
      else
        {
          if (authenticated)
            return this.props.children
          else
            return <Navigate to={redirectUrl} />
        }
    }
}
