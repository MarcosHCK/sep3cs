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
import { Button, Table,Input } from 'reactstrap'
import { ChallengeClient } from '../webApiClient.ts'
import { CreateChallengeCommand, DeleteChallengeCommand } from '../webApiClient.ts'
import { DateTime } from './DateTime'
import { IntegerInput } from './IntegerInput'
import { Pager } from './Pager'
import { TimeSpan } from './TimeSpan'
import { UpdateChallengeCommand } from '../webApiClient.ts'
import { useAuthorize } from '../services/AuthorizeProvider'
import { useErrorReporter } from './ErrorReporter'
import { useParams } from 'react-router-dom'
import { UserRoles } from '../services/AuthorizeConstants'
import { WaitSpinner } from './WaitSpinner'
import React, { useEffect, useState } from 'react'

export function Challenges (props)
{
  const { onPick, picker } = props
  const { initialPage } = useParams ()
  const { isAuthorized, inRole } = useAuthorize ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const [ challengeClient ] = useState (new ChallengeClient ())
  const errorReporter = useErrorReporter ()

  const pageSize = !picker ? 10 : 3
  const visibleIndices = !picker ? 5 : 2

  const addChallenege = async () =>
    {
      const data = new CreateChallengeCommand ()

      data.beginDay = new Date ()
      data.duration = "00:00:01"
      data.bounty = 1
      data.cost = 1
      data.description = "<no text>"
      data.maxLooses =1
      data.minLevel =1
      data.name = "<no text>"

      try {
        await challengeClient.create (data)
        setActivePage (-1)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const removeChallenge = async (item) =>
    {
      try {
        const command = new DeleteChallengeCommand (item)
        await challengeClient.delete (command)
        setActivePage (-1)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  const updateChallenge = async (item) =>
    {
      try {
        const command = new UpdateChallengeCommand (item)
        console.log(command)
        await challengeClient.update (command)
      } catch (error)
        {
          errorReporter (error)
        }
    }

  useEffect (() =>
    {
      const lastPage = async () =>
        {
          try{
          const paginatedList = await challengeClient.getWithPagination (1, pageSize)
          return paginatedList.totalPages
          }
          catch(error)
          {
            errorReporter (error)
            return 0
          }
        }

      const refreshPage = async () =>
        {
          try {
            const paginatedList = await challengeClient.getWithPagination (activePage + 1, pageSize)

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

  const readOnly = !inRole[UserRoles.Administrator] || !!picker

  return (
    isLoading || !isAuthorized
    ? <WaitSpinner />
    : <>
        <div className='d-flex justify-content-center'>
          <Pager
            activePage={activePage}
            hasNextPage={hasNextPage}
            hasPreviousPage={hasPreviousPage}
            onPageChanged={(index) => setActivePage (index)}
            totalPages={totalPages}
            visibleIndices={visibleIndices} />
        </div>
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>{'#'}</th>
              <th>{'Begin day'}</th>
              <th>{'Bounty'}</th>
              <th>{'Cost'}</th>
              <th>{'Description'}</th>
              <th>{'Duration'}</th>
              <th>{'MaxLooses'}</th>
              <th>{'MinLevel'}</th>
              <th>{'Name'}</th>
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
                  onChanged={(date) => { item.beginDay = date; updateChallenge (item) }}
                  readOnly={readOnly} />
              </td>
              <td>
                <IntegerInput
                  defaultValue={item.bounty}
                  disabled={readOnly}
                  natural nonZero
                  onChanged={value => { item.bounty = value; updateChallenge (item) }} />
              </td>
              <td>
                <IntegerInput
                  defaultValue={item.cost}
                  disabled={readOnly}
                  natural nonZero
                  onChanged={value => { item.cost = value; updateChallenge (item) }} />
              </td>
              <td>
                <Input
                  type='text'
                  defaultValue={item.description}
                  onChange={(e) => { item.description = e.target.value; updateChallenge (item) }}
                  readOnly={readOnly} />
              </td>
              <td>
                <TimeSpan
                  defaultValue={item.duration}
                  onChanged={(span) => { item.duration = span; updateChallenge (item) }}
                  readOnly={readOnly} />
              </td>
              <td>
                <IntegerInput
                  defaultValue={item.maxLooses}
                  disabled={readOnly}
                  natural nonZero
                  onChanged={value => { item.maxLooses = value; updateChallenge (item) }} />
              </td>
              <td>
                <IntegerInput
                  defaultValue={item.minLevel}
                  disabled={readOnly}
                  natural nonZero
                  onChanged={value => { item.minLevel = value; updateChallenge (item) }} />
              </td>
              <td>
                <Input
                  type='text'
                  defaultValue={item.name}
                  onChange={(e) => { item.name = e.target.value; updateChallenge (item) }}
                  readOnly={readOnly} />
              </td>
          { readOnly && !picker
            ? <td />
            : (!picker
              ? <td>
                  <Button color='primary' onClick={() => removeChallenge (item)} close />
                </td>
              : <td>
                  <Button color='primary' onClick={() => onPick (item.id)}>+</Button>
                </td>)}
            </tr>))}
          </tbody>
          <tfoot>
        { readOnly ? <tr />
          : <tr key='footer0'>
              <td>
                <Button color='primary' onClick={() => addChallenege ()}>+</Button>
              </td>
              <td /><td /><td /><td /><td /><td /><td /><td /><td />
            </tr>}
          </tfoot>
        </Table>
      </>)
}
