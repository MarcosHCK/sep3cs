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
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Mappings;
using DataClash.Application.Common.Models;
using MediatR;

namespace DataClash.Application.Challenges.Queries.GetChallengesWithPagination
{
  public record GetChallengesWithPaginationQuery : IRequest<PaginatedList<ChallengeBriefDto>>
    {
      public int PageNumber { get; init; } = 1;
      public int PageSize { get; init; } = 10;
    }

  public class GetChallengesWithPaginationQueryHandler : IRequestHandler<GetChallengesWithPaginationQuery, PaginatedList<ChallengeBriefDto>>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;

      public GetChallengesWithPaginationQueryHandler (IApplicationDbContext context, IMapper mapper)
        {
          _context = context;
          _mapper = mapper;
        }

      public async Task<PaginatedList<ChallengeBriefDto>> Handle (GetChallengesWithPaginationQuery query, CancellationToken cancellationToken)
        {
          return await _context.Challenges
            .ProjectTo<ChallengeBriefDto> (_mapper.ConfigurationProvider)
            .PaginatedListAsync (query.PageNumber, query.PageSize);
        }
    }
}
