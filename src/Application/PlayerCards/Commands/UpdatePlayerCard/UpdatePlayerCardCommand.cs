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