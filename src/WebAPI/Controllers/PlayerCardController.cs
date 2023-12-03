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
using DataClash.Application.PlayerCards.Commands.CreatePlayerCard;
using DataClash.Application.PlayerCards.Commands.DeletePlayerCard;
using DataClash.Application.PlayerCards.Commands.UpdatePlayerCard;
using DataClash.Application.PlayerCards.Queries.GetPlayerCardsWithPagination;
using DataClash.Application.PlayerCards.Commands.CreateCardGift;
using DataClash.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DataClash.WebUI.Controllers{
    public class PlayerCardController : ApiControllerBase{
        [HttpGet]
        public async Task<ActionResult<PaginatedList<PlayerCardBriefDto>>> GetWithPagination ([FromQuery] GetPlayerCardsWithPaginationQuery query){
            return await Mediator.Send (query);
        }
        [HttpPost]
        public async Task<ActionResult<(long,long)>> Create (CreatePlayerCardCommand command){
            return await Mediator.Send (command);
        }
        [HttpDelete ]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete (DeletePlayerCardCommand command){
            Console.WriteLine(command);

            await Mediator.Send (command);
            return NoContent ();
        }
        [HttpPost]
        [Route("CreateCardGift")]
        public async Task<ActionResult> CreateCardGift(CreateCardGiftCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id1}/{id2}")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update ([FromRoute](long,long) id, UpdatePlayerCardCommand command){
            if (id.Item1 != command.CardId && id.Item2!= command.PlayerId)
                return BadRequest ();
            await Mediator.Send (command);
            return NoContent ();
        }
    }
}