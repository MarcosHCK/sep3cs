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
