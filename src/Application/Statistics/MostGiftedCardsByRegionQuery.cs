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
using Microsoft.EntityFrameworkCore;

namespace DataClash.Application.Statistics.MostGiftedCardsByRegion
{
    public record GetMostGiftedCardsQuery : IRequest<List<string[]>>;

    public class GetMostGiftedCardsQueryHandler : IRequestHandler<GetMostGiftedCardsQuery, List<string[]>>
    {
        private readonly IApplicationDbContext _context;

        public GetMostGiftedCardsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string[]>> Handle(GetMostGiftedCardsQuery request, CancellationToken cancellationToken)
        {
            var joinedEntities = _context.CardGifts
                .Include(cg => cg.Clan)
                .ToList();

            var groupedEntities = joinedEntities
                .GroupBy(e => new { e.CardId, e.Clan.Region }) // Agrupa por CardId y Region
                .Select(g => new string[]
                {
           _context.Cards.Find(g.Key.CardId).Name,
           g.Key.Region.ToString(),
           g.Count().ToString(),
                })
                .ToList();

            // Ordena los grupos por el recuento de tarjetas en orden descendente y toma los primeros 3 de cada grupo
            var limitedEntities = groupedEntities
                .GroupBy(g => g[1]) // Agrupa por Region
                .SelectMany(g => g.OrderByDescending(x => int.Parse(x[2])).Take(3)) // Ordena por el recuento de tarjetas y toma los primeros 3
                .ToList();

            return limitedEntities;
        }


    }

}





