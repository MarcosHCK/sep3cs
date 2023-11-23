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
using DataClash.Application.Players.Queries.GetPlayer;
using DataClash.Application.Players.Queries.GetPlayersWithPagination;
using DataClash.Framework.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  [Authorize]
  public class PlayerController : UserControllerBase
    {
      private ApplicationDbContext? _context;
      public ApplicationDbContext DbContext => _context ??= HttpContext.RequestServices.GetRequiredService<ApplicationDbContext> ();

      [HttpGet]
      public async Task<ActionResult<PlayerBriefDto>> Get ()
        {
          var userKey = new object[] { CurrentUser.UserId! };
          var user = await DbContext.Users.FindAsync (userKey);

          if (!user!.PlayerId.HasValue)
            return BadRequest ();
          else
            {
              var playerId = user!.PlayerId.Value;
              var query = new GetPlayerQuery (playerId);
              return await Mediator.Send (query);
            }
        }

      [HttpGet]
      [Route ("List")]
      public async Task<ActionResult<PaginatedList<PlayerBriefDto>>> GetWithPagination ([FromQuery] GetPlayersWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpPut]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesResponseType (StatusCodes.Status400BadRequest)]
      [ProducesResponseType (StatusCodes.Status401Unauthorized)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Update (UpdatePlayerCommand command)
        {
          var userKey = new object[] { CurrentUser.UserId! };
          var user = await DbContext.Users.FindAsync (userKey);

          if (!user!.PlayerId.HasValue)
            return BadRequest ();
          else
            {
              var playerId = user!.PlayerId.Value;

              if (command.Id != playerId &&
                !await Identity.IsInRoleAsync (CurrentUser.UserId!, "Administrator"))
                return Unauthorized ();

              await Mediator.Send (command);
            }
        return NoContent ();
        }
    }
}
