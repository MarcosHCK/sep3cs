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

namespace DataClash.Application.Statistics.PopularCards
{
   public record GetPopularCardsQuery(string clanName) : IRequest<List<string[]>>;

   public class GetPopularCardsQueryHandler : IRequestHandler<GetPopularCardsQuery, List<string[]>>
   {
       private readonly IApplicationDbContext _context;

       public GetPopularCardsQueryHandler(IApplicationDbContext context)
       {
           _context = context;
       }

        public async Task<List<string[]>> Handle(GetPopularCardsQuery request, CancellationToken cancellationToken)
        {
            // Obtén el clan dado
            var clan = _context.Clans.FirstOrDefault(c => c.Name == request.clanName);
            if (clan == null)
            {
                return Enumerable.Empty<string[]>().ToList();
            }

            // Obtén todos los jugadores en el clan
            var players = _context.PlayerClans
               .Where(pc => pc.ClanId == clan.Id)
               .Select(pc => pc.Player)
               .ToList();

            // Agrupa los jugadores por su tarjeta favorita y cuenta cuántos jugadores tienen cada tarjeta como favorita
            var favoriteCards = players
               .Where(p => p.FavoriteCardId.HasValue)
               .GroupBy(p => p.FavoriteCardId.Value)
               .Select(g => new { CardId = g.Key, Count = g.Count() })
               .ToList();

            // Ordena las tarjetas por el número de jugadores que las tienen como favorita
            favoriteCards.Sort((a, b) => b.Count.CompareTo(a.Count));

            // Obtén la tarjeta más popular para cada tipo de tarjeta
            var mostPopularCards = new List<string[]>();

            // Para tarjetas mágicas
            var magicCards = favoriteCards
               .Where(f => _context.Cards.OfType<MagicCard>().Any(c => c.Id == f.CardId))
               .OrderByDescending(f => f.Count)
               .FirstOrDefault();

            if (magicCards != null)
            {
                var card = _context.Cards.OfType<MagicCard>().First(c => c.Id == magicCards.CardId);
                var cardArray = new string[] { card.Name, "MagicCard", clan.Name };
                mostPopularCards.Add(cardArray);
            }

            // Para tarjetas estructurales
            var structCards = favoriteCards
               .Where(f => _context.Cards.OfType<StructCard>().Any(c => c.Id == f.CardId))
               .OrderByDescending(f => f.Count)
               .FirstOrDefault();

            if (structCards != null)
            {
                var card = _context.Cards.OfType<StructCard>().First(c => c.Id == structCards.CardId);
                var cardArray = new string[] { card.Name, "StructCard", clan.Name };
                mostPopularCards.Add(cardArray);
            }

            // Para tarjetas de tropas
            var troopCards = favoriteCards
               .Where(f => _context.Cards.OfType<TroopCard>().Any(c => c.Id == f.CardId))
               .OrderByDescending(f => f.Count)
               .FirstOrDefault();

            if (troopCards != null)
            {
                var card = _context.Cards.OfType<TroopCard>().First(c => c.Id == troopCards.CardId);
                var cardArray = new string[] { card.Name, "TroopCard", clan.Name };
                mostPopularCards.Add(cardArray);
            }

            return mostPopularCards;

        }

    }

    public class GetPopularCardsQueryValidator : AbstractValidator<GetPopularCardsQuery>
    {
        public GetPopularCardsQueryValidator()
        {
            // Add validation rules here
        }
    }
}



