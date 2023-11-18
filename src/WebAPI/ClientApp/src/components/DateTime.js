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
import { Input, InputGroup } from 'reactstrap'
import React, { useEffect, useRef, useState } from 'react'

const getStdDate = (date) => date.toISOString ().substring (0, 10)
const getStdTime = (date) => date.toISOString ().substring (11, 16)

export function DateTime (props)
{
  const { defaultValue, onChanged } = props
  const [date, setDate] = useState (defaultValue)
  const firstRender = useRef (true)

  const updateDate = (value) =>
    {
      const dateString = value
      const timeString = getStdTime (date)
      setDate (new Date (`${dateString}T${timeString}:00.000Z`))
    }

  const updateTime = (value) =>
    {
      const dateString = getStdDate (date)
      const timeString = value
      setDate (new Date (`${dateString}T${timeString}:00.000Z`))
    }

  useEffect (() =>
    {
      if (firstRender.current)
        firstRender.current = false
      else
        {
          onChanged (date)
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [date])

  return (
    <InputGroup className='d-flex'>
      <Input type='time' className='flex-grow-1 flex-shrink-1 p-2'
        defaultValue={getStdTime (date)}
        onChange={(e) => updateTime (e.target.value) } />
      <Input type='date' className='flex-grow-1 flex-shrink-1 p-2'
        defaultValue={getStdDate (date)}
        onChange={(e) => updateDate (e.target.value) } />
    </InputGroup>)
}
