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
import './Profile.css'
import { Accordion } from 'reactstrap'
import { AccordionBody } from 'reactstrap'
import { AccordionHeader } from 'reactstrap'
import { AccordionItem } from 'reactstrap'
import { Alert, Col, Container, Row } from 'reactstrap'
import { Avatar } from '../Avatar'
import { Nav, NavItem, NavLink } from 'reactstrap'
import { PlayerClient } from '../../webApiClient.ts'
import { ProfileChallenge } from './ProfileChallenge'
import { ProfileClan } from './ProfileClan'
import { ProfileClanPlayers } from './ProfileClanPlayers'
import { ProfileClanWars } from './ProfileClanWars'
import { ProfileDeck } from './ProfileDeck'
import { ProfileIdentity } from './ProfileIdentity'
import { ProfilePlayer } from './ProfilePlayer'
import { useAuthorize } from '../../services/AuthorizeProvider'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function Profile ()
{
  const { isAuthorized, userProfile } = useAuthorize ()
  const [ activeIndex, setActiveIndex ] = useState (String (0))
  const [ playerProfile, setPlayerProfile ] = useState ()
  const [ playerClient ] = useState (new PlayerClient ())
  const [ sectionOpen, setSectionOpen ] = useState ([])
  const errorReporter = useErrorReporter ()

  const downProps = { playerProfile, userProfile }

  const pages =
    [
      { title: 'Identity', component: <ProfileIdentity {...downProps} /> },
      { title: 'Player', component: <ProfilePlayer {...downProps} /> },
      { separator : true },
      { title: 'Challenges', component: <ProfileChallenge {...downProps}/> },
      { title: 'Clan', component: <ProfileClan {...downProps} />, children :
        [
          { title: 'Players', component: <ProfileClanPlayers /> },
          { title: 'Wars', component: <ProfileClanWars {...downProps} /> },
        ]},
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

  const createSection = (page, index) =>
    {
      const createSectionNav = _ =>
        <NavItem active={activeIndex === index} key={`nav${index}`}>
          <NavLink href='#' onClick={() => { setActiveIndex (index) }}>
            { page.title }
          </NavLink>
        </NavItem>

      if (page.separator)
        return <hr key={index} />
      else if (!page.children)
        return createSectionNav ()
      else
        {
          const children = page.children
          const target = `accordion${index}`

          const toggle = i =>
            {
              const newOpen = [ ...sectionOpen ]
              const oldOpen = sectionOpen[index]
                newOpen[index] = i !== oldOpen ? i : undefined
              setSectionOpen (newOpen)
            }

          return (
            <Accordion
                className='nav-section'
                flush
                open={sectionOpen[index]}
                toggle={i => toggle (i)} >
              <AccordionItem>
                <AccordionHeader targetId={target} tag='div'>
                  { createSectionNav () }
                </AccordionHeader>
                <AccordionBody accordionId={target}>
                  <Nav card vertical='sm'>
                    { children.map ((page, subIndex) => createSection (page, `${index}.${subIndex}`)) }
                  </Nav>
                </AccordionBody>
              </AccordionItem>
            </Accordion>)
        }
    }

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
              <Nav vertical='sm'>
                { pages.map ((page, index) => createSection (page, `${index}`)) }
              </Nav>
            </Col>
            <Col xs='10'>
              { activeIndex.split ('.').reduce ((acc, index) => (!acc ? pages : acc.children)[Number (index)], undefined).component }
            </Col>
          </Row>
        </Container>)
}
