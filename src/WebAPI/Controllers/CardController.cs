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
using DataClash.Application.MagicCards.Queries.GetMagicCard;
using DataClash.Application.MagicCards.Queries.GetMagicCardWithPagination;
using DataClash.Application.StructCards.Queries.GetStructCard;
using DataClash.Application.StructCards.Queries.GetStructCardWithPagination;
using DataClash.Application.TroopCards.Queries.GetTroopCards;
using DataClash.Application.TroopCards.Queries.GetTroopCardWithPagination;
using DataClash.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  [Authorize]
  public class CardController : ApiControllerBase
    {
      [HttpGet ("Magic")]
      public async Task<ActionResult<PaginatedList<MagicCardBriefDto>>> GetMagicCardsWithPagination ([FromQuery] GetMagicCardWithPaginationQuery query)
        => await Mediator.Send (query);
      [HttpGet ("Troop")]
      public async Task<ActionResult<PaginatedList<TroopCardBriefDto>>> GetTroopCardsWithPagination ([FromQuery] GetTroopCardWithPaginationQuery query)
        => await Mediator.Send (query);
      [HttpGet ("Struct")]
      public async Task<ActionResult<PaginatedList<StructCardBriefDto>>> GetStructCardsWithPagination ([FromQuery] GetStructCardWithPaginationQuery query)
        => await Mediator.Send (query);
    }
}
