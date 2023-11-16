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
import './NavMenu.css';
import { ApplicationPaths } from '../services/AuthorizeConstants'
import { Link } from 'react-router-dom'
import { Nav, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap'
import { UserDashboard } from './UserDashboard'
import authService from '../services/AuthorizeService'
import React, { Component } from 'react'

export class NavMenu extends Component
{
  static displayName = NavMenu.name;

  constructor (props)
    {
      super (props)

      this.state =
        {
          isAuthorized : false,
          isReady : false,
          userProfile : false,
        }
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

  async authenticationChanged ()
    {
      this.setState ({ isAuthorized : false, isReady : false, userProfile : undefined })
      await this.populateAuthenticationState ()
    }

  async populateAuthenticationState ()
    {
      const [ isAuthorized, userProfile ] = await Promise.all ([ authService.isAuthenticated (), authService.getUser () ])
      this.setState ({ isAuthorized : isAuthorized, isReady : true, userProfile : userProfile })
    }

  render ()
    {
      return (
        <header>
          <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
            <NavbarBrand tag={Link} to="/">
              <img alt='ICON' src={'/favicon.ico'} className="navbar-brand-logo"/>
                {' '}
              DataClash
            </NavbarBrand>

            <Nav className="d-sm-inline-flex flex-sm-row-reverse" navbar>
              { !this.state.isReady
                ? <div></div>
                : (!this.state.isAuthorized
                  ? (
                      <div>
                        <NavItem>
                          <NavLink tag={Link} className="text-dark" to={`${ApplicationPaths.Register}`}>Register</NavLink>
                        </NavItem>
                        <NavItem>
                          <NavLink tag={Link} className="text-dark" to={`${ApplicationPaths.Login}`}>Login</NavLink>
                        </NavItem>
                      </div>
                    )
                  : (
                      <NavItem>
                        <UserDashboard
                            userName={this.state.userProfile.name}
                            userEmail={this.state.userProfile.email} >
                          <NavLink tag={Link} className="text-dark" to='/'>{'Home'}</NavLink>
                          <NavLink tag={Link} className="text-dark" to={`${ApplicationPaths.Profile}`}>{'Profile'}</NavLink>
                          <hr />
                          <NavLink tag={Link} className="text-dark" to='/cards'>{'Cards'}</NavLink>
                          <NavLink tag={Link} className="text-dark" to='/challenges'>{'Challenges'}</NavLink>
                          <NavLink tag={Link} className="text-dark" to='/clans'>{'Clans'}</NavLink>
                          <NavLink tag={Link} className="text-dark" to='/matches'>{'Matches'}</NavLink>
                        { this.isAdministrator &&
                          <NavLink tag={Link} className="text-dark" to='/players'>{'Players'}</NavLink>
                        }
                          <NavLink tag={Link} className="text-dark" to='/wars'>{'Wars'}</NavLink>
                          <hr />
                          <NavLink tag={Link} className="text-dark" to={{ pathname : `${ApplicationPaths.LogOut}`, state : { local : true } }}>{'Logout'}</NavLink>
                        </UserDashboard>
                      </NavItem>)) }
            </Nav>
          </Navbar>
        </header>)
    }
}
