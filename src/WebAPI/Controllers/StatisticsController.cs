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
  public class TopClansController : ExporterControllerBase<string[]>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetTopClans ()
        => await Mediator.Send (new GetTopClansQuery ());
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportTopClans ([FromQuery] string contentType, [FromQuery] string fileName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetTopClansQuery ()));
    }

  public class CompletedChallengesController : ExporterControllerBase<string[]>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetCompletedChallenges ()
        => await Mediator.Send (new GetCompletedChallengesQuery ());
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportCompletedChallenges ([FromQuery] string contentType, [FromQuery] string fileName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetCompletedChallengesQuery ()));
    }

  public class MostGiftedCardsController : ExporterControllerBase<string[]>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetMostGiftedCards ()
        => await Mediator.Send (new GetMostGiftedCardsQuery ());
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportMostGiftedCards ([FromQuery] string contentType, [FromQuery] string fileName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetMostGiftedCardsQuery ()));
    }

  public class AllWarIdsController : ExporterControllerBase<long>
    {
      [HttpGet]
      public async Task<ActionResult<List<long>>> GetAllWarIds ()
        => await Mediator.Send (new GetAllWarIdsQuery ());
      [HttpGet ("Export")]
      public async Task<ActionResult<List<long>>> ExportAllWarIds ([FromQuery] string contentType, [FromQuery] string fileName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetAllWarIdsQuery ()));
    }

  public class BestPlayerController : ExporterControllerBase<string[]>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetBestPlayer (int warId)
        => await Mediator.Send (new GetBestPlayerQuery (warId));
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportBestPlayer ([FromQuery] string contentType, [FromQuery] string fileName, [FromQuery] int warId)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetBestPlayerQuery (warId)));
    }

  public class AllClanNamesController : ExporterControllerBase<string>
    {
      [HttpGet]
      public async Task<ActionResult<List<string>>> GetAllClanNames ()
        => await Mediator.Send (new GetAllClanNamesQuery ());
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string>>> ExportAllClanNames ([FromQuery] string contentType, [FromQuery] string fileName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetAllClanNamesQuery ()));
    }

  public class PopularCardsController : ExporterControllerBase<string[]>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetPopularCards (string clanName)
        => await Mediator.Send (new GetPopularCardsQuery (clanName));
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportPopularCards ([FromQuery] string contentType, [FromQuery] string fileName, [FromQuery] string clanName)
        => await ExportResult (contentType, fileName, () => Mediator.Send (new GetPopularCardsQuery (clanName)));
    }
}

