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
import { ApplicationPaths } from '../services/AuthorizeConstants'
import { Button, Table,Input } from 'reactstrap'
import { CreateChallengeCommand } from '../webApiClient.ts'
import { DateTime } from './DateTime'
import { Navigate, useParams } from 'react-router-dom'
import { Pager } from './Pager'
import { TimeSpan } from './TimeSpan'
import { UpdateChallengeCommand } from '../webApiClient.ts'
import { useAuthorize } from '../services/AuthorizeProvider'
import { UserRoles } from '../services/AuthorizeConstants'
import { ChallengeClient } from '../webApiClient.ts'
import React, { useEffect, useState } from 'react'
import { WaitSpinner } from './WaitSpinner'
import { useErrorReporter } from './ErrorReporter'



export function Challenges ()
{
  const { initialPage } = useParams ()
  const { isAuthorized, inRole }= useAuthorize ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ hasNextPage, setHasNextPage ] = useState (false)
  const [ hasPreviousPage, setHasPreviousPage ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const [ items, setItems ] = useState (undefined)
  const [ totalPages, setTotalPages ] = useState (0)
  const [ challengeClient ] = useState (new ChallengeClient ())
  const errorReporter = useErrorReporter ()
  var error
  const pageSize = 10
  const visibleIndices = 5

  const addChallenege = async () =>
    {
      const data = new CreateChallengeCommand ()
        data.beginDay = new Date ()
        data.duration = "00:00:01"
        data.bounty=1
        data.cost=1
        data.description="<no text>"
        data.maxLooses=1
        data.minLevel=1
        data.name="<no text>"
      try{
      await challengeClient.create (data)
      setActivePage (-1)
      }catch(error)
      {
        errorReporter (error)
      }
    }

  const removeChallenge = async (item) =>
    {
      try{
      await challengeClient.delete (item.id)
      setActivePage (-1)
      }
      catch(error)
     {
      errorReporter (error)
     }
    }

  const updateChallenge = async (item) =>
    {
      const data = new UpdateChallengeCommand ()
        data.id = item.id
        data.beginDay = item.beginDay
        data.bounty=item.bounty
        data.duration = item.duration
        data.cost=item.cost
        data.description=item.description
        data.maxLooses=item.maxLooses
        data.minLevel=item.minLevel
        data.name=item.name
      try{  
        await challengeClient.update (item.id, data)
      }catch(error)
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
          try{
          const paginatedList = await challengeClient.getWithPagination (activePage + 1, pageSize)

          setHasNextPage (paginatedList.hasNextPage)
          setHasPreviousPage (paginatedList.hasPreviousPage)
          setItems (paginatedList.items)
          setTotalPages (paginatedList.totalPages)
          }
          catch{
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
    isLoading||!isAuthorized
    ? (<WaitSpinner />)
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
          <Table striped responsive bordered  className='my-custom-class'>
            <thead>
              <tr>
                <th>{'#'}</th>
                <th>{'Begin day'}</th>
                <th>{'Duration'}</th>
                <th>{'Bounty'}</th>
                <th>{'Cost'}</th>
                <th>{'Description'}</th>
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
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <TimeSpan
                    defaultValue={item.duration}
                    onChanged={(span) => { item.duration = span; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <Input
                    type='text'
                    defaultValue={item.bounty}
                    onChanged={(e) => { e.preventDefault();item.bounty = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <Input
                    type='number'
                    defaultValue={item.cost}
                    onChanged={(e) => {e.preventDefault(); item.cost = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <Input
                    type='text'
                    defaultValue={item.description}
                    onChange={(e) => { e.preventDefault (); item.description = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <Input
                    type='number'
                    defaultValue={item.maxLooses}
                    onChanged={(e) => {e.preventDefault(); item.maxLooses = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                    
                </td>
                <td>
                  <Input
                    type='number'
                    defaultValue={item.minLevel}
                    onChanged={(e) => {e.preventDefault(); item.minLevel = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>
                <td>
                  <Input
                    type='text'
                    defaultValue={item.name}
                    onChanged={(e) => {e.preventDefault(); item.name = e.target.value; updateChallenge (item) }}
                    readOnly={!inRole[UserRoles.Administrator]} />
                </td>

        {
          (!inRole[UserRoles.Administrator])
          ? (<td />)
          : (
                <td>
                  <Button color='primary' onClick={() => removeChallenge (item)} close />
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
                  <Button color='primary' onClick={() => addChallenege ()}>+</Button>
                </td>
                <td /><td /><td />
              </tr>)
        }
            </tfoot>
          </Table>
        </div>
      </>))
}
