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
using DataClash.Application.Common.Interfaces;
using MediatR;
using FluentValidation;
using DataClash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Statistics.PopularCards
{
  public record GetPopularCardsQuery(string clanName) : IRequest<List<string[]>>;

  public class GetPopularCardsQueryHandler : IRequestHandler<GetPopularCardsQuery, List<string[]>>
    {
      private readonly IApplicationDbContext _context;

      public GetPopularCardsQueryHandler (IApplicationDbContext context)
        {
          _context = context;
        }

      public async Task<List<string[]>> Handle (GetPopularCardsQuery request, CancellationToken cancellationToken)
        {
          var clan = await _context.Clans.FirstOrDefaultAsync (c => c.Name == request.clanName, cancellationToken);

          if (clan == null)
            {
              return Enumerable.Empty<string []> ().ToList ();
            }

          var players = await _context.PlayerClans
              .Where (pc => pc.ClanId == clan.Id)
              .Select (pc => pc.Player)
              .ToListAsync (cancellationToken);

          var favoriteCards = players
              .Where (p => p.FavoriteCardId.HasValue)
              .GroupBy (p => p.FavoriteCardId!.Value)
              .Select (g => new { CardId = g.Key, Count = g.Count () })
              .ToList ();

          favoriteCards.Sort ((a, b) => b.Count.CompareTo (a.Count));

          var mostPopularCards = new List<string[]> ();
          var magicCards = favoriteCards
              .Where (f => _context.Cards.OfType<MagicCard> ().Any (c => c.Id == f.CardId))
              .OrderByDescending (f => f.Count)
              .FirstOrDefault ();

          if (magicCards != null)
            {
              var card = _context.Cards.OfType<MagicCard> ().First (c => c.Id == magicCards.CardId);
              var cardArray = new string[] { card.Name, "MagicCard", clan.Name };
              mostPopularCards.Add (cardArray);
            }

          var structCards = favoriteCards
              .Where (f => _context.Cards.OfType<StructCard> ().Any (c => c.Id == f.CardId))
              .OrderByDescending (f => f.Count)
              .FirstOrDefault ();

          if (structCards != null)
            {
                var card = _context.Cards.OfType<StructCard> ().First (c => c.Id == structCards.CardId);
                var cardArray = new string[] { card.Name, "StructCard", clan.Name };
                mostPopularCards.Add (cardArray);
            }

          var troopCards = favoriteCards
              .Where(f => _context.Cards.OfType<TroopCard> ().Any (c => c.Id == f.CardId))
              .OrderByDescending (f => f.Count)
              .FirstOrDefault ();

          if (troopCards != null)
            {
              var card = _context.Cards.OfType<TroopCard> ().First (c => c.Id == troopCards.CardId);
              var cardArray = new string[] { card.Name, "TroopCard", clan.Name };
              mostPopularCards.Add (cardArray);
            }

          return mostPopularCards;
        }
    }
}



