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
using DataClash.Domain.Entities;

namespace DataClash.Application.Statistics.MostPopularCards
{
    public class MostPopularCards
    {
        public IEnumerable<Tuple<Card, string, Clan>> GetMostPopularCards(IApplicationDbContext context, string clanName)
        {
            // Obtén el clan dado
            var clan = context.Clans.FirstOrDefault(c => c.Name == clanName);
            if (clan == null)
            {
                return Enumerable.Empty<Tuple<Card, string, Clan>>();
            }

            // Obtén todos los jugadores en el clan
            var players = context.PlayerClans
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
            // Obtén la tarjeta más popular para cada tipo de tarjeta
            var mostPopularCards = new List<Tuple<Card, string, Clan>>();

            // Para tarjetas mágicas
            var magicCards = favoriteCards
                .Where(f => context.Cards.OfType<MagicCard>().Any(c => c.Id == f.CardId))
                .OrderByDescending(f => f.Count)
                .FirstOrDefault();

            if (magicCards != null)
            {
                var card = context.Cards.OfType<MagicCard>().First(c => c.Id == magicCards.CardId);
                var cardTuple = Tuple.Create((Card)card, "MagicCard", clan);
                mostPopularCards.Add(cardTuple);
            }

            // Para tarjetas estructurales
            var structCards = favoriteCards
                .Where(f => context.Cards.OfType<StructCard>().Any(c => c.Id == f.CardId))
                .OrderByDescending(f => f.Count)
                .FirstOrDefault();

            if (structCards != null)
            {
                var card = context.Cards.OfType<StructCard>().First(c => c.Id == structCards.CardId);
                var cardTuple = Tuple.Create((Card)card, "StructCard", clan);
                mostPopularCards.Add(cardTuple);
            }

            // Para tarjetas de tropas
            var troopCards = favoriteCards
                .Where(f => context.Cards.OfType<TroopCard>().Any(c => c.Id == f.CardId))
                .OrderByDescending(f => f.Count)
                .FirstOrDefault();

            if (troopCards != null)
            {
                var card = context.Cards.OfType<TroopCard>().First(c => c.Id == troopCards.CardId);
                var cardTuple = Tuple.Create((Card)card, "TroopCard", clan);
                mostPopularCards.Add(cardTuple);
            }

            return mostPopularCards;
        }

        public IEnumerable<string> GetAllClansNames(IApplicationDbContext context)
        {
            var allClanNames = context.Clans.Select(c => c.Name).ToList();
            return allClanNames;
        }

    }
}