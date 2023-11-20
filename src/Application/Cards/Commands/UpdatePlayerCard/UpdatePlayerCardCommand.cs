using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using DataClash.Application.Common.Security;
using DataClash.Domain.Entities;
using MediatR;
namespace DataClash.Application.PlayerCards.Commands.UpdatePlayerCard
{
    public record UpdatePlayerCardCommand : IRequest
    {
        
        public long CardId { get; init; }
        public long PlayerId { get; init; }
        public int Level { get; init; }
    }
    public class UpdatePlayerCardCommandHandler : IRequestHandler<UpdatePlayerCardCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdatePlayerCardCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdatePlayerCardCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PlayerCards.FindAsync(new object[] { request.CardId,request.PlayerId }, cancellationToken) ?? throw new NotFoundException(nameof(PlayerCard), (request.CardId,request.PlayerId));
            entity.CardId = request.CardId;
            entity.PlayerId = request.PlayerId;
            entity.Level = request.Level;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}