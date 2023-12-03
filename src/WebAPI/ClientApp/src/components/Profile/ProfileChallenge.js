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
import './Profile.css'
import { AddPlayerCommand } from '../../webApiClient.ts'
import { Alert, Button, Table } from 'reactstrap'
import { ChallengeClient } from '../../webApiClient.ts'
import { Challenges } from '../Challenges'
import { IntegerInput } from '../IntegerInput'
import { Pager } from '../Pager'
import { Popover, PopoverBody, PopoverHeader } from 'reactstrap'
import { ProfilePage } from './ProfilePage'
import { RemovePlayerCommand } from '../../webApiClient.ts'
import { UpdatePlayerCommand } from '../../webApiClient.ts'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function ProfileChallenge (props)
{
  const { playerProfile } = props
  const [ activePage, setActivePage ] = useState ()
  const [ challengeClient ] = useState (new ChallengeClient ())
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ pickerOpen, setPickerOpen ] = useState (false)
  const [ totalPages, setTotalPages ] = useState (0)
  const errorReporter = useErrorReporter ()

  const pageSize = 10
  const visibleIndices = 5

  const addPlayer = async (challengeId) =>
    {
      const command = new AddPlayerCommand ()

      command.challengeId = challengeId
      command.playerId = playerProfile.id

      try {
        await challengeClient.addPlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const removePlayer = async (item) =>
    {
      try {
        const command = new RemovePlayerCommand (item)
        await challengeClient.removePlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const updatePlayer = async (item) =>
    {
      try {
        const command = new UpdatePlayerCommand (item)
        await challengeClient.updatePlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          try {
            const playerId = playerProfile.id
            const paginatedList = await challengeClient.getForPlayer (playerId, 1, pageSize)
            return paginatedList.totalPages
          } catch (error)
            {
              errorReporter (error)
            }
        }

      const refreshPage = async () =>
        {
          try {
            const playerId = playerProfile.id
            const paginatedList = await challengeClient.getForPlayer (playerId, activePage + 1, pageSize)

            setHasNextPage (paginatedList.hasNextPage)
            setHasPreviousPage (paginatedList.hasPreviousPage)
            setItems (paginatedList.items)
            setTotalPages (paginatedList.totalPages)
          } catch (error)
            {
              errorReporter (error)
            }
        }

      if (activePage >= 0)
        {
          setIsLoading (true)
          refreshPage ().then (() => setIsLoading (false))
        }
      else
        {
          lastPage ().then ((total) =>
            {
              if (total === 0)
                setActivePage (0)
              else
                setActivePage (total - 1)
            })
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [activePage])

  if (!playerProfile)
    return <Alert color='warning'>User has not player status</Alert>
  else
    return (isLoading
    ? <WaitSpinner />
    : <ProfilePage title='Challeges taken'>
        <div className='d-flex justify-content-center'>
          <Pager
            activePage={activePage}
            hasNextPage={hasNextPage}
            hasPreviousPage={hasPreviousPage}
            onPageChanged={(index) => setActivePage (index)}
            totalPages={totalPages}
            visibleIndices={visibleIndices} />
        </div>
        <Table>
          <thead>
            <tr>
              <th>{'#'}</th>
              <th>{'Won throphies'}</th>
              <th />
            </tr>
          </thead>
          <tbody>
        { (items ?? []).map ((item, index) => (
            <tr key={`body${index}`}>
              <th scope="row">
                <p>{ item.challengeId }</p>
              </th>
              <td>
                <IntegerInput
                  defaultValue={item.wonThrophies}
                  onChanged={value => { item.wonThrophies = value; updatePlayer (item) }}
                  natural />
              </td>
              <td>
                <Button close onClick={_ => {
                    setIsLoading (true)
                    removePlayer (item).then (_ => setActivePage (-1))
                  }} />
              </td>
            </tr>))}
          </tbody>
        </Table>
        <>
          <Button
            color='primary'
            id='playerchallenge-picker-button'
            onClick={_ => setPickerOpen (!pickerOpen)} >
              +
          </Button>
          <Popover
              className='playerchallenge-picker'
              isOpen={pickerOpen}
              placement='bottom'
              target='playerchallenge-picker-button'
              toggle={_ => setPickerOpen (!pickerOpen)}>
            <PopoverHeader>
              Wars
            </PopoverHeader>
            <PopoverBody>
              <Challenges picker onPick={id =>
                {
                  setPickerOpen (false)
                  setIsLoading (true)
                  addPlayer (id).then (_ => setActivePage (-1))
                }} />
            </PopoverBody>
          </Popover>
        </>
      </ProfilePage>)
}

