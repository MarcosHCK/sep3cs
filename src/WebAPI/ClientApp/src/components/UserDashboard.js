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
import './UserDashboard.css'
import { Avatar } from './Avatar'
import { Container, Offcanvas, OffcanvasBody, OffcanvasHeader, Row, Col } from 'reactstrap'
import { useState } from 'react'

export function UserDashboard (props)
{
  const [ show, setShow ] = useState (false)
  const { userName, userEmail, children } = props

  return (
    <div className="user-dashboard">
      <div onClick={() => setShow (true)}>
        <Avatar userName={userName} userEmail={userEmail} />
      </div>

      <Offcanvas backdrop scrollable
            direction='end'
            fade={false}
            isOpen={show}>
        <OffcanvasHeader toggle={() => setShow (show === false)}>
          <Container fluid>
            <Row>
              <Col xs="3">
                <div onClick={() => setShow (false)}>
                  <Avatar userName={userName} userEmail={userEmail} />
                </div>
              </Col>
              <Col xs="9">
                <Row>
                  <span className='user-dashboard-name'>
                    {userName}
                  </span>
                </Row>
                <Row>
                  <span className='user-dashboard-nick'>
                    {userName}
                  </span>
                </Row>
              </Col>
            </Row>
          </Container>
        </OffcanvasHeader>
        <OffcanvasBody>
          { children }
        </OffcanvasBody>
      </Offcanvas>
    </div>)
}
