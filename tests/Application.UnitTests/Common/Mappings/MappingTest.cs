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
using System.Runtime.Serialization;
using AutoMapper;
using DataClash.Application.Challenges.Queries.GetChallengesWithPagination;
using DataClash.Application.Common.Mappings;
using DataClash.Application.PlayerCards.Queries.GetPlayerCardsWithPagination;
using DataClash.Application.Players.Queries.GetPlayersWithPagination;
using DataClash.Application.Wars.Queries.GetWar;
using DataClash.Domain.Entities;
using NUnit.Framework;

namespace DataClash.Application.UnitTests.Common.Mappings
{
  public class MappingTests
    {
      private readonly IConfigurationProvider _configuration;
      private readonly IMapper _mapper;

      public MappingTests ()
        {
          _configuration = new MapperConfiguration (config => config.AddProfile<MappingProfile> ());
          _mapper = _configuration.CreateMapper ();
        }

      [Test]
      public void ShouldHaveValidConfiguration ()
        {
          _configuration.AssertConfigurationIsValid ();
        }

      [Test]
      [TestCase (typeof (Challenge), typeof (ChallengeBriefDto))]
      [TestCase (typeof (Player), typeof (PlayerBriefDto))]
      [TestCase (typeof (PlayerCard), typeof (PlayerCardBriefDto))]
      [TestCase (typeof (War), typeof (WarBriefDto))]
      public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
          _mapper.Map (GetInstanceOf (source), source, destination);
        }

      private object GetInstanceOf (Type type)
        {
          if (type.GetConstructor (Type.EmptyTypes) != null)
            return Activator.CreateInstance (type)!;
          // Type without parameterless constructor
          return FormatterServices.GetUninitializedObject (type);
        }
    }
}
