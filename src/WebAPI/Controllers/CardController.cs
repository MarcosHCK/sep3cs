using DataClash.Application.Common.Models;
using DataClash.Application.PlayerCards.Commands.CreatePlayerCard;
using DataClash.Application.PlayerCards.Commands.DeletePlayerCard;
using DataClash.Application.PlayerCards.Commands.UpdatePlayerCard;
using DataClash.Application.PlayerCards.Queries.GetPlayerCardsWithPagination;
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
        [HttpDelete ("{id1}/{is2}")]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete ((long,long)id){
            await Mediator.Send (new DeletePlayerCardCommand (id));
            return NoContent ();
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