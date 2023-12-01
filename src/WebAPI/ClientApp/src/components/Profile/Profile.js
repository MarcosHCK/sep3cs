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
import { Alert, Col, Container, Row } from 'reactstrap'
import { Nav, NavItem, NavLink } from 'reactstrap'
import { PlayerClient } from '../../webApiClient.ts'
import { ProfileClan } from './ProfileClan'
import { ProfileDeck } from './ProfileDeck'
import { ProfileIdentity } from './ProfileIdentity'
import { ProfilePlayer } from './ProfilePlayer'
import { ProfileChallenge } from './ProfileChallenge'
import { useAuthorize } from '../../services/AuthorizeProvider'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function Profile ()
{
  const { isAuthorized, userProfile } = useAuthorize ()
  const [ activeIndex, setActiveIndex ] = useState (0)
  const [ playerProfile, setPlayerProfile ] = useState (-1)
  const [ playerClient ] = useState (new PlayerClient ())
  const errorReporter = useErrorReporter ()

  const downProps = { playerProfile, userProfile }

  const pages =
    [
      { title: 'Identity', component: <ProfileIdentity {...downProps} /> },
      { title: 'Player', component: <ProfilePlayer {...downProps} /> },
      { separator : true },
      { title: 'Challenges', component: <ProfileChallenge {...downProps}/> },
      { title: 'Clan', component: <ProfileClan {...downProps} /> },
      { title: 'Deck', component: <ProfileDeck {...downProps} /> },
    ]

  useEffect (() =>
    {
      const refreshPlayer = async () =>
        {
          if (isAuthorized) try
            {
              return await playerClient.get (-1)
            }
          catch (error)
            {
              errorReporter (error)
            }
        }

      setPlayerProfile (undefined)
      refreshPlayer ().then ((player) => setPlayerProfile (player))
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isAuthorized])

  if (!isAuthorized)
    return <WaitSpinner />
  else if (!playerProfile)
    return <Alert color='danger'>User is not a player</Alert>
  else
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
              { pages[activeIndex].component }
            </Col>
          </Row>
        </Container>)
}
