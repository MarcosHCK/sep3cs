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
import { Input } from 'reactstrap'
import React, { useEffect, useState } from 'react'

export function IntegerInput (props)
{
  const { defaultValue, natural, nonZero, onChange, onChanged, value, type, ...rest } = props
  const [ inValue, setInValue ] = useState (defaultValue)
  const [ isInvalid, setIsInvalid ] = useState (false)

  const inOnChange = e =>
    {
      setInValue (Number (e.target.value))
      setIsInvalid (String (e.target.value).search (`[^${natural ? '' : '-'}0-9]`) >= 0
                      || (!!nonZero && Number (e.target.value) === 0))
    }

  useEffect (_ =>
    {
      if (!isInvalid)
        {
          onChanged (inValue)
        }
    },
  // eslint-disable-next-line react-hooks/exhaustive-deps
  [inValue, isInvalid])

return <Input invalid={isInvalid} onChange={e => inOnChange (e)} type='number' value={inValue} {...rest} />
}
