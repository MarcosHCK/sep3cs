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
using DataClash.Application.Players.Commands.UpdatePlayer;
using DataClash.Application.Players.Queries.GetCurrentPlayer;
using DataClash.Application.Players.Queries.GetPlayer;
using DataClash.Application.Players.Queries.GetPlayersWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  [Authorize]
  public class PlayerController : UserControllerBase
    {
      [HttpGet ("{id}")]
      public async Task<ActionResult<PlayerBriefDto>> Get (long id)
        {
          return await Mediator.Send (new GetPlayerQuery (id));
        }

      [HttpGet ("Current")]
      public async Task<ActionResult<PlayerBriefDto>> GetCurrent ()
        {
          return await Mediator.Send (new GetCurrentPlayerCommand ());
        }

      [HttpGet]
      [ProducesResponseType (StatusCodes.Status200OK)]
      [ProducesDefaultResponseType]
      public async Task<ActionResult<PaginatedList<PlayerBriefDto>>> GetWithPagination ([FromQuery] GetPlayersWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpPut]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Update (UpdatePlayerCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }
    }
}
