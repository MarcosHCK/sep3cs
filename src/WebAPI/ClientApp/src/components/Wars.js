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
import { CreateWarCommand } from '../webApiClient.ts'
import { DateTime } from './DateTime'
import { Pager } from './Pager'
import { TimeSpan } from './TimeSpan'
import { UpdateWarCommand } from '../webApiClient.ts'
import { useAuthorize } from '../services/AuthorizeProvider'
import { useParams } from 'react-router-dom'
import { UserRoles } from '../services/AuthorizeConstants'
import { WarClient } from '../webApiClient.ts'
import React, { useEffect, useState } from 'react'

export function Wars ()
{
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ isReady, isAuthorized, inRole ] = useAuthorize ()
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const [ warClient ] = useState (new WarClient ())

  const pageSize = 10
  const visibleIndices = 5

  const addWar = async () =>
    {
      const data = new CreateWarCommand ()
        data.beginDay = new Date ()
        data.duration = "00:00:01"
      await warClient.create (data)
      setActivePage (-1)
    }

  const removeWar = async (item) =>
    {
      await warClient.delete (item.id)
      setActivePage (-1)
    }

  const updateWar = async (item) =>
    {
      const data = new UpdateWarCommand ()
        data.id = item.id
        data.beginDay = item.beginDay
        data.duration = item.duration
      await warClient.update (item.id, data)
    }

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          const paginatedList = await warClient.getWithPagination (1, pageSize)
          return paginatedList.totalPages
        }

      const refreshPage = async () =>
        {
          const paginatedList = await warClient.getWithPagination (activePage + 1, pageSize)

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
    (isLoading || !isReady || !isAuthorized)
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
                  <DateTime
                    defaultValue={item.beginDay}
                    onChanged={(date) => { item.beginDay = date; updateWar (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <TimeSpan
                    defaultValue={item.duration}
                    onChanged={(span) => { item.duration = span; updateWar (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
        {
          (!inRole[UserRoles.Administrator])
          ? (<td />)
          : (
                <td>
                  <Button color='primary' onClick={() => removeWar (item)} close />
                </td>)
        }
              </tr>))
        }
            </tbody>
            <tfoot>
        {
          (!inRole[UserRoles.Administrator])
          ? (<tr />)
          : (
              <tr key='footer0'>
                <td>
                  <Button color='primary' onClick={() => addWar ()}>+</Button>
                </td>
                <td /><td /><td />
              </tr>)
        }
            </tfoot>
          </Table>
        </div>
      </>))
}
