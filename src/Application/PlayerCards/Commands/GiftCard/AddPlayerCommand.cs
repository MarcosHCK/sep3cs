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
using DataClash.Domain.Enums;
using DataClash.Domain.Events;
using MediatR;

namespace DataClash.Application.PlayerCards.Commands.CreateCardGift
{
  [Authorize]
 public record CreateCardGiftCommand : IRequest
{
   public long CardId { get; init; }
   public long ClanId { get; init; }
   public long PlayerId { get; init; }
}


  public class CreateCardGiftCommandHandler : IRequestHandler<CreateCardGiftCommand>
{
   private readonly IApplicationDbContext _context;

   public CreateCardGiftCommandHandler(IApplicationDbContext context)
   {
       _context = context;
   }

   public async Task Handle(CreateCardGiftCommand request, CancellationToken cancellationToken)
   {
       var cardGift = new CardGift
       {
           CardId = request.CardId,
           ClanId = request.ClanId,
           PlayerId = request.PlayerId
       };

       _context.CardGifts.Add(cardGift);

       await _context.SaveChangesAsync(cancellationToken);

       
   }
}

}
