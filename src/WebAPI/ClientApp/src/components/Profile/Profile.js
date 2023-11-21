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
import { Avatar } from '../Avatar'
import { Col, Container, Row } from 'reactstrap'
import { Nav, NavItem, NavLink } from 'reactstrap'
import { ProfileIdentity } from './ProfileIdentity'
import { useAuthorize } from '../../services/AuthorizeProvider'
import { WaitSpinner } from '../WaitSpinner'
import React, { useState } from 'react'

export function Profile ()
{
  const { isAuthorized, userProfile } = useAuthorize ()
  const [ activeIndex, setActiveIndex ] = useState (0)

  const pages =
    [
      { title: 'Identity', component: ProfileIdentity },
    ]

  return (
    !isAuthorized
    ? <WaitSpinner />
    : <Container fluid>
        <Row>
          <div className='d-flex align-items-center'>
            <Avatar userName={ userProfile.name } userEmail={ userProfile.email } />
            <div className='p-2'>
              <h2>{ userProfile.name } { !userProfile.family_name ? null : `(${userProfile.family_name})` }</h2>
              <span>Personal profile</span>
            </div>
          </div>
        </Row>
          <span className='p-3' />
        <Row>
          <Col xs='2'>
            <Nav card pills vertical='sm'>
          { pages.map ((page, index) => (
            page.separator
            ? <hr key={`nav${index}`} />
            : <NavItem key={`nav${index}`}>
                <NavLink active={activeIndex === index} href='#' onClick={() => { setActiveIndex (index) }}>
                  { page.title }
                </NavLink>
              </NavItem>))}
            </Nav>
          </Col>
          <Col xs='10'>
            { pages[activeIndex].component ({ userProfile }) }
          </Col>
        </Row>
      </Container>)
}
