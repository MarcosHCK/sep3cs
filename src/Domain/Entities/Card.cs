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
using DataClash.Domain.Common;
using DataClash.Domain.Enums;

namespace DataClash.Domain.Entities
{
  public abstract class Card : BaseEntity
    {
      public string? Description { get; set; }
      public double ElixirCost { get; set; }
      public long InitialLevel { get; set; }
      public string? Name { get; set; }
      public Quality Quality { get; set; }
      public string? Picture { get; set; }

    }

  public class MagicCard : Card
    {
      public double DamageRadius {get; set; }
      public double AreaDamage { get; set; }
      public double TowerDamage { get; set; }
      public TimeSpan Duration { get; set; }

    }

  public class StructCard : Card
    {
      public double HitPoints { get; set; }
      public double RangeDamage { get; set; }
      public double AttackPaseRate { get; set; }
    }

  public class TroopCard : Card
    {
      public double AreaDamage { get; set; }
      public double HitPoints { get; set; }
      public long UnitCount { get; set; }
    }
}
