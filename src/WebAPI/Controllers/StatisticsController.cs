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

using DataClash.Application.Common.Interfaces;
using DataClash.Application.Statistics.TopPlayersInWars;
using DataClash.Application.Statistics.TopClansByRegion;
using DataClash.Application.Statistics.CompletedChallenges;
using DataClash.Application.Statistics.MostPopularCards;
using DataClash.Application.Statistics.MostGiftedCardsByRegion;
using Microsoft.AspNetCore.Mvc;

using DataClash.Domain.Entities;
using DataClash.Domain.ValueObjects;

namespace DataClash.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestPlayersController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly BestPlayers _bestPlayers;

        public BestPlayersController(IApplicationDbContext context)
        {
            _context = context;
            _bestPlayers = new BestPlayers();
        }

        [HttpGet("{warId}")]
        public IEnumerable<dynamic> GetBestPlayers(int warId)
        {
            return _bestPlayers.GetBestPlayer(_context, warId);
        }

        [HttpGet("warIds")]
        public IEnumerable<long> GetAllWarIds()
        {
            return _bestPlayers.GetAllWarIds(_context);
        }

    }
    public class BestClansController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly BestClans _bestClans;

        public BestClansController(IApplicationDbContext context)
        {
            _context = context;
            _bestClans = new BestClans();
        }

        [HttpGet]
        public async Task<IEnumerable<Tuple<Clan, Region, long>>> GetTopClansByRegion()
        {
            return await _bestClans.GetTopClansByRegion(_context);
        }

    }

    public class CompletedChallengesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly CompletedChallenges _completedChallenges;

        public CompletedChallengesController(IApplicationDbContext context)
        {
            _context = context;
            _completedChallenges = new CompletedChallenges();
        }

        [HttpGet]
        public async Task<IEnumerable<Tuple<Player?, Challenge?>>> GetCompletedChallenges()
        {
            return _completedChallenges.GetCompletedChallenge(_context);
        }
    }

    public class MostPopularCardsController : ControllerBase
   {
       private readonly IApplicationDbContext _context;
       private readonly MostPopularCards _mostPopularCards;

       public MostPopularCardsController(IApplicationDbContext context)
       {
           _context = context;
           _mostPopularCards = new MostPopularCards();
       }

       [HttpGet("{clanName}")]
       public async Task<IEnumerable<Tuple<Card, string, Clan>>> GetMostPopularCards(string clanName)
       {
           return  _mostPopularCards.GetMostPopularCards(_context, clanName);
       }

       [HttpGet("{clanNames}")]
       public IEnumerable<string> GetAllClansNames()
       {
           return _mostPopularCards.GetAllClansNames(_context);
       }
   }

   public class MostGiftedCardsController : ControllerBase
   {
       private readonly IApplicationDbContext _context;
       private readonly MostGiftedCards _mostGiftedCards;

       public MostGiftedCardsController(IApplicationDbContext context)
       {
           _context = context;
           _mostGiftedCards = new MostGiftedCards();
       }

       [HttpGet]
       public async Task<IEnumerable<Tuple<Card, Region, long>>> GetMostDonatedCardsByRegion()
       {
           return await _mostGiftedCards.GetMostDonatedCardsByRegion(_context);
       }
   }

}
