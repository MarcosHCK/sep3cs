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
import { Alert, Button, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import { WaitSpinner } from '../WaitSpinner'
import { ProfilePage } from './ProfilePage'
import React, { useState, useEffect } from 'react';
import { Card, CardImg, Row, Col, UncontrolledPopover, PopoverHeader, PopoverBody, CardBody, CardTitle, CardText } from 'reactstrap';
import { CardClient, PlayerCardClient, CreatePlayerCardCommand, ValueTupleOfLongAndLong, DeletePlayerCardCommand, PlayerClient, UpdatePlayerCommand } from '../../webApiClient.ts'
import { useErrorReporter } from '../ErrorReporter';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faStar } from '@fortawesome/free-solid-svg-icons';


export function ProfileDeck(props) {
  const { playerProfile } = props;
  const [isLoading, setIsLoading] = useState(false);
  const [deck, setDeck] = useState([]);
  const [selectedCards, setSelectedCards] = useState([]);
  const [availableCards, setAvailableCards] = useState([]);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const cardClient = new CardClient();
  const errorReporter = useErrorReporter();
  const playerCardClient = new PlayerCardClient();
  const loadCards = async () => {
    try {
      const magicCardsList = await cardClient.getWithPagination('MagicCard', 1, 10);
      const troopCardsList = await cardClient.getWithPagination2('TroopCard', 1, 10);
      const structCardsList = await cardClient.getWithPagination3('StructCard', 1, 10);
      const playerCardsList = await playerCardClient.getWithPagination(1, 10);
      let CardMap = {};
      const allCardsList = [...magicCardsList.items, ...troopCardsList.items, ...structCardsList.items];
      allCardsList.forEach(card => {
        CardMap[card.id] = card;

      });
      let newDeck = playerCardsList.items.map(playerCard => {
        return CardMap[playerCard.cardId];
      })
      setDeck(newDeck);



      const availableCardsList = allCardsList.filter(card => {
        return !newDeck.find(deckCard => deckCard.id === card.id);
      });
      setAvailableCards(availableCardsList);



    } catch (error) {
      errorReporter(error);
    }
  };
  useEffect(() => {


    loadCards();
  }, [playerProfile]);

  const handleCardClick = (card) => {
    setSelectedCards([...selectedCards, card]);
    const newCard = new CreatePlayerCardCommand();
    newCard.cardId = card.id;
    newCard.playerId = playerProfile.id;
    newCard.level = 1; // Ajusta este valor según sea necesario
    playerCardClient.create(newCard, playerCardClient.id);
    loadCards();
  };
  const handleCardRemove = async (card) => {
    const playerCardsList = await playerCardClient.getWithPagination(1, 10);
    console.log(playerCardsList.items)
    console.log(card)
    const cardToRemove = playerCardsList.items.find(playerCard => playerCard.cardId === card.id);
    const command = new DeletePlayerCardCommand();
    command.cardId = cardToRemove.cardId;
    command.playerId = playerProfile.id;
    try {
      await playerCardClient.delete(command);
      setDeck(prevDeck => prevDeck.filter(deckCard => deckCard.id !== card.id));
    } catch (error) {
      console.error('Error al eliminar la carta', error);

    }
    loadCards();
  };
  const handleFavoriteCard =async (card) =>{
    const playerClient=new PlayerClient();
    const command=new UpdatePlayerCommand(playerProfile);
    command.favoriteCardId=card.id;
    console.log(card.id);
    console.log("cardid")
    console.log(playerProfile.favoriteCardId);
    await playerClient.update(playerProfile.id,command);

  } 

console.log (playerProfile)


  const toggle = () => setDropdownOpen(prevState => !prevState);
  
  
  if (!playerProfile) {
    return <Alert color='warning'>User has not player status</Alert>;
  } else {
    return (
      isLoading
        ? <WaitSpinner />
        : <ProfilePage title='Deck'>
          <Row>
            {(deck ?? []).map((item, index) => (
              <Col sm="2" key={`card${index}`}>
                <Card id={`card${index}`} style={{ width: '150px', margin: '10px' }}>
                  <UncontrolledPopover trigger="hover" placement="top" target={`card${index}`}>
                    <PopoverHeader>{item.name}</PopoverHeader>
                    <PopoverBody>{item.description}</PopoverBody>
                  </UncontrolledPopover>
                  <CardImg top width="100%" src={`/cards/${item.picture}.png`} alt={item.name} />
                  <CardBody>
                    <CardTitle>{item.name}</CardTitle>
                    <Button onClick={() => handleCardRemove(item)}>
                    <FontAwesomeIcon icon={faTrash} /></Button>
                    <Button onClick={() => handleFavoriteCard(item)}>
                      <FontAwesomeIcon icon={faStar} style={{ color: playerProfile.favoriteCardId == item.id ? 'gold' : 'gray' }} />
                    </Button>
                  </CardBody>
                </Card>
              </Col>
            ))}




          </Row>

          <h2>Añadir carta</h2>
          <Dropdown isOpen={dropdownOpen} toggle={toggle}>
            <DropdownToggle caret>
              Añadir carta
            </DropdownToggle>
            <DropdownMenu>
              {availableCards.map((card, index) => (
                <DropdownItem key={index} onClick={() => handleCardClick(card)}>
                  {card.name}
                </DropdownItem>
              ))}
            </DropdownMenu>
          </Dropdown>

        </ProfilePage>
    );
  }
}
