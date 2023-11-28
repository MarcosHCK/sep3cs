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

export function Home() {
  
  const [bestPlayers, setBestPlayers] = useState([]);
  const [warIds, setWarIds] = useState([]);
  const [dropdownOpenWar, setDropdownOpenWar] = useState(false);
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

  const toggleWar = () => setDropdownOpenWar(prevState => !prevState);

  const [topClans, setTopClans] = useState([]);

  useEffect(() => {
    fetch('/api/BestClans')
      .then(response => response.json())
      .then(data => setTopClans(data));
  }, []);


  //
  const [completedChallenges, setCompletedChallenges] = useState([]);
  useEffect(() => {
    fetch('/api/CompletedChallenges')
      .then(response => response.json())
      .then(data => setCompletedChallenges(data));
   }, []);
    
   //
   const [popularCards, setPopularCards] = useState([]);
   const [clanName, setClanNames] = useState([]);
   const [dropdownOpenClan, setDropdownOpenClan] = useState(false);
   const [selectedClanName, setSelectedClanName] = useState(null);
 
   useEffect(() => {
     fetch('/api/MostPopularCards')
       .then(response => response.json())
       .then(data => setPopularCards(data));
   }, []);
 
   useEffect(() => {
     fetch('/api/MostPopularCards/clanNames')
       .then(response => response.json())
       .then(data => setClanNames(data));
   }, []);
 
   useEffect(() => {
     if (selectedWarId) {
       fetch(`/api/MostPopularCards/${selectedClanName}`)
         .then(response => response.json())
         .then(data => setPopularCards(data));
     }
   }, [selectedClanName]);
 
   const toggleClan = () => setDropdownOpenClan(prevState => !prevState);

    //
   const [mostDonatedCards, setMostDonatedCards] = useState([]);

   useEffect(() => {
    fetch('/api/MostGiftedCards')
      .then(response => response.json())
      .then(data => setMostDonatedCards(data));
   }, []);
   

  return (
    <div>
      <HomeCarousel items={[
        { caption: 'Administrator working', src: '/comic/administrator.jpeg', },
        { caption: 'Querying cards statistics', src: '/comic/query_card_stats.jpeg', },
        { caption: 'Querying challenges statistics', src: '/comic/query_challenge_stats.jpeg', },
        { caption: 'Querying clans statistics', src: '/comic/query_clan_stats.jpeg', },
        { caption: 'Querying wars statistics', src: '/comic/query_clan_war_stats.jpeg', },
        { caption: 'Querying matches statistics', src: '/comic/query_match_stats.jpeg', },
        { caption: 'User signing up for DataClash', src: '/comic/registration.jpeg', },
      ]} />
      <div className="tableContainer">

        <div className="headerContainer">
          <h1>Top Players by Wars</h1>
          <Dropdown isOpen={dropdownOpenWar} toggle={toggleWar}>
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

        <h1>Top Clans by Regions</h1>
        <Table columns={['Clan', 'Region', 'Trofeos']} data={topClans} />

        <h1>Completed Challenges</h1>
        <Table columns={['Player', 'Challenge']} data={completedChallenges} />

        <div className="headerContainer">
          <h1>Most Popular Cards by Clans</h1>
          <Dropdown isOpen={dropdownOpenClan} toggle={toggleClan}>
            <DropdownToggle caret>
              Select a clan
            </DropdownToggle>
            <DropdownMenu>
              {clanName.map((name, index) => (
                <DropdownItem key={index} onClick={() => setSelectedClanName(name)}>
                  {name}
                </DropdownItem>
              ))}
            </DropdownMenu>
          </Dropdown>
        </div>
        <Table columns={['Card', 'Type', 'Clan']} data={popularCards} />

        <h1>Most Donated Cards by Region</h1>
        <Table columns={['Card', 'Region', 'Amount of Donations']} data={mostDonatedCards} />


      </div>
    </div>
  )
}
