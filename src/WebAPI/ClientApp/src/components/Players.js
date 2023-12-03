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
import { PlayerClient } from '../webApiClient.ts'
import { Button, Table } from 'reactstrap'
import { useErrorReporter } from './ErrorReporter'
import { useParams } from 'react-router-dom'
import React, { useEffect, useState } from 'react'

export function Players (props)
{
  const { onPick, picker } = props
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ playerClient ] = useState (new PlayerClient ())
  const [ totalPages, setTotalPages ] = useState (0)
  const errorReporter = useErrorReporter ()

  const pageSize = !picker ? 10 : 3
  const visibleIndices = !picker ? 5 : 2

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          try {
            const paginatedList = await playerClient.getWithPagination (1, pageSize)
            return paginatedList.totalPages
          } catch (error)
            {
              errorReporter (error)
            }
        }

      const refreshPage = async () =>
        {
          try {
            const paginatedList = await playerClient.getWithPagination (activePage + 1, pageSize)

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
        <Table>
          <thead>
            <tr>
              <th>{'#'}</th>
              <th>{'Nick'}</th>
              <th>{'Level'}</th>
            { !picker ? <></> : <th /> }
            </tr>
          </thead>
          <tbody>
        { (items ?? []).map ((item, index) => (
            <tr key={`body${index}`}>
              <th scope="row">
                <p>{ item.id }</p>
              </th>
              <td>
                <p>{ item.nickname ?? "<no nickname>" }</p>
              </td>
              <td>
                <p>{ item.level }</p>
              </td>
          { !picker
            ? <></>
            : <td>
                <Button color='primary' onClick={_ => onPick (item.id)}>+</Button>
              </td>}
            </tr>))}
          </tbody>
        </Table>
      </>
    )
  )
}
