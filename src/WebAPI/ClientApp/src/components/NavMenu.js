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
import { Nav } from 'reactstrap'
import { Component } from 'react'
import { Link } from 'react-router-dom'
import { LoginMenu } from './LoginMenu'
import { Navbar } from 'reactstrap'
import { NavbarBrand } from 'reactstrap'
import React from 'react'

export class NavMenu extends Component
{
  static displayName = NavMenu.name;

  constructor (props)
    {
      super(props);

      this.toggleNavbar = this.toggleNavbar.bind (this);
      this.state =
        {
          collapsed: true
        };
    }

  toggleNavbar ()
    {
      this.setState (
        {
          collapsed: !this.state.collapsed
        });
    }

  render()
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
              <LoginMenu />
            </Nav>
          </Navbar>
        </header>);
    }
}
