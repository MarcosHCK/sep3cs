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
import './PlayerInput.css'
import { Button, Input, InputGroup } from 'reactstrap'
import { Players } from './Players'
import { PopoverBody, PopoverHeader } from 'reactstrap'
import { UncontrolledPopover } from 'reactstrap'
import { v4 as uuid } from 'uuid'
import Avatar from './AvatarEmpty.svg'
import React, { useEffect, useState } from 'react'

export function PlayerInput (props)
{
  const { defaultValue, onChanged, value, ...rest } = props
  const [ pickedPlayer, setPickerPlayer ] = useState (defaultValue)
  const [ tagId ] = useState (`unique-${uuid() }`)

  useEffect (_ =>
    {
      onChanged (pickedPlayer)
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pickedPlayer])

  return (
    <InputGroup>
      <Input disabled type='text' value={pickedPlayer} {...rest} />
      <Button color='primary' id={tagId}>
        <img alt='' src={Avatar} width='16'/>
      </Button>
      <UncontrolledPopover className='player-input-picker' placement='bottom' target={tagId} trigger='focus'>
        <PopoverHeader>
          Players
        </PopoverHeader>
        <PopoverBody>
          <Players picker onPick={id => setPickerPlayer (id)} />
        </PopoverBody>
      </UncontrolledPopover>
    </InputGroup>)
}
