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
import { ApplicationPaths } from './ApiAuthorizationConstants'
import { Link } from 'react-router-dom'
import { NavItem, NavLink } from 'reactstrap'
import authService from './AuthorizeService'
import React, { Component, Fragment } from 'react'

class LoginMenuPlain extends Component
{
  constructor (props)
    {
      super (props)

      this.state =
        {
          isAuthenticated : false,
          userName : null,
        }
    }

  anonymousView (registerPath, loginPath)
    {
      return (
        <Fragment>
          <NavItem>
            <NavLink tag={Link} className="text-dark" to={registerPath}>Register</NavLink>
          </NavItem>
          <NavItem>
            <NavLink tag={Link} className="text-dark" to={loginPath}>Login</NavLink>
          </NavItem>
        </Fragment>)
    }

	authenticatedView (userName, profilePath, logoutPath)
    {
      return (
        <Fragment>
          <NavItem>
            <NavLink tag={Link} className="text-dark" to={profilePath}>
              {'Hello'} {userName}
            </NavLink>
          </NavItem>
          <NavItem>
            <NavLink tag={Link} className="text-dark" to={logoutPath}>
              {'Logout'}
            </NavLink>
          </NavItem>
        </Fragment>)
    }

  async populateState ()
    {
      const [isAuthenticated, user] = await Promise.all ([ authService.isAuthenticated (), authService.getUser () ])
      this.setState({ isAuthenticated, userName : user && user.name })
    }

  componentDidMount ()
    {
      this._subscription = authService.subscribe (() => this.populateState ())
      this.populateState ()
    }

  componentWillUnmount ()
    {
      authService.unsubscribe (this._subscription)
    }

  render()
    {
      const { isAuthenticated, userName } = this.state

      if (!isAuthenticated)
        {
          const registerPath = `${ApplicationPaths.Register}`
          const loginPath = `${ApplicationPaths.Login}`
          return this.anonymousView (registerPath, loginPath)
        }
      else
        {
          const profilePath = `${ApplicationPaths.Profile}`
          const logoutPath = { pathname : `${ApplicationPaths.LogOut}`, state : { local : true } }
          return this.authenticatedView (userName, profilePath, logoutPath)
        }
    }
}

export const LoginMenu = LoginMenuPlain;
