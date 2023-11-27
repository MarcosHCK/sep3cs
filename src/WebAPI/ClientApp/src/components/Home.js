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
import { HomeCarousel } from './HomeCarousel'
import Table from './HomeTable';
import './Home.css';
import React, { useEffect, useState } from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

export function Home ()
{
 const [bestPlayers, setBestPlayers] = useState([]);
 const [warIds, setWarIds] = useState([]);
 const [dropdownOpen, setDropdownOpen] = useState(false);
 const [selectedWarId, setSelectedWarId] = useState(null);

 useEffect(() => {
 fetch('/api/BestPlayers')
  .then(response => response.json())
  .then(data => setBestPlayers(data));
}, []);

 useEffect(() => {
 fetch('/api/BestPlayers/warIds')
  .then(response => response.json())
  .then(data => setWarIds(data));
}, []);

 useEffect(() => {
 if (selectedWarId) {
 fetch(`/api/BestPlayers/${selectedWarId}`)
   .then(response => response.json())
   .then(data => setBestPlayers(data));
 }
}, [selectedWarId]);

 const toggle = () => setDropdownOpen(prevState => !prevState);

 const columns = ['Jugador', 'Clan', 'Trofeos'];
 const data = [
 { Jugador: 'Jugador 1', Clan: 'Clan 1', Trofeos: 100 },
 { Jugador: 'Jugador 2', Clan: 'Clan 2', Trofeos: 80 },

 // Agrega más jugadores aquí
 ];

 const clanColumns = ['Clan', 'Guerras ganadas', 'Trofeos'];
 const clanData = [
 { Clan: 'Clan 1', 'Guerras ganadas': 10, Trofeos: 100 },
 { Clan: 'Clan 2', 'Guerras ganadas': 8, Trofeos: 80 },
 { Clan: 'Clan 3', 'Guerras ganadas': 6, Trofeos: 60 },
 // Agrega más clanes aquí
 ];

 const cardsColumns = ['Carta', 'Cantidad de donaciones'];
 const cardsData = [
 { Carta: 'Carta 1', 'Cantidad de donaciones': 10 },
 { Carta: 'Carta 2', 'Cantidad de donaciones': 8 },
 { Carta: 'Carta 3', 'Cantidad de donaciones': 6},
 // Agrega más cartas aquí
 ];

 const favColumns = ['Carta'];
 const favData = [
 { Carta: 'Carta 1' },
 { Carta: 'Carta 2'},
 // Agrega más jugadores aquí
 ];

 const challengeColumns = ['Challenge', 'Player'];
 const challengeData = [
 { Challenge: 'Challenge 1', Player : 'Player 1' },
 
 // Agrega más jugadores aquí
 ];

 return (
 <div>
  <HomeCarousel items={[
    { caption : 'Administrator working', src : '/comic/administrator.jpeg', },
    { caption : 'Querying cards statistics', src : '/comic/query_card_stats.jpeg', },
    { caption : 'Querying challenges statistics', src : '/comic/query_challenge_stats.jpeg', },
    { caption : 'Querying clans statistics', src : '/comic/query_clan_stats.jpeg', },
    { caption : 'Querying wars statistics', src : '/comic/query_clan_war_stats.jpeg', },
    { caption : 'Querying matches statistics', src : '/comic/query_match_stats.jpeg', },
    { caption : 'User signing up for DataClash', src : '/comic/registration.jpeg', },
  ]} />
  <div className="tableContainer">

<div className="headerContainer">
 <h1>Top Players in Wars</h1>
 <Dropdown isOpen={dropdownOpen} toggle={toggle}>
   <DropdownToggle caret>
     Select a war
   </DropdownToggle>
   <DropdownMenu>
     {warIds.map((id, index) => (
       <DropdownItem key={index} onClick={() => setSelectedWarId(id)}>
         {id}
       </DropdownItem>
     ))}
   </DropdownMenu>
 </Dropdown>
</div>
<Table columns={['Jugador', 'Clan', 'Trofeos']} data={bestPlayers} />


    <h1>Top Clans in Regions</h1>
    <Table columns={clanColumns} data={clanData} />
    <h1>Top Cards in Regions</h1>
    <Table columns={cardsColumns} data={cardsData} />
    <h1>Most Popular Cards in Clans</h1>
    <Table columns={favColumns} data={favData} />
    <h1>Complete Challenges</h1>
    <Table columns={challengeColumns} data={challengeData} />
  </div>
 </div>
 )
}
