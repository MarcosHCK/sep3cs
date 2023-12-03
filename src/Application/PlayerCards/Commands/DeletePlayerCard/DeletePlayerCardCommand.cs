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
using Microsoft.EntityFrameworkCore;
namespace DataClash.Application.PlayerCards.Commands.DeletePlayerCard
{
    public record DeletePlayerCardCommand : IRequest
        {
            public long CardId { get; init; }
            public long PlayerId { get; init; }
        }
    
    public class DeletePlayerCardCommandHandler : IRequestHandler<DeletePlayerCardCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeletePlayerCardCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle (DeletePlayerCardCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.PlayerId);
            Console.WriteLine("kjejfnnesklkfs");
            var entity = await _context.PlayerCards
                .Where (l => l.CardId == request.CardId && l.PlayerId == request.PlayerId)
                .SingleOrDefaultAsync (cancellationToken)
                ?? throw new NotFoundException (nameof (PlayerCard), (request.CardId, request.PlayerId));
            _context.PlayerCards.Remove (entity);
            await _context.SaveChangesAsync (cancellationToken);
        }
    }
}
