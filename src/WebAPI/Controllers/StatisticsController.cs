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
using DataClash.Application.Statistics.AllClanNames;
using DataClash.Application.Statistics.AllWarIds;
using DataClash.Application.Statistics.BestPlayer;
using DataClash.Application.Statistics.CompletedChallenges;
using DataClash.Application.Statistics.MostGiftedCardsByRegion;
using DataClash.Application.Statistics.PopularCards;
using DataClash.Application.Statistics.TopClansByRegion;
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  public class TopClansController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetTopClans ()
        => await Mediator.Send (new GetTopClansQuery ());
    }

  public class CompletedChallengesController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetCompletedChallenges ()
        => await Mediator.Send(new GetCompletedChallengesQuery ());
    }

  public class MostGiftedCardsController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetMostGiftedCards ()
        => await Mediator.Send(new GetMostGiftedCardsQuery ());
    }

  public class AllWarIdsController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<long>>> GetAllWarIds ()
        => await Mediator.Send(new GetAllWarIdsQuery ());
    }

  public class BestPlayerController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetBestPlayer (int warId)
        => await Mediator.Send(new GetBestPlayerQuery (warId));
    }

  public class AllClanNamesController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string>>> GetAllClanNames ()
        => await Mediator.Send(new GetAllClanNamesQuery ());
    }

  public class PopularCardsController : ApiControllerBase
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetPopularCards (string clanName)
        => await Mediator.Send(new GetPopularCardsQuery (clanName));
    }
}

