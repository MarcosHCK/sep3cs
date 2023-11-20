using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using MediatR;

namespace DataClash.Application.PlayerCards.Commands.CreatePlayerCard
{
    
    public record CreatePlayerCardCommand : IRequest<(long, long)>
    {
        public long CardId { get; init; }
        public long PlayerId { get; init; }
        public int Level { get; init; }
    }
    public class CreatePlayerCardCommandHandler : IRequestHandler<CreatePlayerCardCommand, (long, long)>
    {
        private readonly IApplicationDbContext _context;
        public CreatePlayerCardCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<(long, long)> Handle(CreatePlayerCardCommand request, CancellationToken cancellationToken)
        {
            var entity =new PlayerCard
            {
                CardId = request.CardId,
                PlayerId = request.PlayerId,
                Level = request.Level,
                    

            };
            _context.PlayerCards.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return (entity.CardId, entity.PlayerId);
            
        }
    }
}
