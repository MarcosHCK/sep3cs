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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataClash.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Common.Mappings
{
  public static class MappingExtensions
    {
      public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination> (this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
          where TDestination : class
        => PaginatedList<TDestination>.CreateAsync (queryable.AsNoTracking (), pageNumber, pageSize);

      public static Task<List<TDestination>> ProjectToListAsync<TDestination> (this IQueryable queryable, IConfigurationProvider configuration)
          where TDestination : class
        => queryable.ProjectTo<TDestination> (configuration).AsNoTracking ().ToListAsync ();
    }
}
