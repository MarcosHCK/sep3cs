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
using DataClash.Application.Clans.Commands.AddPlayer;
using DataClash.Application.Clans.Commands.CreateClan;
using DataClash.Application.Clans.Commands.CreateClanWithChief;
using DataClash.Application.Clans.Commands.DeleteClan;
using DataClash.Application.Clans.Commands.EnterWar;
using DataClash.Application.Clans.Commands.LeaveWar;
using DataClash.Application.Clans.Commands.RemovePlayer;
using DataClash.Application.Clans.Commands.UpdateClan;
using DataClash.Application.Clans.Commands.UpdatePlayer;
using DataClash.Application.Clans.Commands.UpdateWar;
using DataClash.Application.Clans.Queries.GetClanForCurrentPlayer;
using DataClash.Application.Clans.Queries.GetClansWithPagination;
using DataClash.Application.Clans.Queries.GetPlayerClansWithPagination;
using DataClash.Application.Clans.Queries.GetWarClansWithPagination;
using DataClash.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  [Authorize]
  public class ClanController : ApiControllerBase
    {
      [HttpGet ("Current")]
      public async Task<ActionResult<CurrentPlayerClanVm?>> GetForCurrentPlayer ([FromQuery] GetClanForCurrentPlayerQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpGet ("Player")]
      public async Task<ActionResult<PaginatedList<PlayerClanBriefDto>>> GetPlayerClansWithPagination ([FromQuery] GetPlayerClansWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpGet ("War")]
      public async Task<ActionResult<PaginatedList<WarClanBriefDto>>> GetWarClansWithPagination ([FromQuery] GetWarClansWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpGet]
      public async Task<ActionResult<PaginatedList<ClanBriefDto>>> GetWithPagination ([FromQuery] GetClansWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpPost]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      [Route ("Player")]
      public async Task<IActionResult> AddPlayer (AddPlayerCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpPost ("War")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> EnterWar (EnterWarCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpPost ("WithChief")]
      public async Task<ActionResult<long>> CreateWithChief (CreateClanWithChiefCommand command)
        {
          return await Mediator.Send (command);
        }

      [HttpPost]
      public async Task<ActionResult<long>> Create (CreateClanCommand command)
        {
          return await Mediator.Send (command);
        }

      [HttpDelete ("Player")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> RemovePlayer (RemovePlayerCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpDelete ("War")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> LeaveWar (LeaveWarCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpDelete]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Delete (DeleteClanCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpPut ("Player")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> UpdatePlayer(UpdatePlayerCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpPut ("War")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> UpdateWar (UpdateWarCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }

      [HttpPut]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Update (UpdateClanCommand command)
        {
          await Mediator.Send (command);
          return NoContent ();
        }
    }
}
