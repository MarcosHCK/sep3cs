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

using DataClash.Application.Common.Mappings;
using DataClash.Domain.Enums;
using DataClash.Domain.Entities;

namespace DataClash.Application.StructCards.Queries.GetStructCard
{
 public class CardBriefDto : IMapFrom<Card>
   {
     public long Id { get; init; }
     public string? Description { get; init; }
     public double ElixirCost { get; init; }
     public long InitialLevel { get; init; }
     public string? Name { get; init; }
     public Quality Quality { get; init; }
     public string? Picture { get; init; }
   }


 public class StructCardBriefDto : CardBriefDto, IMapFrom<StructCard>
 {
   public double HitPoints { get; init; }
   public double RangeDamage { get; init; }
   public double AttackPaseRate { get; init; }
 }

 
}
