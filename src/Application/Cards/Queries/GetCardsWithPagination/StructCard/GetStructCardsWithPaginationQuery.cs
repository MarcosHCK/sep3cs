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
using DataClash.Application.StructCards.Queries.GetStructCard;
using DataClash.Application.Common.Models;
using MediatR;

namespace DataClash.Application.StructCards.Queries.GetStructCardWithPagination
{
  public record GetStructCardWithPaginationQuery : IRequest<PaginatedList<StructCardBriefDto>>
  {
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? CardType { get; init; } // Añade este parámetro
  }


  public class GetStructCardWithPaginationQueryHandler : IRequestHandler<GetStructCardWithPaginationQuery, PaginatedList<StructCardBriefDto>>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStructCardWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PaginatedList<StructCardBriefDto>> Handle(GetStructCardWithPaginationQuery query, CancellationToken cancellationToken)
    {
      
      var cards = _context.StructCards;
      
      

      return await cards
        .ProjectTo<StructCardBriefDto>(_mapper.ConfigurationProvider)
        .PaginatedListAsync(query.PageNumber, query.PageSize);
    }

  }
}
