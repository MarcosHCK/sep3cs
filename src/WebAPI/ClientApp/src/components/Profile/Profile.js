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
import { Avatar } from '../Avatar'
import { Col, Container, Row } from 'reactstrap'
import { Nav, NavItem, NavLink } from 'reactstrap'
import { ProfileChallenge } from './ProfileChallenge'
import { ProfileClan } from './ProfileClan'
import { ProfileClanPlayers } from './ProfileClanPlayers'
import { ProfileClanWars } from './ProfileClanWars'
import { ProfileDeck } from './ProfileDeck'
import { ProfileIdentity } from './ProfileIdentity'
import { ProfilePlayer } from './ProfilePlayer'
import React, { useState } from 'react'

export function Profile (props)
{
  const { playerProfile, userProfile } = props
  const [ activeIndex, setActiveIndex ] = useState (String (0))
  const [ sectionOpen, setSectionOpen ] = useState ([])

  const downProps = { playerProfile, userProfile }

  const pages =
    [
      { title: 'Identity', component: <ProfileIdentity {...downProps} /> },
      { title: 'Player', component: <ProfilePlayer {...downProps} /> },
      { separator : true },
      { title: 'Challenges', component: <ProfileChallenge {...downProps}/> },
      { title: 'Clan', component: <ProfileClan {...downProps} />, children :
        [
          { title: 'Players', component: <ProfileClanPlayers {...downProps} /> },
          { title: 'Wars', component: <ProfileClanWars {...downProps} /> },
        ]},
      { title: 'Deck', component: <ProfileDeck {...downProps} /> },
    ]

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
                key={`accordionKey${index}`}
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

  return (
    <Container fluid>
      <Row>
        <div className='d-flex align-items-center'>
          <Avatar userName={ userProfile.name } userEmail={ userProfile.email } />
          <div className='p-2'>
            <h2>{ playerProfile.nickname ?? userProfile.family_name ?? userProfile.name }</h2>
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
