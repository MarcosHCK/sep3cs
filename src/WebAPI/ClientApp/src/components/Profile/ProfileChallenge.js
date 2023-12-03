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
import React, { useEffect, useState } from 'react';
import { Button, Table, Form, FormGroup, Input, Label } from 'reactstrap';
import { ChallengeClient, UpdateChallengeCommand ,AddPlayerCommand} from '../../webApiClient.ts';
import { useErrorReporter } from '../ErrorReporter.js';
import { WaitSpinner } from '../WaitSpinner.js';

export function ProfileChallenge(props) {
  const { playerProfile } = props
 const [challenges, setChallenges] = useState([]);
 const [selectedChallenge, setSelectedChallenge] = useState(null);
 const client = new ChallengeClient();
 const errorReporter = useErrorReporter();

 useEffect(() => {
   async function fetchChallenges() {
     try {
       const pageNumber = 1;
       const pageSize = 10;
       const paginatedChallenges = await client.getWithPagination(pageNumber, pageSize);
       const challenges = paginatedChallenges.items;
       console.log(challenges);
       setChallenges(challenges);
     } catch (error) {
       errorReporter(error);
     }
   }
   fetchChallenges();
 }, []);

 const handleJoinChallenge = async () => {
   if (selectedChallenge) {
     try {
      const command=new AddPlayerCommand();
      command.challengeId=selectedChallenge.id;
      command.playerId=playerProfile.id;
      command.wonThrophies=0;
       await client.addPlayer(command);
       alert('Has unido el desafío exitosamente');
     } catch (error) {
       errorReporter(error);
     }
   }
 };

 return (
   <div>
     <h2>Desafíos disponibles</h2>
     <Table striped bordered dark>
       <thead>
         <tr>
           <th>Nombre</th>
           <th>Descripción</th>
           <th />
         </tr>
       </thead>
       <tbody>
         {challenges.map((challenge) => (
           <tr key={challenge.id}>
             <td>{challenge.name}</td>
             <td>{challenge.description}</td>
             <td>
               <Button color='primary' onClick={() => setSelectedChallenge(challenge)}>
                Unirse a este desafío
               </Button>
             </td>
           </tr>
         ))}
       </tbody>
     </Table>
     <Button color='primary' onClick={handleJoinChallenge}>
       Unirse al desafío seleccionado
     </Button>
   </div>
 );
}
