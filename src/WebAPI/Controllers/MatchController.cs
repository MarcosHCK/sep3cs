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
using DataClash.Application.Common.Models;
using DataClash.Application.Matches.Commands.CreateMatch;
using DataClash.Application.Matches.Commands.DeleteMatch;
using DataClash.Application.Matches.Commands.UpdateMatch;
using DataClash.Application.Matches.Queries.GetMatch;
using DataClash.Application.Matches.Queries.GetMatchesWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  public class MatchController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<PaginatedList<MatchBriefDto>>> GetWithPagination ([FromQuery] GetMatchesWithPaginationQuery query)
        => await Mediator.Send (query);
      [HttpPost]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Create (CreateMatchCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpDelete]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Delete (DeleteMatchCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpPut]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Update (UpdateMatchCommand command)
        => await NoContent (() => Mediator.Send (command));
    }
}
