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
import { TopClansClient } from '../webApiClient.ts'
import { CompletedChallengesClient } from '../webApiClient.ts';
import { MostGiftedCardsClient } from '../webApiClient.ts';


export function Home() {

  //clanes x region, falta automatizar lo de crear la variable pa poner en la tabla y revisar la query como tal 
  const [topClansClient] = useState(new TopClansClient())
  const [topClans, clanData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/TopClans');//topClansClient.getTopClans 
      const data = await response.json();
      clanData(data);
    };
    fetchData();
  }, []);


  const topClans2 = topClans.length > 0 ? [{ Clan: topClans[0][0], Region: topClans[0][1], Trofeos: topClans[0][2] }] : [];


  //desafios completados, falta la misma automatizacion de arriba y probar la query
  const [completedChallengesClient] = useState(new CompletedChallengesClient())
  const [completedChallenges, challengeData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/CompletedChallenges'); //completedChallengesClient.getCompletedChallenges
      const data = await response.json();
      challengeData(data);
    };
    fetchData();
  }, []);

  const completedChallenges2 = completedChallenges.length > 0 ? [{ Player: completedChallenges[0][0], Challenge: completedChallenges[0][1] }] : [];

  //cartas mas donadas, falta la misma automatizacion de arriba y probar la query
  const[mostGiftedCardsClient] = useState(new MostGiftedCardsClient())
  const [giftedCards, giftedData] = useState([]);
 
  useEffect(() => {
    const fetchData = async () => {
      const response = await mostGiftedCardsClient.getMostGiftedCards();//fetch('/api/MostGiftedCards');
      const data = await response.json();
      giftedData(data);
    };
    fetchData();
   }, []);
   
  const giftedCards2 = giftedCards.length > 0 ? [{ Card: giftedCards[0][0], Region: giftedCards[0][1], Donations: giftedCards[0][2] }] : [];
 

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

      <h1>Top Clans by Regions</h1>
      <Table columns={['Clan', 'Region', 'Trofeos']} data={topClans2} />

      <h1>Completed Challenges</h1>
      <Table columns={['Player', 'Challenge']} data={completedChallenges2} />

      <h1>Most Gifted Cards by Region</h1>
      <Table columns={['Card', 'Region', 'Donations']} data={giftedCards2} />
 



    </div>


  )

  /* const [bestPlayers, setBestPlayers] = useState([]);
   const [warIds, setWarIds] = useState([]);
   const [dropdownOpenWar, setDropdownOpenWar] = useState(false);
   const [selectedWarId, setSelectedWarId] = useState(null);
 
   useEffect(() => {
     fetch('/api/BestPlayers')
       .then(response => response.json())
       .then(data => {setBestPlayers(data); console.log(data)});
   }, []); 
 
   useEffect(() => {
     fetch('/api/BestPlayers/warIds')
       .then(response => response.json())
       .then(data => {setWarIds(data); console.log(data)});
   }, []); 
 
   useEffect(() => {
     if (selectedWarId) {
       fetch(`/api/BestPlayers/${selectedWarId}`)
         .then(response => response.json())
         .then(data => { setBestPlayers(data); console.log(data)} );
     }
   }, [selectedWarId]);
 
   const toggleWar = () => setDropdownOpenWar(prevState => !prevState);
 
 
   //
   const [topClans, setTopClans] = useState([]);
 
   useEffect(() => {
     console.log('Ejecutando useEffect para BestClans');
     fetch('/api/BestClans')
       .then(response => response.json())
       .then(data => {setTopClans(data); console.log(data);});
       
   }, []);
 
   //
   const [completedChallenges, setCompletedChallenges] = useState([]);
   useEffect(() => {
     fetch('/api/CompletedChallenges')
       .then(response => response.json())
       .then(data => {setCompletedChallenges(data); console.log(data)});
    }, []);
     
    //
    const [popularCards, setPopularCards] = useState([]);
    const [clanName, setClanNames] = useState([]);
    const [dropdownOpenClan, setDropdownOpenClan] = useState(false);
    const [selectedClanName, setSelectedClanName] = useState(null);
  
    useEffect(() => {
      fetch('/api/MostPopularCards')
        .then(response => response.json())
        .then(data => {setPopularCards(data); console.log(data)});
    }, []);
  
    useEffect(() => {
      fetch('/api/MostPopularCards/clanNames')
        .then(response => response.json())
        .then(data => {setClanNames(data); console.log(data)});
    }, []);
  
    useEffect(() => {
      if (selectedWarId) {
        fetch(`/api/MostPopularCards/${selectedClanName}`)
          .then(response => response.json())
          .then(data =>{setPopularCards(data); console.log(data)} );
      }
    }, [selectedClanName]);
  
    const toggleClan = () => setDropdownOpenClan(prevState => !prevState);
 
     //
    const [mostDonatedCards, setMostDonatedCards] = useState([]);
 
    useEffect(() => {
     fetch('/api/MostGiftedCards')
       .then(response => response.json())
       .then(data => {setMostDonatedCards(data); console.log(data)});
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
 
 
   
    </div >
   )*/
}
