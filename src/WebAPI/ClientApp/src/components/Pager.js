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
import { Pagination } from 'reactstrap'
import { PaginationItem } from 'reactstrap'
import { PaginationLink } from 'reactstrap'
import React from 'react'

export function Pager (props)
{
  const
    {
      activePage,
      hasNextPage,
      hasPreviousPage,
      onPageChanged,
      totalPages,
      visibleIndices,
    } = props

  if (visibleIndices !== undefined && (visibleIndices % 2 === 0 && visibleIndices > 2))
    throw new Error ('Bad visibleIndices value (should be odd >=3)')

  const calcSeq = () =>
    {
      if (visibleIndices === undefined)
        return [ 0, totalPages ]
      else if (visibleIndices + 1 > totalPages)
        return [ 0, totalPages ]
      else
        {
          const half = ((visibleIndices - 1) / 2)|0
          const left = totalPages - activePage - 1
          const limLeft = half > activePage ? activePage : half
          const limRight = half > left ? left : half

          return (
            [
              (activePage - limLeft) - (half - limRight),
              (activePage + limRight + 1) + (half - limLeft)
            ])
        }
    }

  const expandPages = () =>
    {
      const pages = []
      const [ begin, end ] = calcSeq ()

      for (let i = begin; i < end; ++i)
        {
          pages.push (
            <PaginationItem active={i === activePage} key={`i${i}`}>
              <PaginationLink onClick={() => onPageChanged (i)}>
                { i }
              </PaginationLink>
            </PaginationItem>)
        }
    return pages
    }

  return (
    <Pagination>

      <PaginationItem key='first'>
        <PaginationLink first onClick={() => onPageChanged (0)} />
      </PaginationItem>

      <PaginationItem key='previous' disabled={!hasPreviousPage || activePage === 0}>
        <PaginationLink previous onClick={() => onPageChanged (activePage - 1)} />
      </PaginationItem>

    { expandPages () }

      <PaginationItem key='next' disabled={!hasNextPage || (activePage + 1) === totalPages}>
        <PaginationLink next onClick={() => onPageChanged (activePage + 1)} />
      </PaginationItem>

      <PaginationItem key='last'>
        <PaginationLink last onClick={() => onPageChanged (totalPages - 1)} />
      </PaginationItem>

    </Pagination>
  )
}
