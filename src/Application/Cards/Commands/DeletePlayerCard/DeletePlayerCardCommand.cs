using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace DataClash.Application.PlayerCards.Commands.DeletePlayerCard
{
    public record DeletePlayerCardCommand ((long CardId, long PlayerId) Key) : IRequest;
    
    public class DeletePlayerCardCommandHandler : IRequestHandler<DeletePlayerCardCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeletePlayerCardCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle (DeletePlayerCardCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PlayerCards
                .Where (l => l.CardId == request.Key.CardId && l.PlayerId == request.Key.PlayerId)
                .SingleOrDefaultAsync (cancellationToken)
                ?? throw new NotFoundException (nameof (PlayerCard), (request.Key.CardId, request.Key.PlayerId));
            _context.PlayerCards.Remove (entity);
            await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
