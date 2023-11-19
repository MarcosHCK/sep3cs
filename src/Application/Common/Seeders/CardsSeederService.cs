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
using DataClash.Domain.Entities;
using DataClash.Domain.Enums;

namespace DataClash.Application.Common.Seeders
{
  public class CardsSeederProvider : IApplicationDbContextSeederProvider
    {
      private void AddCard<T> (IApplicationDbContext context,
            string id, string description,
            double elixirCost, long initialLevel, string name,
            Quality quality,
            double damageRadius, double areaDamage, double towerDamage,
            TimeSpan duration)
          where T : MagicCard, new ()
        {
          if (context.Cards.All (c => c.Name != id))
            {
              context.Cards.Add (new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  DamageRadius = damageRadius,
                  AreaDamage = areaDamage,
                  TowerDamage = towerDamage,
                  Duration = duration });
            }
        }

      private void AddCard<T> (IApplicationDbContext context,
            string id, string description,
            double elixirCost, long initialLevel, string name,
            Quality quality,
            double hitPoints, double rangeDamage, double attackPaseRate)
          where T : StructCard, new ()
        {
          if (context.Cards.All (c => c.Name != id))
            {
              context.Cards.Add (new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  HitPoints = hitPoints,
                  RangeDamage = rangeDamage,
                  AttackPaseRate = attackPaseRate });
            }
        }

      private void AddCard<T> (IApplicationDbContext context,
            string id, string description,
            double elixirCost, long initialLevel, string name,
            Quality quality,
            double areaDamage, double hitPoints, long unitCount)
          where T : TroopCard, new ()
        {
          if (context.Cards.All (c => c.Name != id))
            {
              context.Cards.Add (new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  AreaDamage = areaDamage,
                  HitPoints = hitPoints,
                  UnitCount = unitCount });
            }
        }

      public void SeedAsync (IApplicationDbContext context)
        {
          AddCard<TroopCard> (context, "angry_barbarian", "An angry barbarian", 6, 1, "Elite Barbarian", Quality.Normal, 0, 0, 0);
          AddCard<StructCard> (context, "goblindrill", "A powerfull gublin-made drill", 4, 1, "Goblin Drill", Quality.Normal, 0, 0, 0);
          AddCard<MagicCard> (context, "fire_fireball", "A magic ball of fire", 4, 1, "Ball of fire", Quality.Normal, 0, 0, 0, TimeSpan.Zero);
        }
    }
}
