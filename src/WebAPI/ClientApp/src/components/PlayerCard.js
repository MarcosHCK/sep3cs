import React from 'react';
import { Card, CardImg, Button } from 'reactstrap';
import React, { useState } from 'react';
import { Row, Col } from 'reactstrap';

function CardComponent({ card, onSelect }) {
  return (
    <Card>
      <CardImg top width="100%" src={card.imagePath} alt={card.name} />
      <Button onClick={() => onSelect(card)}>Seleccionar</Button>
    </Card>
  );
}



function CardSelection({ cards }) {
  const [selectedCards, setSelectedCards] = useState([]);

  const handleSelect = (card) => {
    setSelectedCards([...selectedCards, card]);
  };

  return (
    <Row>
      {cards.map((card) => (
        <Col sm="4">
          <CardComponent card={card} onSelect={handleSelect} />
        </Col>
      ))}
    </Row>
  );
}
