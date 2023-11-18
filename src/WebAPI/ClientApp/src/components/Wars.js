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
import { Pager } from './Pager'
import { Table } from 'reactstrap'
import { useParams } from 'react-router-dom'
import { WarClient } from '../webApiClient.ts'
import React, { useEffect, useState } from 'react'

export function Wars ()
{
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const [ warClient ] = useState (new WarClient ())

  const pageSize = 10
  const visibleIndices = 5

  useEffect (() =>
    {
      const refreshPage = async () =>
        {
          const paginatedList = await warClient.getWithPagination (activePage + 1, pageSize)

          setHasNextPage (paginatedList.hasNextPage)
          setHasPreviousPage (paginatedList.hasPreviousPage)
          setItems (paginatedList.items)
          setTotalPages (paginatedList.totalPages)
        }

      setIsLoading (true)
      refreshPage ().then (() => setIsLoading (false))
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
                <th>{'Begin day'}</th>
                <th>{'Duration'}</th>
              </tr>
            </thead>
            <tbody>
        { (items ?? []).map ((item, index) => (
              <tr key={`0${index}`}>
                <th scope="row">{ item.id }</th>
                <td>{ item.beginDay.toString () }</td>
                <td>{ item.duration.toString () }</td>
              </tr>))
        }
            </tbody>
          </Table>
        </div>
      </>))
}
