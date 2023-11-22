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

    public void SeedAsync(IApplicationDbContext context)
    {
      AddCard<TroopCard>(context, "angry_barbarian.png", "angry_barbarian description", 1, 1, "angry_barbarian", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "archer.png", "archer description", 1, 1, "archer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "archerqueen.png", "archerqueen description", 1, 1, "archerqueen", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "baby_dragon.png", "baby_dragon description", 1, 1, "baby_dragon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "bandit.png", "bandit description", 1, 1, "bandit", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "barbarian_card.png", "barbarian_card description", 1, 1, "barbarian_card", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "barbarian_hut.png", "barbarian_hut description", 1, 1, "barbarian_hut", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "barb_barrel.png", "barb_barrel description", 1, 1, "barb_barrel", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "bats.png", "bats description", 1, 1, "bats", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "battle_healer.png", "battle_healer description", 1, 1, "battle_healer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "battle_ram.png", "battle_ram description", 1, 1, "battle_ram", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "blowdart_goblin.png", "blowdart_goblin description", 1, 1, "blowdart_goblin", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "bomber.png", "bomber description", 1, 1, "bomber", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "bomb_tower.png", "bomb_tower description", 1, 1, "bomb_tower", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "bowler.png", "bowler description", 1, 1, "bowler", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "building_elixir_collector.png", "building_elixir_collector description", 1, 1, "building_elixir_collector", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "building_inferno.png", "building_inferno description", 1, 1, "building_inferno", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "building_mortar.png", "building_mortar description", 1, 1, "building_mortar", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "building_tesla.png", "building_tesla description", 1, 1, "building_tesla", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "building_xbow.png", "building_xbow description", 1, 1, "building_xbow", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "cannon_cart.png", "cannon_cart description", 1, 1, "cannon_cart", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "chaos_cannon.png", "chaos_cannon description", 1, 1, "chaos_cannon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "chr_balloon.png", "chr_balloon description", 1, 1, "chr_balloon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "chr_golem.png", "chr_golem description", 1, 1, "chr_golem", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "copy.png", "copy description", 1, 1, "copy", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "dark_prince.png", "dark_prince description", 1, 1, "dark_prince", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "earthquake.png", "earthquake description", 1, 1, "earthquake", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "electrogiant.png", "electrogiant description", 1, 1, "electrogiant", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "electrospirit.png", "electrospirit description", 1, 1, "electrospirit", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "electro_dragon.png", "electro_dragon description", 1, 1, "electro_dragon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "electro_wizard.png", "electro_wizard description", 1, 1, "electro_wizard", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "elixir_golem.png", "elixir_golem description", 1, 1, "elixir_golem", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "executioner.png", "executioner description", 1, 1, "executioner", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "firecracker.png", "firecracker description", 1, 1, "firecracker", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "firespirit_hut.png", "firespirit_hut description", 1, 1, "firespirit_hut", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "fire_fireball.png", "fire_fireball description", 1, 1, "fire_fireball", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "fire_furnace.png", "fire_furnace description", 1, 1, "fire_furnace", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "fire_spirits.png", "fire_spirits description", 1, 1, "fire_spirits", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "fisherman.png", "fisherman description", 1, 1, "fisherman", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "flying_machine.png", "flying_machine description", 1, 1, "flying_machine", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "freeze.png", "freeze description", 1, 1, "freeze", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "ghost.png", "ghost description", 1, 1, "ghost", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "giant.png", "giant description", 1, 1, "giant", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "giant_skeleton.png", "giant_skeleton description", 1, 1, "giant_skeleton", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblindrill.png", "goblindrill description", 1, 1, "goblindrill", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblins.png", "goblins description", 1, 1, "goblins", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblin_archer.png", "goblin_archer description", 1, 1, "goblin_archer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblin_barrel.png", "goblin_barrel description", 1, 1, "goblin_barrel", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblin_cage.png", "goblin_cage description", 1, 1, "goblin_cage", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblin_gang.png", "goblin_gang description", 1, 1, "goblin_gang", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goblin_giant.png", "goblin_giant description", 1, 1, "goblin_giant", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "goldenknight.png", "goldenknight description", 1, 1, "goldenknight", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "graveyard.png", "graveyard description", 1, 1, "graveyard", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "healspirit.png", "healspirit description", 1, 1, "healspirit", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "hogrider_champion.png", "hogrider_champion description", 1, 1, "hogrider_champion", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "hog_rider.png", "hog_rider description", 1, 1, "hog_rider", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "hunter.png", "hunter description", 1, 1, "hunter", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "icegolem.png", "icegolem description", 1, 1, "icegolem", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "ice_wizard.png", "ice_wizard description", 1, 1, "ice_wizard", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "inferno_dragon.png", "inferno_dragon description", 1, 1, "inferno_dragon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "knight.png", "knight description", 1, 1, "knight", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "lava_hound.png", "lava_hound description", 1, 1, "lava_hound", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "lightning.png", "lightning description", 1, 1, "lightning", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "little_prince.png", "little_prince description", 1, 1, "little_prince", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "magic_archer.png", "magic_archer description", 1, 1, "magic_archer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "mega_knight.png", "mega_knight description", 1, 1, "mega_knight", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "mega_minion.png", "mega_minion description", 1, 1, "mega_minion", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "mightyminer.png", "mightyminer description", 1, 1, "mightyminer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "miner.png", "miner description", 1, 1, "miner", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "minion.png", "minion description", 1, 1, "minion", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "minion_horde.png", "minion_horde description", 1, 1, "minion_horde", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "minipekka.png", "minipekka description", 1, 1, "minipekka", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "mirror.png", "mirror description", 1, 1, "mirror", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "monk.png", "monk description", 1, 1, "monk", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "motherwitch.png", "motherwitch description", 1, 1, "motherwitch", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "musketeer.png", "musketeer description", 1, 1, "musketeer", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "nightwitch.png", "nightwitch description", 1, 1, "nightwitch", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "order_volley.png", "order_volley description", 1, 1, "order_volley", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "party_hut.png", "party_hut description", 1, 1, "party_hut", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "party_rocket.png", "party_rocket description", 1, 1, "party_rocket", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "pekka.png", "pekka description", 1, 1, "pekka", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "phoenix.png", "phoenix description", 1, 1, "phoenix", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "poison.png", "poison description", 1, 1, "poison", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "prince.png", "prince description", 1, 1, "prince", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "princess.png", "princess description", 1, 1, "princess", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "rage.png", "rage description", 1, 1, "rage", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "rage_barbarian.png", "rage_barbarian description", 1, 1, "rage_barbarian", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "ram_rider.png", "ram_rider description", 1, 1, "ram_rider", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "rascals.png", "rascals description", 1, 1, "rascals", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "rocket.png", "rocket description", 1, 1, "rocket", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "royalgiant.png", "royalgiant description", 1, 1, "royalgiant", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "royal_delivery.png", "royal_delivery description", 1, 1, "royal_delivery", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "royal_hog.png", "royal_hog description", 1, 1, "royal_hog", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "royal_recruits.png", "royal_recruits description", 1, 1, "royal_recruits", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeletondragon.png", "skeletondragon description", 1, 1, "skeletondragon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeletonking.png", "skeletonking description", 1, 1, "skeletonking", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeletons_card.png", "skeletons_card description", 1, 1, "skeletons_card", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeleton_balloon.png", "skeleton_balloon description", 1, 1, "skeleton_balloon", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeleton_horde.png", "skeleton_horde description", 1, 1, "skeleton_horde", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "skeleton_warriors.png", "skeleton_warriors description", 1, 1, "skeleton_warriors", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "snowball.png", "snowball description", 1, 1, "snowball", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "snow_spirits.png", "snow_spirits description", 1, 1, "snow_spirits", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "superarcher.png", "superarcher description", 1, 1, "superarcher", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "superhogrider.png", "superhogrider description", 1, 1, "superhogrider", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "supericegolem.png", "supericegolem description", 1, 1, "supericegolem", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "supermagicarcher.png", "supermagicarcher description", 1, 1, "supermagicarcher", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "superminipekka.png", "superminipekka description", 1, 1, "superminipekka", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "superwitch.png", "superwitch description", 1, 1, "superwitch", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "super_lava_hound.png", "super_lava_hound description", 1, 1, "super_lava_hound", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "the_log.png", "the_log description", 1, 1, "the_log", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "three_musketeers.png", "three_musketeers description", 1, 1, "three_musketeers", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "tombstone.png", "tombstone description", 1, 1, "tombstone", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "tornado.png", "tornado description", 1, 1, "tornado", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "valkyrie.png", "valkyrie description", 1, 1, "valkyrie", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "wallbreaker.png", "wallbreaker description", 1, 1, "wallbreaker", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "witch.png", "witch description", 1, 1, "witch", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "wizard.png", "wizard description", 1, 1, "wizard", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "zap.png", "zap description", 1, 1, "zap", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "zapMachine.png", "zapMachine description", 1, 1, "zapMachine", Quality.Normal, 100, 100, 1);
      AddCard<TroopCard>(context, "zappies.png", "zappies description", 1, 1, "zappies", Quality.Normal, 100, 100, 1);
    }
  }
}
