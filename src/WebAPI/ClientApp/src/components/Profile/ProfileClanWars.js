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
import './ProfileClanWars.css'
import { Alert, Button, Input, Table } from 'reactstrap'
import { ClanClient, ClanRole } from '../../webApiClient.ts'
import { EnterWarCommand } from '../../webApiClient.ts'
import { LeaveWarCommand } from '../../webApiClient.ts'
import { UpdateWarCommand } from '../../webApiClient.ts'
import { Pager } from '../Pager'
import { Popover, PopoverBody, PopoverHeader } from 'reactstrap'
import { ProfilePage } from './ProfilePage'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import { Wars } from '../Wars'
import React, { useEffect, useState } from 'react'

export function ProfileClanWars (props)
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
  const [ pickerOpen, setPickerOpen ] = useState (false)
  const [ totalPages, setTotalPages ] = useState (0)
  const errorReporter = useErrorReporter ()

  const pageSize = 10
  const visibleIndices = 5

  const enterWar = async (warId) =>
    {
      const command = new EnterWarCommand ()
        command.clanId = clanId
        command.warId = warId

      try {
        await clanClient.enterWar (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const leaveWar = async (item) =>
    {
      try {
        const command = new LeaveWarCommand (item)
        await clanClient.leaveWar (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const updateWar = async (item) =>
    {
      try {
        const command = new UpdateWarCommand (item)
        await clanClient.updateWar (command)
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
            const paginatedList = await clanClient.getWarClansWithPagination (clanId, 1, pageSize)
            return paginatedList.totalPages
          } catch (error)
            {
              errorReporter (error)
            }
        }

      const refreshPage = async () =>
        {
          try {
            const paginatedList = await clanClient.getWarClansWithPagination (clanId, activePage + 1, pageSize)

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

  if (!playerProfile)
    return <Alert color='warning'>User has not player status</Alert>
  else if (!hasClan)
    return <Alert color='danger'>You do not belong to any clan</Alert>
  else
    return (
      isLoading
      ? <WaitSpinner />
      : <ProfilePage title='Wars fought by our clan'>
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
                <th>{'Throphies won'}</th>
                <th />
              </tr>
            </thead>
            <tbody>
          { (items ?? []).map ((item, index) => (
              <tr key={`body${index}`}>
                <th scope="row">
                  <p>{ item.warId }</p>
                </th>
                <td>
                  <Input disabled={clanRole !== ClanRole.Chief}
                      defaultValue={item.wonThrophies}
                      onChange={e => { item.wonThrophies = Number (e.target.value); updateWar (item) }}
                      type='number' />
                </td>
              { clanRole !== ClanRole.Chief
                ? <></>
                : <td>
                    <Button close onClick={_ => {
                        setIsLoading (true)
                        leaveWar (item).then (_ => setActivePage (-1))
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
              id='warclan-picker-button'
              onClick={_ => setPickerOpen (!pickerOpen)} >
                +
            </Button>
            <Popover
                className='warclan-picker'
                isOpen={pickerOpen}
                placement='bottom'
                target='warclan-picker-button'
                toggle={_ => setPickerOpen (!pickerOpen)}>
              <PopoverHeader>
                Wars
              </PopoverHeader>
              <PopoverBody>
                <Wars picker onPick={warId =>
                  {
                    setPickerOpen (false)
                    setIsLoading (true)
                    enterWar (warId).then (_ => setActivePage (-1))
                  }} />
              </PopoverBody>
            </Popover>
          </>}
        </ProfilePage>)
}
