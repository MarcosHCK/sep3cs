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
using DataClash.Domain.ValueObjects;

namespace DataClash.Application.Statistics.MostGiftedCardsByRegion
{
    public class MostGiftedCards
    {
        public async Task<IEnumerable<Tuple<Card, Region, long>>> GetMostDonatedCardsByRegion(IApplicationDbContext context)
        {
            var lastMonth = DateTime.Now.AddMonths(-1);

            var cardGifts =  context.CardGifts
                .Where(g => g.Date >= lastMonth)
                .ToList();

            var mostDonatedCards = cardGifts
                .GroupBy(g => new { g.Clan.Region.Code, g.CardId })
                .Select(g => new
                {
                    CardId = g.First().CardId,
                    Region = g.First().Clan.Region,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToList();

            var result = new List<Tuple<Card, Region, long>>();
            foreach (var cardGift in mostDonatedCards)
            {
                var card = await context.Cards.FindAsync(cardGift.CardId);
                result.Add(Tuple.Create(card, cardGift.Region, (long)cardGift.Count));
            }

            return result;
        }



    }
}