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
import { AddPlayerCommand2 } from '../../webApiClient.ts'
import { Alert, Button, Input, Table } from 'reactstrap'
import { ClanClient, ClanRole } from '../../webApiClient.ts'
import { Pager } from '../Pager'
import { Players } from '../Players'
import { PopoverBody, PopoverHeader } from 'reactstrap'
import { ProfilePage } from './ProfilePage'
import { RemovePlayerCommand2 } from '../../webApiClient.ts'
import { UncontrolledPopover } from 'reactstrap'
import { UpdatePlayerCommand2 } from '../../webApiClient.ts'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function ProfileClanPlayers (props)
{
  const { playerProfile } = props
  const [ activePage, setActivePage ] = useState ()
  const [ clanClient ] = useState (new ClanClient ())
  const [ clanId, setClanId ] = useState ()
  const [ clanRole, setClanRole ] = useState ()
  const [ hasClan, setHasClan ] = useState (false)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const errorReporter = useErrorReporter ()

  const pageSize = 10
  const visibleIndices = 5

  const addPlayer = async (playerId) =>
    {
      const command = new AddPlayerCommand2 ()

      command.clanId = clanId
      command.playerId = playerId
      command.role = ClanRole.Commoner

      try {
        await clanClient.addPlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const removePlayer = async (item) =>
    {
      try {
        const command = new RemovePlayerCommand2 (item)
        await clanClient.removePlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const updatePlayer = async (item) =>
    {
      try {
        const command = new UpdatePlayerCommand2 (item)
        await clanClient.updatePlayer (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  useEffect (() =>
    {
      const refreshClan = async () =>
        {
          if (!!playerProfile) try
            {
              const currentClan = await clanClient.getForCurrentPlayer ()

              if (currentClan === null)
                setHasClan (false)
              else
                {
                  setHasClan (true)
                  setClanId (currentClan.clan.id)
                  setClanRole (currentClan.role)
                }
            }
          catch (error) { errorReporter (error) }
        }

      setIsLoading (true)
      refreshClan ().then (() => setIsLoading (false))
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [playerProfile])

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          try {
            const paginatedList = await clanClient.getPlayerClansWithPagination (clanId, 1, pageSize)
            return paginatedList.totalPages
          } catch (error)
            {
              errorReporter (error)
            }
        }

      const refreshPage = async () =>
        {
          try {
            const paginatedList = await clanClient.getPlayerClansWithPagination (clanId, activePage + 1, pageSize)

            setHasNextPage (paginatedList.hasNextPage)
            setHasPreviousPage (paginatedList.hasPreviousPage)
            setItems (paginatedList.items)
            setTotalPages (paginatedList.totalPages)
          } catch (error)
            {
              errorReporter (error)
            }
        }

      if (hasClan)
        {
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
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [activePage, clanId])

  const clanRoles = Object.keys (ClanRole).filter (k => !isNaN (Number (ClanRole[k])))

  if (!hasClan)
    return <Alert color='danger'>You do not belong to any clan</Alert>
  else
    return (
      isLoading
      ? <WaitSpinner />
      : <ProfilePage title='Clan members'>
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
                <th>{'Nickname'}</th>
                <th>{'Role'}</th>
                <th />
              </tr>
            </thead>
            <tbody>
          { (items ?? []).map ((item, index) => (
              <tr key={`body${index}`}>
                <th scope="row">
                  <p>{ item.playerId }</p>
                </th>
                <td>
                  {' '}
                </td>
                <td>
                  <Input
                    disabled={clanRole !== ClanRole.Chief || item.role === ClanRole.Chief}
                    onChange={e => { item.role = ClanRole[e.target.value]; updatePlayer (item) }}
                    defaultValue={clanRoles.find (k => k === ClanRole[item.role])}
                    type='select' >
                  { clanRoles
                      .filter (t => t !== ClanRole[ClanRole.Chief] || item.role === ClanRole.Chief)
                      .map ((type, i) => <option key={`role${i}`}>{type}</option>) }
                  </Input>
                </td>
                { (clanRole !== ClanRole.Chief || item.role === ClanRole.Chief)
                  ? <td />
                  : <td>
                      <Button close onClick={_ => {
                          setIsLoading (true)
                          removePlayer (item).then (_ => setActivePage (-1))
                        }} />
                    </td> }
              </tr>))}
            </tbody>
          </Table>
        { clanRole !== ClanRole.Chief
        ? <></>
        : <>
            <Button
              color='primary'
              id='playerclan-picker-button' >
                +
            </Button>
            <UncontrolledPopover
                className='playerclan-picker'
                placement='bottom'
                target='playerclan-picker-button'
                trigger='focus'>
              <PopoverHeader>
                Wars
              </PopoverHeader>
              <PopoverBody>
                <Players picker onPick={warId =>
                  {
                    setIsLoading (true)
                    addPlayer (warId).then (_ => setActivePage (-1))
                  }} />
              </PopoverBody>
            </UncontrolledPopover>
          </>}
        </ProfilePage>)
}
