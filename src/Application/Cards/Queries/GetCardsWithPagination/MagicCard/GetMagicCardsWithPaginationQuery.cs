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
using DataClash.Application.MagicCards.Queries.GetMagicCard;
using DataClash.Application.Common.Models;
using MediatR;

namespace DataClash.Application.MagicCards.Queries.GetMagicCardWithPagination
{
  public record GetMagicCardWithPaginationQuery : IRequest<PaginatedList<MagicCardBriefDto>>
  {
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? CardType { get; init; } // Añade este parámetro
  }


  public class GetMagicCardWithPaginationQueryHandler : IRequestHandler<GetMagicCardWithPaginationQuery, PaginatedList<MagicCardBriefDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMagicCardWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PaginatedList<MagicCardBriefDto>> Handle(GetMagicCardWithPaginationQuery query, CancellationToken cancellationToken)
    {
      
      var cards =_context.MagicCards;
      
      

      return await cards
        .ProjectTo<MagicCardBriefDto>(_mapper.ConfigurationProvider)
        .PaginatedListAsync(query.PageNumber, query.PageSize);
    }

  }
}
