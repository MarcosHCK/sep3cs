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
import { Avatar } from './Avatar.tsx'
import { Dropdown, DropdownItem, DropdownMenu, DropdownToggle } from 'reactstrap'
import { useState } from 'react'

export function AvatarDropdown (props)
{
  const [ show, setShow ] = useState (false)
  const { userName, userEmail, children } = props

  return (
    <div onMouseEnter={ () => setShow (true) }
         onMouseLeave={ () => setShow (false) }>
      { <Dropdown isOpen={show} toggle={() => setShow (true)}>
          <DropdownToggle tag={'span'}>
            <Avatar userName={userName} userEmail={userEmail} />
          </DropdownToggle>
          <DropdownMenu>
            { children.map ((child, index) =>
              {
                if (child.type === DropdownItem)
                  return <div key={index}>{ child }</div>
                else
                  return (
                    <DropdownItem key={index}>
                      { child }
                    </DropdownItem>)
              }) }
          </DropdownMenu>
        </Dropdown>}
    </div>)
}
