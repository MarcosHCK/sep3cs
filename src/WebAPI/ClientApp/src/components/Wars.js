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
import { Alert } from 'reactstrap'
import { useAuthToken } from '../services/AuthorizeReact.js'
import { useParams } from 'react-router-dom'
import { WarClient } from '../webApiClient.ts'
import React, { useEffect, useState } from 'react'

export function Wars ()
{
  const { initialPage } = useParams ()
  const [ activePage, setActivePage ] = useState (initialPage ? initialPage : 0)
  const [ authToken ] = useAuthToken ()
  const [ isLoading, setIsLoading ] = useState (false)
  const [ isReady, setIsReady ] = useState (false)
  const [ warClient ] = useState (new WarClient ())
  const pageSize = 10

  useEffect (() =>
    {
      if (authToken !== undefined)
        warClient.setBearerToken (authToken)
      else
        warClient.clearBearerToken ()

      setIsReady (authToken !== null)
    }, [authToken, warClient])

  useEffect (() =>
    {
      if (isReady)
        {
          const paginatedList = warClient.getWithPagination (1, pageSize)
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isReady, activePage])

  return (
    !isReady
      ? (<Alert color='danger'>Restricted</Alert>)
    : (isLoading
      ? (<div></div>)
    : <p>ds</p>))
}
