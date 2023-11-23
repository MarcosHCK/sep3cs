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
import { Button, Form, FormGroup, Input, Label } from 'reactstrap'
import { PlayerClient, UpdatePlayerCommand } from '../../webApiClient.ts'
import { ProfilePage } from './ProfilePage'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function ProfilePlayer (props)
{
  const [ isLoading, setIsLoading ] = useState (false)
  const [ playerClient ] = useState (new PlayerClient ())
  const [ playerId, setPlayerId ] = useState (-1)
  const [ playerLevel, setPlayerLevel ] = useState (0)
  const [ playerNickname, setPlayerNickname ] = useState ('')
  const [ playerTotalCardsFound, setPlayerTotalCardsFound ] = useState (0)
  const [ playerTotalThrophies, setPlayerTotalThrophies ] = useState (0)
  const [ playerTotalWins, setPlayerTotalWins ] = useState (0)
  const errorReporter = useErrorReporter ()

  const onSubmit = async (e) =>
    {
      e.preventDefault ()

      const command = new UpdatePlayerCommand ()

      command.id = playerId
      command.level = playerLevel
      command.nickname = playerNickname === '' ? undefined : playerNickname
      command.totalCardsFound = playerTotalCardsFound
      command.totalThrophies = playerTotalThrophies
      command.totalWins = playerTotalWins

      try {
        await playerClient.update (playerId, command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  useEffect (() =>
    {
      const refreshForm = async () =>
        {
          try {
            const data = await playerClient.get (-1)

            setPlayerId (data.id)
            setPlayerLevel (data.level)
            setPlayerNickname (data.nickname ?? '')
            setPlayerTotalCardsFound (data.totalCardsFound)
            setPlayerTotalThrophies (data.totalThrophies)
            setPlayerTotalWins (data.totalWins)
            setIsLoading (false)
          } catch (error)
            {
              errorReporter (error)
            }
        }

      setIsLoading (true)
      refreshForm ().then (() => setIsLoading (false))
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

  return (
    isLoading
    ? <WaitSpinner />
    : <ProfilePage title='Identity'>
        <Form onSubmit={(e) => { setIsLoading (true); onSubmit (e).then (() => setIsLoading (false)) }}>
          <FormGroup floating>
            <Input id='player-input-level' type='number' onChange={(e) => setPlayerLevel (e.target.value)} value={playerLevel} />
            <Label for='player-input-level'>Level</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='player-input-nickname' type='text' onChange={(e) => setPlayerNickname(e.target.value)} value={playerNickname} />
            <Label for='player-input-nickname'>Nickname</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='player-input-total-cards-found' type='number' onChange={(e) => setPlayerTotalCardsFound (e.target.value)} value={playerTotalCardsFound} />
            <Label for='player-input-total-cards-found'>Total cards found</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='player-input-total-throphies' type='number' onChange={(e) => setPlayerTotalThrophies (e.target.value)} value={playerTotalThrophies} />
            <Label for='player-input-total-throphies'>Total throphies won</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='player-input-total-wins' type='number' onChange={(e) => setPlayerTotalWins (e.target.value)} value={playerTotalWins} />
            <Label for='player-input-total-wins'>Total battles won</Label>
          </FormGroup>
          <Button color='primary'>Update</Button>
        </Form>
      </ProfilePage>)
}
