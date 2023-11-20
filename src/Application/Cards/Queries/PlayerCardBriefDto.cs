using DataClash.Application.Common.Mappings;
using DataClash.Domain.Entities;

namespace DataClash.Application.PlayerCards.Queries.GetPlayerCardsWithPagination
{
    public class PlayerCardBriefDto : IMapFrom<PlayerCard>
    {
        
        public long CardId { get; init; }
        public long PlayerId { get; init; }
        public long Level { get; init; }
        
    }
}