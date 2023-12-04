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
  public record TopClansBrief
    {
      public string Name { get; init; } = null!;
      public string Region { get; init; } = null!;
      public string Throphies { get; init; } = null!;
    }

  public record CompletedChallengesBrief
    {
      public string Player { get; init; } = null!;
      public string Challenge { get; init; } = null!;
    }

  public record MostGiftedCardsBrief
    {
      public string Card { get; init; } = null!;
      public string Region { get; init; } = null!;
      public string Donations { get; init; } = null!;
    }

  public record BestPlayerBrief
    {
      public string Player { get; init; } = null!;
      public string Clan { get; init; } = null!;
      public string Throphies { get; init; } = null!;
    }

  public record PopularCardsBrief
    {
      public string Card { get; init; } = null!;
      public string Type { get; init; } = null!;
      public string Clan { get; init; } = null!;
    }

  public class TopClansController : ExporterControllerBase<TopClansBrief>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetTopClans ()
        => await Mediator.Send (new GetTopClansQuery ());
      [HttpGet ("Export")]
      public async Task<FileResult> ExportTopClans ([FromQuery] string contentType, [FromQuery] string? fileName)
        => await ExportResult (contentType, fileName, async () => {
          var values = await Mediator.Send (new GetTopClansQuery ());
          return values.Select (x => new TopClansBrief { Name = x[0], Region = x[1], Throphies = x[2] });
        });
    }

  public class CompletedChallengesController : ExporterControllerBase<CompletedChallengesBrief>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetCompletedChallenges ()
        => await Mediator.Send (new GetCompletedChallengesQuery ());
      [HttpGet ("Export")]
      public async Task<FileResult> ExportCompletedChallenges ([FromQuery] string contentType, [FromQuery] string? fileName)
        => await ExportResult (contentType, fileName, async () => {
          var values = await Mediator.Send (new GetCompletedChallengesQuery ());
          return values.Select (x => new CompletedChallengesBrief { Player = x[0], Challenge = x[1] });
        });
    }

  public class MostGiftedCardsController : ExporterControllerBase<MostGiftedCardsBrief>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetMostGiftedCards ()
        => await Mediator.Send (new GetMostGiftedCardsQuery ());
      [HttpGet ("Export")]
      public async Task<FileResult> ExportMostGiftedCards ([FromQuery] string contentType, [FromQuery] string? fileName)
        => await ExportResult (contentType, fileName, async () =>
          {
            var values = await Mediator.Send (new GetMostGiftedCardsQuery ());
            return values.Select (x => new MostGiftedCardsBrief { Card = x[0], Region = x[1], Donations = x[2] });
          });
    }

  public class AllWarIdsController : ExporterControllerBase<long>
    {
      [HttpGet]
      public async Task<ActionResult<List<long>>> GetAllWarIds ()
        => await Mediator.Send (new GetAllWarIdsQuery ());
      [HttpGet ("Export")]
      public async Task<FileResult> ExportAllWarIds ([FromQuery] string contentType, [FromQuery] string? fileName)
        => await ExportResult (contentType, fileName, async () =>
          {
            var values = await Mediator.Send (new GetAllWarIdsQuery ());
            return values;
          });
    }

  public class BestPlayerController : ExporterControllerBase<BestPlayerBrief>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetBestPlayer (int warId)
        => await Mediator.Send (new GetBestPlayerQuery (warId));
      [HttpGet ("Export")]
      public async Task<ActionResult<List<string[]>>> ExportBestPlayer ([FromQuery] string contentType, [FromQuery] string? fileName, [FromQuery] int warId)
        => await ExportResult (contentType, fileName, async () =>
          {
            var values = await Mediator.Send (new GetBestPlayerQuery (warId));
            return values.Select (x => new BestPlayerBrief { Player = x[0], Clan = x[1], Throphies = x[2] });
          });
    }

  public class AllClanNamesController : ExporterControllerBase<string>
    {
      [HttpGet]
      public async Task<ActionResult<List<string>>> GetAllClanNames ()
        => await Mediator.Send (new GetAllClanNamesQuery ());
      [HttpGet ("Export")]
      public async Task<FileResult> ExportAllClanNames ([FromQuery] string contentType, [FromQuery] string? fileName)
        => await ExportResult (contentType, fileName, async () =>
          {
            var values = await Mediator.Send (new GetAllClanNamesQuery ());
            return values;
          });
    }

  public class PopularCardsController : ExporterControllerBase<PopularCardsBrief>
    {
      [HttpGet]
      public async Task<ActionResult<List<string[]>>> GetPopularCards (string clanName)
        => await Mediator.Send (new GetPopularCardsQuery (clanName));
      [HttpGet ("Export")]
      public async Task<FileResult> ExportPopularCards ([FromQuery] string contentType, [FromQuery] string? fileName, [FromQuery] string clanName)
        => await ExportResult (contentType, fileName, async () =>
          {
            var values = await Mediator.Send (new GetPopularCardsQuery (clanName));
            return values.Select (x => new PopularCardsBrief { Card = x[0], Type = x[1], Clan = x[2] });
          });
    }
}

