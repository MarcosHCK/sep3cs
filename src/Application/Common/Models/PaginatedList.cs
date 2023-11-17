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
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Common.Models
{
  public class PaginatedList<T>
    {
      public IReadOnlyCollection<T> Items { get; }
      public int PageNumber { get; }
      public int TotalPages { get; }
      public int TotalCount { get; }

      public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
          PageNumber = pageNumber;
          TotalPages = (int) Math.Ceiling (count / (double) pageSize);
          TotalCount = count;
          Items = items;
        }

      public bool HasPreviousPage => PageNumber > 1;
      public bool HasNextPage => PageNumber < TotalPages;

      public static async Task<PaginatedList<T>> CreateAsync (IQueryable<T> source, int pageNumber, int pageSize)
        {
          var count = await source.CountAsync ();
          var items = await source.Skip ((pageNumber - 1) * pageSize).Take (pageSize).ToListAsync ();
        return new PaginatedList<T> (items, count, pageNumber, pageSize);
        }
    }
}
