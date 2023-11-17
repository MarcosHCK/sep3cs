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
import { UserRoles } from '../services/AuthorizeConstants'
import { AvatarEmpty } from './Avatar';
import { Link } from 'react-router-dom'
import { Nav, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap'
import { useAuthorize } from '../services/AuthorizeReact';
import { UserDashboard } from './UserDashboard'
import React from 'react'

export function NavMenu ()
{
  const adminRole = UserRoles.Administrator
  const [ isReady, isAuthorized, userProfile, hasRoles ] = useAuthorize (adminRole)

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">
          <img alt='ICON' src={'/favicon.ico'} className="navbar-brand-logo"/>
            {' '}
          DataClash
        </NavbarBrand>

        <Nav className="d-sm-inline-flex flex-sm-row-reverse" navbar>
          { !isReady
            ? <div></div>
            : (!isAuthorized
              ? (
                  <NavItem>
                    <NavLink tag={Link} to={`${ApplicationPaths.Login}`}>
                      <AvatarEmpty />
                    </NavLink>
                  </NavItem>
                )
              : (
                  <NavItem>
                    <UserDashboard
                        userName={userProfile.name}
                        userEmail={userProfile.email} >
                      <NavLink tag={Link} className="text-dark" to='/'>{'Home'}</NavLink>
                      <NavLink tag={Link} className="text-dark" to={`${ApplicationPaths.Profile}`}>{'Profile'}</NavLink>
                      <hr />
                      <NavLink tag={Link} className="text-dark" to='/cards'>{'Cards'}</NavLink>
                      <NavLink tag={Link} className="text-dark" to='/challenges'>{'Challenges'}</NavLink>
                      <NavLink tag={Link} className="text-dark" to='/clans'>{'Clans'}</NavLink>
                      <NavLink tag={Link} className="text-dark" to='/matches'>{'Matches'}</NavLink>
                    { hasRoles[UserRoles.Administrator] &&
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
