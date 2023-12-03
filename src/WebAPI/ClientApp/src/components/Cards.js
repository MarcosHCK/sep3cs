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
 *//*
import React, { useEffect, useState } from 'react'
import { Card, CardImg, CardBody, CardTitle, CardText, Row, Col, UncontrolledTooltip } from 'reactstrap'
import { CardClient } from '../webApiClient.ts'
import { useErrorReporter } from './ErrorReporter'

export function Cards () {
 const [ items, setItems ] = useState (undefined)
 const [ cardClient ] = useState (new CardClient ())
 const errorReporter = useErrorReporter ()

 useEffect (() => {
 const loadCards = async () => {
 try {
 const paginatedList = await cardClient.getWithPagination(1, 120)
 setItems (paginatedList.items)
 } catch (error) {
 errorReporter (error)
 }
 }

 loadCards ()
 }, [])

 return (
 <Row>
 { (items ?? []).map ((item, index) => (
 <Col sm="2" key={index}>
   <Card id={`card${index}`} style={{ width: '150px', margin: '10px' }}>
     <CardImg top width="100%" src={`/cards/${item.picture}.png`} alt={item.name} />
     <UncontrolledTooltip target={`card${index}`} placement="top">
       <h5>{item.name}</h5>
       <p>{item.description}</p>
       {/* Aquí puedes agregar más información de la carta si lo deseas }
     </UncontrolledTooltip>
   </Card>
 </Col>
 ))}
 </Row>
 )
}
*/
import React, { useEffect, useState } from 'react'
import { Card, CardImg, Row, Col, UncontrolledPopover, PopoverHeader, PopoverBody } from 'reactstrap'
import { CardClient } from '../webApiClient.ts'
import { useErrorReporter } from './ErrorReporter'
import { FaInfo, FaHeart, FaFire, FaSquare, FaUsers, FaClock } from 'react-icons/fa';


export function Cards () {
  const [ troopCards, setTroopCards ] = useState (undefined)
  const [ structCards, setStructCards ] = useState (undefined)
  const [ magicCards, setMagicCards ] = useState (undefined)
  const [ cardClient ] = useState (new CardClient ())
  const errorReporter = useErrorReporter ()
 
  useEffect(() => {
    const loadCards = async () => {
      try {
        const troopCardsList = await cardClient.getWithPagination2 ('TroopCard', 1, 10);
        const structCardsList = await cardClient.getWithPagination3 ('StructCard', 1, 10);
        const magicCardsList = await cardClient.getWithPagination ('MagicCard', 1, 10);
        setTroopCards(troopCardsList.items);
        setStructCards(structCardsList.items);
        setMagicCards(magicCardsList.items);
        console.log (troopCardsList)
      } catch (error) {
        errorReporter(error);
      }
    };
   
    loadCards();
   }, []);
   
  return (
  <>
      <h2>Cards</h2>
    <h2>Troops</h2>
    <Row>
      { (troopCards ?? []).map ((item, index) => (
        <Col sm="2" key={`troopcard${index}`}>
          <Card id={`troopcard${index}`} style={{ width: '150px', margin: '10px' }}>
            <CardImg top width="100%" src={`/cards/${item.picture}.png`} alt={item.name} />
            <UncontrolledPopover trigger="hover" placement="top" target={`troopcard${index}`}>
              <PopoverHeader>{item.name}</PopoverHeader>
              <PopoverBody>
                <p><FaInfo /> {item.description}</p>
                <p><FaFire color='orange'/> {item.areaDamage}</p>
                <p><FaHeart color='red' /> {item.hitPoints}</p>
                <p><FaUsers color='blue'/> {item.unitCount}</p>

              </PopoverBody>
            </UncontrolledPopover>
          </Card>
        </Col>
      ))}
    </Row>
    <h2>Structs</h2>
    <Row>
      {(structCards ?? []).map ((item, index) => (
        <Col sm="2" key={`structcard${index}`}>
          <Card id={`structcard${index}`} style={{ width: '150px', margin: '10px' }}>
            <CardImg top width="100%" src={`/cards/${item.picture}.png`} alt={item.name} />
            <UncontrolledPopover trigger="hover" placement="top" target={`structcard${index}`}>
              <PopoverHeader>{item.name}</PopoverHeader>
              <PopoverBody>
                <p><FaInfo /> {item.description}</p>
                <p><FaHeart /> {item.hitPoints}</p>
                <p><FaFire /> {item.AttackPaseRate}</p>
                <p><FaSquare /> {item.RangeDamage}</p>
                </PopoverBody>
            </UncontrolledPopover>
          </Card>
        </Col>
      ))}
    </Row>
    <h2>Magic</h2>
    <Row>
      { (magicCards ?? []).map ((item, index) => (
        <Col sm="2" key={`magiccard${index}`}>
          <Card id={`magiccard${index}`} style={{ width: '150px', margin: '10px' }}>
            <CardImg top width="100%" src={`/cards/${item.picture}.png`} alt={item.name} />
            <UncontrolledPopover trigger="hover" placement="top" target={`magiccard${index}`}>
              <PopoverHeader>{item.name}</PopoverHeader>
              <PopoverBody>
                <p><FaInfo /> {item.description}</p>
                <p><FaClock /> {item.DamageRadius}</p>
                <p><FaUsers /> {item.AreaDamage}</p>
                <p><FaFire /> {item.TowerDamage}</p>
                <p><FaClock /> {item.Duration}</p>
                </PopoverBody>
            </UncontrolledPopover>
          </Card>
        </Col>
      ))}
    </Row>
  </>
  )
 }
 