import React, { useEffect, useState } from 'react';
import { Card, CardImg, Row, Col, UncontrolledPopover, PopoverHeader, PopoverBody } from 'reactstrap';
import { UpdateClanCommand } from '../../webApiClient.ts'

export function Cards () {
  const [troopCards, setTroopCards] = useState([]);
  const [structCards, setStructCards] = useState([]);
  const [magicCards, setMagicCards] = useState([]);
  const [selectedCards, setSelectedCards] = useState([]);
  const cardClient = new CardClient();

  useEffect(() => {
    const loadCards = async () => {
      const troopCardsList = await cardClient.getWithPagination2('TroopCard', 1, 10);
      const structCardsList = await cardClient.getWithPagination3('StructCard', 1, 10);
      const magicCardsList = await cardClient.getWithPagination('MagicCard', 1, 10);
      setTroopCards(troopCardsList.items);
      setStructCards(structCardsList.items);
      setMagicCards(magicCardsList.items);
    };
    loadCards();
  }, []);

  const handleCardClick = (card) => {
    setSelectedCards([...selectedCards, card]);
  };

  const saveSelectedCards = async () => {
    for (const card of selectedCards) {
      try {
        await cardClient.create({
          cardId: card.id,
          /* demás datos necesarios */
        });
      } catch (error) {
        console.error('Error al guardar la tarjeta', error);
      }
    }
  };

  return (
    <>
      {/* Aquí se muestran las cartas para seleccionar */}
      <h2>Tropas</h2>
      <Row>
        { (troopCards ?? []).map ((item, index) => (
          <Col sm="2" key={`troopcard${index}`}>
            <Card onClick={() => handleCardClick(item)} id={`troopcard${index}`} style={{ width: '150px', margin: '10px' }}>
              {/* Contenido de la carta */}
            </Card>
          </Col>
        ))}
      </Row>

      {/* Aquí se muestran las cartas seleccionadas */}
      <h2>Cartas seleccionadas</h2>
      <Row>
        { (selectedCards ?? []).map ((item, index) => (
          <Col sm="2" key={`selectedcard${index}`}>
            <Card id={`selectedcard${index}`} style={{ width: '150px', margin: '10px' }}>
              {/* Contenido de la carta */}
            </Card>
          </Col>
        ))}
      </Row>

      {/* Botón para guardar las cartas seleccionadas */}
      <button onClick={saveSelectedCards}>Guardar</button>
    </>
  )
}
