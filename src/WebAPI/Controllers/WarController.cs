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
using DataClash.Application.Wars.Commands.CreateWar;
using DataClash.Application.Wars.Commands.DeleteWar;
using DataClash.Application.Wars.Commands.UpdateWar;
using DataClash.Application.Wars.Queries.GetWar;
using DataClash.Application.Wars.Queries.GetWarsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  [Authorize]
  public class WarController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<PaginatedList<WarBriefDto>>> GetWithPagination ([FromQuery] GetWarsWithPaginationQuery query)
        {
          return await Mediator.Send (query);
        }

      [HttpPost]
      public async Task<ActionResult<long>> Create (CreateWarCommand command)
        {
          return await Mediator.Send (command);
        }

      [HttpDelete ("{id}")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Delete (long id)
        {
          await Mediator.Send (new DeleteWarCommand (id));
          return NoContent ();
        }

      [HttpPut ("{id}")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesResponseType (StatusCodes.Status400BadRequest)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Update (long id, UpdateWarCommand command)
        {
          if (id != command.Id)
            return BadRequest ();

          await Mediator.Send (command);
          return NoContent ();
        }
    }
}
