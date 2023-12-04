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
import {Mural} from './HomeTable';
import { FaFileDownload } from 'react-icons/fa';
import './Home.css';
import React, { useEffect, useState } from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

export function Home ()
{
  //clanes x region, falta automatizar lo de crear la variable pa poner en la tabla y revisar la query como tal 
  const [topClans, clanData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/TopClans');//topClansClient.getTopClans 
      const data = await response.json();
      clanData(data);
    };
    fetchData();
  }, []); 

  //desafios completados, falta la misma automatizacion de arriba y probar la query
  const [completedChallenges, challengeData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/CompletedChallenges'); //completedChallengesClient.getCompletedChallenges
      const data = await response.json();
      challengeData(data);
    };
    fetchData();
  }, []);

  //cartas mas donadas, falta la misma automatizacion de arriba y probar la query
  const [giftedCards, giftedData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/MostGiftedCards'); //mostGiftedCardsClient.getMostGiftedCards();
      const data = await response.json();
      giftedData(data);
    };
    fetchData();
  }, []);

  //top players by wars, falta la automatizacion y probar la query
  const [warIds, setWarIds] = useState([]);
  const [dropdownOpenWar, setDropdownOpenWar] = useState(false);
  const [selectedWarId, setSelectedWarId] = useState(null);
  const [bestPlayers, setBestPlayers] = useState([]);

  useEffect(() => {
    fetch('/api/BestPlayer')
      .then(response => response.json())
      .then(data => { setBestPlayers(data); console.log(data) });
  }, []);

  useEffect(() => {
    fetch('/api/AllWarIds')
      .then(response => response.json())
      .then(data => { setWarIds(data); console.log(data) });
  }, []);

  useEffect(() => {
    if (selectedWarId) {
      fetch(`/api/BestPlayer?warId=${selectedWarId}`)
        .then(response => response.json())
        .then(data => {
          setBestPlayers(data);
          console.log('selectedWarId:', selectedWarId);
          console.log('bestPlayers:', data);
        });
    }
  }, [selectedWarId]);

  const toggleWar = () => setDropdownOpenWar(prevState => !prevState);

  //popular card en clanes, falta la automatizacion y probar la query
  const [popularCards, setPopularCards] = useState([]);
  const [clanName, setClanNames] = useState([]);
  const [dropdownOpenClan, setDropdownOpenClan] = useState(false);
  const [selectedClanName, setSelectedClanName] = useState(null);

  useEffect(() => {
    fetch('/api/PopularCards')
      .then(response => response.json())
      .then(data => {setPopularCards(data); console.log(data)});
  }, []);

  useEffect(() => {
    fetch('/api/AllClanNames')
      .then(response => response.json())
      .then(data => {setClanNames(data); console.log(data)});
  }, []);

  useEffect(() => {
    if (selectedWarId) {
      fetch(`/api/PopularCards?clanName=${selectedClanName}`)
        .then(response => response.json())
        .then(data =>{setPopularCards(data); console.log(data);
        console.log('selectedclanName:', selectedClanName);
        console.log('popularcards:', data);} )
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedClanName]);

  const toggleClan = () => setDropdownOpenClan(prevState => !prevState);

  return (
    <div>
      <div className="home">
        <div className="carousel">
          <HomeCarousel items={[
            { src: '/comic/administrator.jpeg', },
            { src: '/comic/query_card_stats.jpeg', },
            { src: '/comic/query_challenge_stats.jpeg', },
            { src: '/comic/query_clan_stats.jpeg', },
            { src: '/comic/query_clan_war_stats.jpeg', },
            { src: '/comic/registration.jpeg', },
          ]} />
        </div>
        <Mural />
      </div>

      <h1 className="table-title d-flex">
        <img alt='' className="table-image" src="clanes.jfif" />
            Top Clans by Regions
        <span className='flex-grow-1' />
        <a href='/api/TopClans/Export?contentType=text%2Fcsv'><FaFileDownload /></a>
      </h1>
      <Table columns={['Clan', 'Region', 'Trophies']} data={topClans} />

      <h1 className="table-title d-flex">
        <img alt='' className="table-image" src="top_players.webp" />
            Top Players by Wars
        <span className='flex-grow-1' />
        <Dropdown isOpen={dropdownOpenWar} toggle={toggleWar}>
          <DropdownToggle caret>
            Select a war
          </DropdownToggle>
          <DropdownMenu>
        { warIds.map((id, index) => (
            <DropdownItem key={index} onClick={() => setSelectedWarId (id)}>
              {id}
            </DropdownItem>))}
          </DropdownMenu>
        </Dropdown>
        <a href={`/api/BestPlayer/Export?contentType=text%2Fcsv&warId=${selectedWarId}`}><FaFileDownload /></a>
      </h1>
      <Table columns={['Player', 'Clan', 'Trophies']} data={bestPlayers} />

      <h1 className="table-title d-flex">
        <img alt='' className="table-image" src="desafio.jfif" />
            Completed Challenges
        <span className='flex-grow-1' />
        <a href='/api/CompletedChallenges/Export?contentType=text%2Fcsv'><FaFileDownload /></a>
      </h1>
      <Table columns={['Player', 'Challenge']} data={completedChallenges} />
          
      <h1 className="table-title d-flex">
        <img alt='' className="table-image" src="cards1.jfif" />
            Most Popular Cards by Clans
        <span className='flex-grow-1' />
        <Dropdown isOpen={dropdownOpenClan} toggle={toggleClan}>
          <DropdownToggle caret>
            Select a clan
          </DropdownToggle>
          <DropdownMenu>
        { clanName.map((name, index) => (
            <DropdownItem key={index} onClick={() => setSelectedClanName (name)}>
              {name}
            </DropdownItem>))}
          </DropdownMenu>
        </Dropdown>
        <a href={`/api/PopularCards/Export?contentType=text%2Fcsv&clanName=${selectedClanName}`}><FaFileDownload /></a>
      </h1>
      <Table columns={['Card', 'Type', 'Clan']} data={popularCards} />

      <h1 className="table-title d-flex">
        <img alt='' className="table-image" src="cards3.jfif" />
            Most Gifted Cards by Region
        <span className='flex-grow-1' />
        <a href='/api/MostGiftedCards/Export?contentType=text%2Fcsv'><FaFileDownload /></a>
      </h1>
      <Table columns={['Card', 'Region', 'Donations']} data={giftedCards} />
    </div>
  )
}

