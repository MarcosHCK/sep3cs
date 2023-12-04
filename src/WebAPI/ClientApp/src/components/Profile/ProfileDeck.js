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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with sep3cs. If not, see <http://www.gnu.org/licenses/>.
 */
import { Button, Card, CardImg, CardBody, CardTitle, Col, Row } from 'reactstrap'
import { CardClient, CreatePlayerCardCommand, DeletePlayerCardCommand } from '../../webApiClient.ts'
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap'
import { faTrash, faStar, faGift } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { PlayerCardClient, PlayerClient, UpdatePlayerCommand3, CreateCardGiftCommand, ClanClient } from '../../webApiClient.ts'
import { ProfilePage } from './ProfilePage'
import { UncontrolledPopover, PopoverHeader, PopoverBody } from 'reactstrap'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useState, useEffect } from 'react'

export function ProfileDeck (props)
{
  const { playerProfile } = props
  const [ availableCards, setAvailableCards ] = useState ([])
  const [ cardClient ] = useState (new CardClient ())
  const [ clanClient ] = useState (new ClanClient ())
  const [ clanId, setClanId ] = useState ()
  const [ deck, setDeck ] = useState ([])
  const [ dropdownOpen, setDropdownOpen ] = useState (false)
  const [ favoriteCard, setFavoriteCard ] = useState (playerProfile.favoriteCardId)
  const [ hasClan, setHasClan ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ playerCardClient ] = useState (new PlayerCardClient ())
  const [ selectedCards, setSelectedCards ] = useState ([])
  const errorReporter = useErrorReporter ()

  const loadCards = async () =>
    {
      try {
        const magicCardsList = await cardClient.getMagicCardsWithPagination (1, 100)
        const troopCardsList = await cardClient.getTroopCardsWithPagination (1, 100)
        const structCardsList = await cardClient.getStructCardsWithPagination (1, 100)
        const playerCardsList = await playerCardClient.getWithPagination (playerProfile.id, 1, 10)
        const allCardsList = [ ...magicCardsList.items, ...troopCardsList.items, ...structCardsList.items ]
        const CardMap = {}

        allCardsList.forEach (card =>
          {
            CardMap[card.id] = card;
          })

        const newDeck = playerCardsList.items.map (playerCard => CardMap[playerCard.cardId])
        setDeck (newDeck)
        const availableCardsList = allCardsList.filter (card => !newDeck.find(deckCard => deckCard.id === card.id))
        setAvailableCards(availableCardsList)
      } catch (error)
        {
          errorReporter(error);
        }
    }

  useEffect(() =>
    {
      const refreshClan = async () =>
        {
          if (!!playerProfile) try
            {
              const currentClan = await clanClient.getForPlayer (playerProfile.id)

              if (currentClan === null)
                setHasClan (false)
              else
                {
                  setHasClan (true)
                  setClanId (currentClan.clan.id)
                }
            }
          catch (error) { errorReporter (error) }
        }

      setIsLoading (true)
      refreshClan ().then (() =>
        {
          loadCards ().then (() => setIsLoading (false))
        })
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [playerProfile])

  const handleCardClick = async (card) =>
    {
      setSelectedCards ([...selectedCards, card])
      const newCard = new CreatePlayerCardCommand ()
      newCard.cardId = card.id
      newCard.playerId = playerProfile.id
      newCard.level = card.initialLevel
      await playerCardClient.create (newCard, playerProfile.id)
      await loadCards ()
    }

  const handleCardRemove = async (card) =>
    {
      const playerCardsList = await playerCardClient.getWithPagination (playerProfile.id, 1, 10)
      const cardToRemove = playerCardsList.items.find (playerCard => playerCard.cardId === card.id)
      const command = new DeletePlayerCardCommand ()

      command.cardId = cardToRemove.cardId
      command.playerId = playerProfile.id

      try {
        await playerCardClient.delete (command);
        setDeck(prevDeck => prevDeck.filter (deckCard => deckCard.id !== card.id))
      } catch (error)
        {
          errorReporter (error)
        }

      await loadCards ()
    }

  const handleFavoriteCard = async (card) =>
    {
      const playerClient = new PlayerClient ()
      const command = new UpdatePlayerCommand3 (playerProfile)

      if (command.favoriteCardId === card.id)
        command.favoriteCardId = null
      else
        command.favoriteCardId = card.id

      await playerClient.update (command)

      // Messy workaround
      setFavoriteCard (command.favoriteCardId)
      playerProfile.favoriteCardId = command.favoriteCardId
    }

  const handleGiftCardClick = async (card) =>
    {
      try{
      const command = new CreateCardGiftCommand ()
      command.cardId = card.id
      command.clanId = clanId
      command.playerId = playerProfile.id
      await playerCardClient.createCardGift (command)
      } catch(error)
        {
          errorReporter(error)
        }
    }
 
  return (
    isLoading
    ? <WaitSpinner />
    : <ProfilePage title='Cards'>
        <Row>
      { (deck ?? []).map((item, index) => (
          <Col sm="2" key={`card${index}`}>
            <Card id={`card${index}`}>
              <CardImg alt={item.name} src={`/cards/${item.picture}.png`} top width="100%" />
              <UncontrolledPopover trigger="hover" placement="top" target={`card${index}`}>
                <PopoverHeader>{item.name}</PopoverHeader>
                <PopoverBody>{item.description}</PopoverBody>
              </UncontrolledPopover>
              <CardTitle className='d-flex justify-content-center'>
                { item.name }
              </CardTitle>
              <CardBody className='d-flex justify-content-center gap-1'>
                <Button onClick={() => { setIsLoading (true); handleCardRemove (item).then (_ => setIsLoading (false)) }} >
                  <FontAwesomeIcon icon={faTrash} />
                </Button>
                <Button onClick={() => { setIsLoading (true); handleFavoriteCard (item).then (_ => setIsLoading (false)) }} >
                  <FontAwesomeIcon icon={faStar} style={{ color: favoriteCard === item.id ? 'gold' : 'gray' }} />
                </Button>
            { !hasClan
              ? <></>
              : <Button onClick={() => { setIsLoading (true); handleGiftCardClick (item).then (_ => setIsLoading (false)) }} >
                  <FontAwesomeIcon icon={faGift} />
                </Button> }
              </CardBody>
            </Card>
          </Col>))}
        </Row>

        <br />

        <Dropdown isOpen={dropdownOpen} toggle={_ => setDropdownOpen (!dropdownOpen)}>
          <DropdownToggle caret>
            +
          </DropdownToggle>
          <DropdownMenu>
          { availableCards.map((card, index) => (
            <DropdownItem key={index} onClick={() => { setIsLoading (true); handleCardClick (card).then (_ => setIsLoading (false)) }} >
              <div className='d-flex justify-content-start gap-2'>
                <img alt={ card.name } src={`/cards/${card.picture}.png`} width='16' />
                { card.name }
              </div>
            </DropdownItem> ))}
          </DropdownMenu>
        </Dropdown>
      </ProfilePage>)
}
