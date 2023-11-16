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
import { createUseStyles } from 'react-jss'
import AvatarPlaceholder from './AvatarEmpty.svg'
import md5 from 'md5'
import React from 'react'

const getColorAndBackground = (digest) =>
{
  const matches = digest.match (/.{2}/g)
  const [red, green, blue] = matches.map (hex => parseInt (hex, 16))

  // Formula from https://www.w3.org/TR/AERT/#color-contrast
  const luminance = (red * 0.299 + green * 0.587 + blue * 0.114) / 255
  const color = luminance > 0.6 ? '#222' : '#fff'
  return { background : `rgb(${[red, green, blue]})`, color, }
}

const getInitials = (name, maxLength = 3) =>
{
  const chars = [...name.trim ()]
  const initials = []

  if (name.length <= maxLength)
    return name
  for (const [idx, char] of chars.entries ())
    {
      if (
        char.toLowerCase() !== char ||
        !chars[idx - 1] ||
        /\s/.test(chars[idx - 1]))
        {
          initials.push (char)

          if (initials.length === maxLength)
            break
        }
    }
return initials.join ('')
}

export function Avatar (props)
{
  // 250px is large enough that it will suffice for most purposes,
  // but small enough that it won't require too much bandwidth.
  // We limit the minimum size to improve caching.

  const { userName, userEmail, size = 50, ...rest } = props
  const digest = md5 (userEmail === undefined ? userName : userEmail)
  const initials = getInitials (userName)

  const c = createUseStyles (
    {
      img: () => (
        {
          borderRadius: '50%',
          height: size,
          left: 0,
          position: 'absolute',
          top: 0,
          width: size,
        }),
      parent: () => (
        {
          ...getColorAndBackground (digest),
          alignItems: 'center',
          borderRadius: '50%',
          boxShadow: '5px 5px 10px rgba(0, 0, 0, 0.15)',
          display: 'inline-flex',
          height: size,
          justifyContent: 'center',
          position: 'relative',
          width: size,
        }),
      swatch: () => (
        {
          // scale the text size depending on avatar size and
          // number of initials
          fontFamily: 'sans-serif',
          fontSize: size / (1.4 * Math.max ([...initials].length, 2)),
          position: 'absolute',
          userSelect: 'none',
        }),
    }) ()

  if (userEmail === undefined)
    {
      return (
        <div className={c.parent} {...rest}>
          <div aria-hidden='true' className={c.swatch}>
            {initials}
          </div>
        </div>)
    }
  else
    {
      const url = `https://www.gravatar.com/avatar/${digest}?s=${String (Math.max (size, 250),)}&d=blank`

      return (
        <div className={c.parent} {...rest}>
          <div aria-hidden='true' className={c.swatch}>
            {initials}
          </div>

          <img className={c.img} src={String(url)} alt={`${userName}’s avatar`} />
        </div>)
    }
}

export function AvatarEmpty (props)
{
  const { size = 50, } = props

  const c = createUseStyles (
    {
      box : () => (
        {
          border: '3px solid black',
          borderRadius: `${size}px ${size}px ${size}px ${size}px`,
        }),
      img : () => (
        {
          height : size,
          width : size,
        }),
      label : () => (
        {
          paddingLeft : 7,
          paddingRight : 11,
          margin : 0,
        }),
    }) ()

  return (
    <div className={`d-flex flex-row align-items-center justify-content-center border ${c.box}`}>
      <img className={`rounded-circle ${c.img}`} src={AvatarPlaceholder} alt={'empty avatar'} />
      <p className={c.label}>{'Sign in'}</p>
    </div>
  )
}
