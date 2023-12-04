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
using DataClash.Application.Challenges.Commands.AddPlayer;
using DataClash.Application.Challenges.Commands.CreateChallenge;
using DataClash.Application.Challenges.Commands.DeleteChallenge;
using DataClash.Application.Challenges.Commands.RemovePlayer;
using DataClash.Application.Challenges.Commands.UpdateChallenge;
using DataClash.Application.Challenges.Commands.UpdatePlayer;
using DataClash.Application.Challenges.Queries.GetChallengesForPlayerWithPagination;
using DataClash.Application.Challenges.Queries.GetChallengesWithPagination;
using DataClash.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  public class ChallengeController : ApiControllerBase
    {
      [HttpGet ("Player")]
      public async Task<ActionResult<PaginatedList<PlayerChallengeBriefDto>>> GetForPlayer ([FromQuery] GetChallengesForPlayerWithPaginationQuery query)
        => await Mediator.Send (query);
      [HttpGet]
      public async Task<ActionResult<PaginatedList<ChallengeBriefDto>>> GetWithPagination ([FromQuery] GetChallengesWithPaginationQuery query)
        => await Mediator.Send (query);
      [HttpPost ("Player")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> AddPlayer (AddPlayerCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpPost]
      public async Task<ActionResult<long>> Create (CreateChallengeCommand command)
        => await Mediator.Send (command);
      [HttpDelete ("Player")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> RemovePlayer (RemovePlayerCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpDelete]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
      public async Task<IActionResult> Delete (DeleteChallengeCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpPut ("Player")]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
       public async Task<IActionResult> UpdatePlayer (UpdatePlayerCommand command)
        => await NoContent (() => Mediator.Send (command));
      [HttpPut]
      [ProducesResponseType (StatusCodes.Status204NoContent)]
      [ProducesDefaultResponseType]
       public async Task<IActionResult> Update (UpdateChallengeCommand command)
        => await NoContent (() => Mediator.Send (command));
    }
}
