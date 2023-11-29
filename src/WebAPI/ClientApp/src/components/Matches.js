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
import { Button, Table } from 'reactstrap'
import { CreateMatchCommand } from '../webApiClient.ts'//probablemente de error
import { DateTime } from './DateTime.js'
import { Pager } from './Pager.js'
import { TimeSpan } from './TimeSpan.js'
import { UpdateMatchCommand } from '../webApiClient.ts'
import { useParams } from 'react-router-dom'
import { UserRoles } from '../services/AuthorizeConstants.js'
import { MatchClient } from '../webApiClient.ts'
import authService from '../services/AuthorizeService.ts'
import React, { useEffect, useState } from 'react'

export function Matches ()
{
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isAdministrator, setIsAdministrator ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const [ matchClient ] = useState (new MatchClient ())

  const pageSize = 10
  const visibleIndices = 5

  const addMatch = async () =>
    {
      const data = new CreateMatchCommand ()
        data.winnerPlayerId = 1
        data.looserPlayerId = 2
        data.beginDay = new Date ()
        data.duration = "00:00:01"
      await matchClient.create (data)
      setActivePage (-1)
    }

  const removeMatch = async (item) =>
    {
      await matchClient.delete (item.id)
      setActivePage (-1)
    }

  const updateMatch = async (item) =>
    {
      const data = new UpdateMatchCommand ()
        data.id = item.id
        data.winnerPlayerId = item.winnerPlayerId
        data.looserPlayerId = item.looserPlayerId
        data.beginDay = item.beginDay
        data.duration = item.duration
      await matchClient.update (item.id, data)
    }

  useEffect (() =>
    {
      const checkRole = async () =>
        {
          const hasRole = await authService.hasRole (UserRoles.Administrator)

          setIsAdministrator (hasRole)
        }

      setIsLoading (true)
      checkRole ().then (() => setIsLoading (false))
    }, [])

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          const paginatedList = await matchClient.getWithPagination (1, pageSize)
          return paginatedList.totalPages
        }

      const refreshPage = async () =>
        {
          const paginatedList = await matchClient.getWithPagination (activePage + 1, pageSize)

          setHasNextPage (paginatedList.hasNextPage)
          setHasPreviousPage (paginatedList.hasPreviousPage)
          setItems (paginatedList.items)
          setTotalPages (paginatedList.totalPages)
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

  return (
    isLoading
      ? (<div></div>)
    : (
      <>
        <div className='d-flex justify-content-center'>
          <Pager
            activePage={activePage}
            hasNextPage={hasNextPage}
            hasPreviousPage={hasPreviousPage}
            onPageChanged={(index) => setActivePage (index)}
            totalPages={totalPages}
            visibleIndices={visibleIndices} />
        </div>
        <div>
          <Table>
            <thead>
              <tr>
                <th>{'#'}</th>
                <th>{'Winner player'}</th>
                <th>{'Looser player'}</th>
                <th>{'Begin day'}</th>
                <th>{'Duration'}</th>
                <th />
              </tr>
            </thead>
            <tbody>
        { (items ?? []).map ((item, index) => (
              <tr key={`body${index}`}>
                <th scope="row">{ item.id }</th>
                <td>
                  <p>{ item.winnerPlayerId }</p>
                </td>
                <td>
                  <p>{ item.looserPlayerId }</p>
                </td>
                <td>
                  <DateTime
                    defaultValue={item.beginDay}
                    onChanged={(date) => { item.beginDay = date; updateMatch (item) }}
                    readOnly={!isAdministrator} />
                </td>
                <td>
                  <TimeSpan
                    defaultValue={item.duration}
                    onChanged={(span) => { item.duration = span; updateMatch (item) }}
                    readOnly={!isAdministrator} />
                </td>
        {
          (!isAdministrator)
          ? (<td />)
          : (
                <td>
                  <Button color='primary' onClick={() => removeMatch (item)} close />
                </td>)
        }
              </tr>))
        }
            </tbody>
            <tfoot>
        {
          (!isAdministrator)
          ? (<tr />)
          : (
              <tr key='footer0'>
                <td>
                  <Button color='primary' onClick={() => addMatch ()}>+</Button>
                </td>
                <td /><td /><td />
              </tr>)
        }
            </tfoot>
          </Table>
        </div>
      </>))
}
