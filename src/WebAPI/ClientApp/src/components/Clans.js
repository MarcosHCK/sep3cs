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
import { ClanClient, ClanType } from '../webApiClient.ts'
import { Pager } from './Pager'
import { Input, Table } from 'reactstrap'
import { useErrorReporter } from './ErrorReporter'
import { useParams } from 'react-router-dom'
import { WaitSpinner } from './WaitSpinner'
import React, { useEffect, useState } from 'react'

export function Clans ()
{
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ clanClient ] = useState (new ClanClient ())
  const [ totalPages, setTotalPages ] = useState (0)
  const errorReporter = useErrorReporter ()

  const pageSize = 10
  const visibleIndices = 5

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          try {
            const paginatedList = await clanClient.getWithPagination (1, pageSize)
            return paginatedList.totalPages
          } catch (error)
            {
              errorReporter (error)
            }
        }

      const refreshPage = async () =>
        {
          try {
            const paginatedList = await clanClient.getWithPagination (activePage + 1, pageSize)

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

  return (
    (isLoading)
    ? (<WaitSpinner />)
    : (<>
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
            <th>{'Description'}</th>
            <th>{'Name'}</th>
            <th>{'Region'}</th>
            <th>{'Trophies to enter'}</th>
            <th>{'Trophies won on war'}</th>
            <th>{'Type'}</th>
          </tr>
        </thead>
        <tbody>
    { (items ?? []).map ((item, index) => (
          <tr key={`body${index}`}>
            <th scope="row">{ item.id }</th>
            <td>
              <Input disabled type='text' value={item.description} />
            </td>
            <td>
              <Input disabled type='text' value={item.name} />
            </td>
            <td>
              <Input disabled type='text' value={item.region} />
            </td>
            <td>
              <Input disabled type='text' value={item.totalTrophiesToEnter} />
            </td>
            <td>
              <Input disabled type='text' value={item.totalTrophiesWonOnWar} />
            </td>
            <td>
              <Input disabled type='text' value={ClanType[item.type]} />
            </td>
          </tr>))}
        </tbody>
      </Table>
    </>))
}
