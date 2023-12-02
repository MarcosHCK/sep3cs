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
using DataClash.Application.Statistics.TopClansByRegion;
using DataClash.Application.Statistics.CompletedChallenges;
using DataClash.Application.Statistics.MostGiftedCardsByRegion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataClash.WebUI.Controllers
{
    public class TopClansController : ApiControllerBase
   {
       [HttpGet]
       public async Task<ActionResult<List<string[]>>> GetTopClans()
       {
           return await Mediator.Send(new GetTopClansQuery());
       }
   }


   public class CompletedChallengesController : ApiControllerBase
   {
       [HttpGet]
       public async Task<ActionResult<List<string[]>>> GetCompletedChallenges()
       {
           return await Mediator.Send(new GetCompletedChallengesQuery());
       }
   }

   public class MostGiftedCardsController : ApiControllerBase
   {
       [HttpGet]
       public async Task<ActionResult<List<string[]>>> GetMostGiftedCards()
       {
           return await Mediator.Send(new GetMostGiftedCardsQuery());
       }
   }
}


        /*private readonly IApplicationDbContext _context;
        private readonly BestClans _bestClans;
        private readonly ILogger _logger;

        public BestClansController(IApplicationDbContext context, ILogger<BestClansController> logger)
        {
            _context = context;
            _bestClans = new BestClans();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string[]>> GetTopClansByRegion()
        {
            _logger.LogInformation("Realizando consulta para obtener los mejores clanes por region...");
            var result = await _bestClans.GetTopClansByRegion(_context);
            _logger.LogInformation("Resultado de la consulta: {result}", result.First());
            return result;
        }*/
    /*
    [ApiController]
    [Route("api/[controller]")]
    public class CompletedChallengesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly CompletedChallenges _completedChallenges;
        private readonly ILogger _logger;

        public CompletedChallengesController(IApplicationDbContext context, ILogger<CompletedChallengesController> logger)
        {
            _context = context;
            _completedChallenges = new CompletedChallenges();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string[]>> GetCompletedChallenges()
        {
            _logger.LogInformation("Realizando consulta para obtener los desafios completados...");
            var result = await _completedChallenges.GetCompletedChallenge(_context);
            _logger.LogInformation("Resultado de la consulta: {result}", result.First());
            return result;
        }
    }
}


/*
public class MostGiftedCardsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly MostGiftedCards _mostGiftedCards;
        private readonly ILogger _logger;

       public MostGiftedCardsController(IApplicationDbContext context,  ILogger<MostGiftedCardsController> logger)
       {
           _context = context;
           _mostGiftedCards = new MostGiftedCards();
           _logger = logger;
       }

       [HttpGet]
       public async Task<IEnumerable<object>> GetMostDonatedCardsByRegion()
       {
           _logger.LogInformation("Realizando consulta para obtener los mejores clanes por region...");
           var result = await _mostGiftedCards.GetMostDonatedCardsByRegion(_context);
           _logger.LogInformation("Resultado de la consulta: {result}", result);
           return result;
       }
   }

}

    
public class BestPlayersController : ControllerBase
{
   private readonly IApplicationDbContext _context;
   private readonly BestPlayers _bestPlayers;
   private readonly ILogger _logger;

   public BestPlayersController(IApplicationDbContext context, ILogger<BestPlayersController> logger)
   {
       _context = context;
       _bestPlayers = new BestPlayers();
       _logger = logger;
   }

   [HttpGet("{warId}")]
   public IEnumerable<dynamic> GetBestPlayers(int warId)
   {
       _logger.LogInformation("Realizando consulta para obtener los mejores jugadores...");
       var result = _bestPlayers.GetBestPlayer(_context, warId);
       _logger.LogInformation("Resultado de la consulta: {result}", result);
       return result;
   }

   [HttpGet("warIds")]
   public IEnumerable<long> GetAllWarIds()
   {
       _logger.LogInformation("Realizando consulta para obtener todos los IDs de guerra...");
       var result =_bestPlayers.GetAllWarIds(_context);
       _logger.LogInformation("Resultado de la consulta: {result}", result);
       return result;
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
            return _mostPopularCards.GetMostPopularCards(_context, clanName);
        }


        [HttpGet("clanNames")]
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
       public async Task<IEnumerable<(Card, Region, long)>> GetMostDonatedCardsByRegion()
       {
           return await _mostGiftedCards.GetMostDonatedCardsByRegion(_context);
       }
   }

}
*/