using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Mappings;
using DataClash.Application.Common.Models;

using MediatR;

namespace DataClash.Application.PlayerCards.Queries.GetPlayerCardsWithPagination
{
  public record GetPlayerCardsWithPaginationQuery : IRequest<PaginatedList<PlayerCardBriefDto>>
    {
      public int PageNumber { get; init; } = 1;
      public int PageSize { get; init; } = 10;
    }

  public class GetPlayerCardsWithPaginationQueryHandler : IRequestHandler<GetPlayerCardsWithPaginationQuery, PaginatedList<PlayerCardBriefDto>>
    {
      private readonly IApplicationDbContext _context;
      private readonly IMapper _mapper;

      public GetPlayerCardsWithPaginationQueryHandler (IApplicationDbContext context, IMapper mapper)
        {
          _context = context;
          _mapper = mapper;
        }

      public async Task<PaginatedList<PlayerCardBriefDto>> Handle (GetPlayerCardsWithPaginationQuery query, CancellationToken cancellationToken)
        {
          return await _context.PlayerCards
            .ProjectTo<PlayerCardBriefDto> (_mapper.ConfigurationProvider)
            .PaginatedListAsync (query.PageNumber, query.PageSize);
        }
    }
}