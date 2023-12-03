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
            string description, double elixirCost, long initialLevel, string name, Quality quality, string picture,
            double damageRadius, double areaDamage, double towerDamage, TimeSpan duration)
          where T : MagicCard, new ()
        {
          if (context.Cards.All (c => c.Picture != picture))
              {var card= new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  Picture = picture,
                  DamageRadius = damageRadius,
                  AreaDamage = areaDamage,
                  TowerDamage = towerDamage,
                  Duration = duration };
                  context.Cards.Add (card);
                  context.MagicCards.Add (card);}
        }

      private void AddCard<T> (IApplicationDbContext context,
            string description, double elixirCost, long initialLevel, string name, Quality quality, string picture,
            double hitPoints, double rangeDamage, double attackPaseRate)
          where T : StructCard, new ()
        {
          if (context.Cards.All (c => c.Picture != picture))
              {var card= new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  Picture = picture,
                  HitPoints = hitPoints,
                  RangeDamage = rangeDamage,
                  AttackPaseRate = attackPaseRate };
                  context.Cards.Add (card);
                  context.StructCards.Add (card);}
        }

      private void AddCard<T> (IApplicationDbContext context,
            string description, double elixirCost, long initialLevel, string name, Quality quality, string picture,
            double areaDamage, double hitPoints, long unitCount)
          where T : TroopCard, new ()
        {
          if (context.Cards.All (c => c.Picture != picture))
           {var card= new T
                { Description = description,
                  ElixirCost = elixirCost,
                  InitialLevel = initialLevel,
                  Name = name,
                  Quality = quality,
                  Picture = picture,
                  AreaDamage = areaDamage,
                  HitPoints = hitPoints,
                  UnitCount = unitCount };
                  context.Cards.Add (card);
                  context.TroopCards.Add (card);}
              

        }

    public void SeedAsync(IApplicationDbContext context)
      {
        AddCard<TroopCard>(context, "angry_barbarian description", 1, 1, "angry_barbarian", Quality.Normal, "angry_barbarian", 100, 100, 1);
        AddCard<TroopCard>(context, "A pair of lightly armored ranged attackers. They'll help you take down ground and air units, but you're on your own with hair coloring advice.", 3, 1, "archer", Quality.Normal, "archer", 42, 119, 2);
        AddCard<TroopCard>(context, "archerqueen description", 1, 1, "archerqueen", Quality.Normal, "archerqueen", 100, 100, 1);
        AddCard<TroopCard>(context, "baby_dragon description", 1, 1, "baby_dragon", Quality.Normal, "baby_dragon", 100, 100, 1);
        AddCard<TroopCard>(context, "bandit description", 1, 1, "bandit", Quality.Normal, "bandit", 100, 100, 1);
        AddCard<TroopCard>(context, "barbarian_card description", 1, 1, "barbarian_card", Quality.Normal, "barbarian_card", 100, 100, 1);
        AddCard<TroopCard>(context, "barbarian_hut description", 1, 1, "barbarian_hut", Quality.Normal, "barbarian_hut", 100, 100, 1);
        AddCard<TroopCard>(context, "barb_barrel description", 1, 1, "barb_barrel", Quality.Normal, "barb_barrel", 100, 100, 1);
        AddCard<TroopCard>(context, "bats description", 1, 1, "bats", Quality.Normal, "bats", 100, 100, 1);
        AddCard<TroopCard>(context, "battle_healer description", 1, 1, "battle_healer", Quality.Normal, "battle_healer", 100, 100, 1);
        AddCard<TroopCard>(context, "battle_ram description", 1, 1, "battle_ram", Quality.Normal, "battle_ram", 100, 100, 1);
        AddCard<TroopCard>(context, "blowdart_goblin description", 1, 1, "blowdart_goblin", Quality.Normal, "blowdart_goblin", 100, 100, 1);
        AddCard<TroopCard>(context, "bomber description", 1, 1, "bomber", Quality.Normal, "bomber", 100, 100, 1);
        AddCard<TroopCard>(context, "bomb_tower description", 1, 1, "bomb_tower", Quality.Normal, "bomb_tower", 100, 100, 1);
        AddCard<StructCard>(context, "bowler description", 1, 1, "bowler", Quality.Normal, "bowler", 100, 100, 1);
        AddCard<TroopCard>(context, "building_elixir_collector description", 1, 1, "building_elixir_collector", Quality.Normal, "building_elixir_collector", 100, 100, 1);
        AddCard<TroopCard>(context, "building_inferno description", 1, 1, "building_inferno", Quality.Normal, "building_inferno", 100, 100, 1);
        AddCard<TroopCard>(context, "building_mortar description", 1, 1, "building_mortar", Quality.Normal, "building_mortar", 100, 100, 1);
        AddCard<TroopCard>(context, "building_tesla description", 1, 1, "building_tesla", Quality.Normal, "building_tesla", 100, 100, 1);
        AddCard<TroopCard>(context, "building_xbow description", 1, 1, "building_xbow", Quality.Normal, "building_xbow", 100, 100, 1);
        AddCard<TroopCard>(context, "cannon_cart description", 1, 1, "cannon_cart", Quality.Normal, "cannon_cart", 100, 100, 1);
        AddCard<TroopCard>(context, "chaos_cannon description", 1, 1, "chaos_cannon", Quality.Normal, "chaos_cannon", 100, 100, 1);
        AddCard<TroopCard>(context, "chr_balloon description", 1, 1, "chr_balloon", Quality.Normal, "chr_balloon", 100, 100, 1);
        AddCard<TroopCard>(context, "chr_golem description", 1, 1, "chr_golem", Quality.Normal, "chr_golem", 100, 100, 1);
        AddCard<TroopCard>(context, "copy description", 1, 1, "copy", Quality.Normal, "copy", 100, 100, 1);
        AddCard<TroopCard>(context, "dark_prince description", 1, 1, "dark_prince", Quality.Normal, "dark_prince", 100, 100, 1);
        AddCard<StructCard>(context, "earthquake description", 1, 1, "earth", Quality.Normal, "earthquake", 100, 100, 1);
        AddCard<StructCard>(context, "electrogiant description", 1, 1, "electro", Quality.Normal, "electrogiant", 100, 100, 1);
        AddCard<TroopCard>(context, "electrospirit description", 1, 1, "electrospirit", Quality.Normal, "electrospirit", 100, 100, 1);
        AddCard<TroopCard>(context, "electro_dragon description", 1, 1, "electro_dragon", Quality.Normal, "electro_dragon", 100, 100, 1);
        AddCard<TroopCard>(context, "electro_wizard description", 1, 1, "electro_wizard", Quality.Normal, "electro_wizard", 100, 100, 1);
        AddCard<TroopCard>(context, "elixir_golem description", 1, 1, "elixir_golem", Quality.Normal, "elixir_golem", 100, 100, 1);
        AddCard<TroopCard>(context, "executioner description", 1, 1, "executioner", Quality.Normal, "executioner", 100, 100, 1);
        AddCard<TroopCard>(context, "firecracker description", 1, 1, "firecracker", Quality.Normal, "firecracker", 100, 100, 1);
        AddCard<TroopCard>(context, "firespirit_hut description", 1, 1, "firespirit_hut", Quality.Normal, "firespirit_hut", 100, 100, 1);
        AddCard<TroopCard>(context, "fire_fireball description", 1, 1, "fire_fireball", Quality.Normal, "fire_fireball", 100, 100, 1);
        AddCard<TroopCard>(context, "fire_furnace description", 1, 1, "fire_furnace", Quality.Normal, "fire_furnace", 100, 100, 1);
        AddCard<TroopCard>(context, "fire_spirits description", 1, 1, "fire_spirits", Quality.Normal, "fire_spirits", 100, 100, 1);
        AddCard<TroopCard>(context, "fisherman description", 1, 1, "fisherman", Quality.Normal, "fisherman", 100, 100, 1);
        AddCard<TroopCard>(context, "flying_machine description", 1, 1, "flying_machine", Quality.Normal, "flying_machine", 100, 100, 1);
        AddCard<TroopCard>(context, "freeze description", 1, 1, "freeze", Quality.Normal, "freeze", 100, 100, 1);
        AddCard<TroopCard>(context, "ghost description", 1, 1, "ghost", Quality.Normal, "ghost", 100, 100, 1);
        AddCard<TroopCard>(context, "Slow but durable, only attacks buildings. A real one-man wrecking crew!", 5, 1, "giant", Quality.Rare, "giant", 120, 1930, 1);
        AddCard<TroopCard>(context, "giant_skeleton description", 1, 1, "giant_skeleton", Quality.Normal, "giant_skeleton", 100, 100, 1);
        AddCard<TroopCard>(context, "goblindrill description", 1, 1, "goblindrill", Quality.Normal, "goblindrill", 100, 100, 1);
        AddCard<TroopCard>(context, " Four fast, unarmored melee attackers. Small, fast, green and mean!", 2, 1, "goblins", Quality.Normal, "goblins", 47, 79, 4);
        AddCard<TroopCard>(context, "goblin_archer description", 1, 1, "goblin_archer", Quality.Normal, "goblin_archer", 100, 100, 1);
        AddCard<TroopCard>(context, "goblin_barrel description", 1, 1, "goblin_barrel", Quality.Normal, "goblin_barrel", 100, 100, 1);
        AddCard<TroopCard>(context, "goblin_cage description", 1, 1, "goblin_cage", Quality.Normal, "goblin_cage", 100, 100, 1);
        AddCard<TroopCard>(context, "goblin_gang description", 1, 1, "goblin_gang", Quality.Normal, "goblin_gang", 100, 100, 1);
        AddCard<TroopCard>(context, "goblin_giant description", 1, 1, "goblin_giant", Quality.Normal, "goblin_giant", 100, 100, 1);
        AddCard<TroopCard>(context, "goldenknight description", 1, 1, "goldenknight", Quality.Normal, "goldenknight", 100, 100, 1);
        AddCard<TroopCard>(context, "graveyard description", 1, 1, "graveyard", Quality.Normal, "graveyard", 100, 100, 1);
        AddCard<TroopCard>(context, "healspirit description", 1, 1, "healspirit", Quality.Normal, "healspirit", 100, 100, 1);
        AddCard<TroopCard>(context, "hogrider_champion description", 1, 1, "hogrider_champion", Quality.Normal, "hogrider_champion", 100, 100, 1);
        AddCard<TroopCard>(context, "hog_rider description", 1, 1, "hog_rider", Quality.Normal, "hog_rider", 100, 100, 1);
        AddCard<TroopCard>(context, "hunter description", 1, 1, "hunter", Quality.Normal, "hunter", 100, 100, 1);
        AddCard<TroopCard>(context, "icegolem description", 1, 1, "icegolem", Quality.Normal, "icegolem", 100, 100, 1);
        AddCard<TroopCard>(context, "ice_wizard description", 1, 1, "ice_wizard", Quality.Normal, "ice_wizard", 100, 100, 1);
        AddCard<TroopCard>(context, "inferno_dragon description", 1, 1, "inferno_dragon", Quality.Normal, "inferno_dragon", 100, 100, 1);
        AddCard<TroopCard>(context, "A tough melee fighter. The Barbarian's handsome, cultured cousin. Rumor has it that he was knighted based on the sheer awesomeness of his mustache alone.", 1, 1, "knight", Quality.Normal, "knight", 79, 690, 1);
        AddCard<TroopCard>(context, "lava_hound description", 1, 1, "lava_hound", Quality.Normal, "lava_hound", 100, 100, 1);
        AddCard<TroopCard>(context, "lightning description", 1, 1, "lightning", Quality.Normal, "lightning", 100, 100, 1);
        AddCard<TroopCard>(context, "little_prince description", 1, 1, "little_prince", Quality.Normal, "little_prince", 100, 100, 1);
        AddCard<TroopCard>(context, "magic_archer description", 1, 1, "magic_archer", Quality.Normal, "magic_archer", 100, 100, 1);
        AddCard<TroopCard>(context, "mega_knight description", 1, 1, "mega_knight", Quality.Normal, "mega_knight", 100, 100, 1);
        AddCard<TroopCard>(context, "mega_minion description", 1, 1, "mega_minion", Quality.Normal, "mega_minion", 100, 100, 1);
        AddCard<TroopCard>(context, "mightyminer description", 1, 1, "mightyminer", Quality.Normal, "mightyminer", 100, 100, 1);
        AddCard<TroopCard>(context, "miner description", 1, 1, "miner", Quality.Normal, "miner", 100, 100, 1);
        AddCard<TroopCard>(context, "minion description", 1, 1, "minion", Quality.Normal, "minion", 100, 100, 1);
        AddCard<TroopCard>(context, "minion_horde description", 1, 1, "minion_horde", Quality.Normal, "minion_horde", 100, 100, 1);
        AddCard<TroopCard>(context, "minipekka description", 1, 1, "minipekka", Quality.Normal, "minipekka", 100, 100, 1);
        AddCard<TroopCard>(context, "mirror description", 1, 1, "mirror", Quality.Normal, "mirror", 100, 100, 1);
        AddCard<TroopCard>(context, "monk description", 1, 1, "monk", Quality.Normal, "monk", 100, 100, 1);
        AddCard<TroopCard>(context, "motherwitch description", 1, 1, "motherwitch", Quality.Normal, "motherwitch", 100, 100, 1);
        AddCard<TroopCard>(context, "musketeer description", 1, 1, "musketeer", Quality.Normal, "musketeer", 100, 100, 1);
        AddCard<TroopCard>(context, "nightwitch description", 1, 1, "nightwitch", Quality.Normal, "nightwitch", 100, 100, 1);
        AddCard<TroopCard>(context, "order_volley description", 1, 1, "order_volley", Quality.Normal, "order_volley", 100, 100, 1);
        AddCard<TroopCard>(context, "party_hut description", 1, 1, "party_hut", Quality.Normal, "party_hut", 100, 100, 1);
        AddCard<TroopCard>(context, "party_rocket description", 1, 1, "party_rocket", Quality.Normal, "party_rocket", 100, 100, 1);
        AddCard<TroopCard>(context, "A heavily armored, slow melee fighter. Swings from the hip, but packs a huge punch.", 7, 1, "pekka", Quality.Uncommon, "pekka", 510,2350, 1);
        AddCard<TroopCard>(context, "phoenix description", 1, 1, "phoenix", Quality.Normal, "phoenix", 100, 100, 1);
        AddCard<TroopCard>(context, "poison description", 1, 1, "poison", Quality.Normal, "poison", 100, 100, 1);
        AddCard<TroopCard>(context, "prince description", 1, 1, "prince", Quality.Normal, "prince", 100, 100, 1);
        AddCard<TroopCard>(context, "princess description", 1, 1, "princess", Quality.Normal, "princess", 100, 100, 1);
        AddCard<TroopCard>(context, "rage description", 1, 1, "rage", Quality.Normal, "rage", 100, 100, 1);
        AddCard<TroopCard>(context, "rage_barbarian description", 1, 1, "rage_barbarian", Quality.Normal, "rage_barbarian", 100, 100, 1);
        AddCard<TroopCard>(context, "ram_rider description", 1, 1, "ram_rider", Quality.Normal, "ram_rider", 100, 100, 1);
        AddCard<TroopCard>(context, "rascals description", 1, 1, "rascals", Quality.Normal, "rascals", 100, 100, 1);
        AddCard<TroopCard>(context, "rocket description", 1, 1, "rocket", Quality.Normal, "rocket", 100, 100, 1);
        AddCard<TroopCard>(context, "royalgiant description", 1, 1, "royalgiant", Quality.Normal, "royalgiant", 100, 100, 1);
        AddCard<TroopCard>(context, "royal_delivery description", 1, 1, "royal_delivery", Quality.Normal, "royal_delivery", 100, 100, 1);
        AddCard<TroopCard>(context, "royal_hog description", 1, 1, "royal_hog", Quality.Normal, "royal_hog", 100, 100, 1);
        AddCard<TroopCard>(context, "royal_recruits description", 1, 1, "royal_recruits", Quality.Normal, "royal_recruits", 100, 100, 1);
        AddCard<TroopCard>(context, "skeletondragon description", 1, 1, "skeletondragon", Quality.Normal, "skeletondragon", 100, 100, 1);
        AddCard<TroopCard>(context, "skeletonking description", 1, 1, "skeletonking", Quality.Normal, "skeletonking", 100, 100, 1);
        AddCard<TroopCard>(context, "skeletons_card description", 1, 1, "skeletons_card", Quality.Normal, "skeletons_card", 100, 100, 1);
        AddCard<TroopCard>(context, "skeleton_balloon description", 1, 1, "skeleton_balloon", Quality.Normal, "skeleton_balloon", 100, 100, 1);
        AddCard<TroopCard>(context, "skeleton_horde description", 1, 1, "skeleton_horde", Quality.Normal, "skeleton_horde", 100, 100, 1);
        AddCard<TroopCard>(context, "skeleton_warriors description", 1, 1, "skeleton_warriors", Quality.Normal, "skeleton_warriors", 100, 100, 1);
        AddCard<TroopCard>(context, "snowball description", 1, 1, "snowball", Quality.Normal, "snowball", 100, 100, 1);
        AddCard<StructCard>(context, "sparky description", 1, 1, "sparky", Quality.Normal, "sparky", 100, 100, 1);
        AddCard<TroopCard>(context, "superarcher description", 1, 1, "superarcher", Quality.Normal, "superarcher", 100, 100, 1);
        AddCard<TroopCard>(context, "superhogrider description", 1, 1, "superhogrider", Quality.Normal, "superhogrider", 100, 100, 1);
        AddCard<TroopCard>(context, "supericegolem description", 1, 1, "supericegolem", Quality.Normal, "supericegolem", 100, 100, 1);
        AddCard<TroopCard>(context, "supermagicarcher description", 1, 1, "supermagicarcher", Quality.Normal, "supermagicarcher", 100, 100, 1);
        AddCard<TroopCard>(context, "superminipekka description", 1, 1, "superminipekka", Quality.Normal, "superminipekka", 100, 100, 1);
        AddCard<TroopCard>(context, "superwitch description", 1, 1, "superwitch", Quality.Normal, "superwitch", 100, 100, 1);
        AddCard<TroopCard>(context, "super_lava_hound description", 1, 1, "super_lava_hound", Quality.Normal, "super_lava_hound", 100, 100, 1);
        AddCard<TroopCard>(context, "the_log description", 1, 1, "the_log", Quality.Normal, "the_log", 100, 100, 1);
        AddCard<TroopCard>(context, "three_musketeers description", 1, 1, "three_musketeers", Quality.Normal, "three_musketeers", 100, 100, 1);
        AddCard<TroopCard>(context, "tombstone description", 1, 1, "tombstone", Quality.Normal, "tombstone", 100, 100, 1);
        AddCard<TroopCard>(context, "tornado description", 1, 1, "tornado", Quality.Normal, "tornado", 100, 100, 1);
        AddCard<TroopCard>(context, "valkyrie description", 1, 1, "valkyrie", Quality.Normal, "valkyrie", 100, 100, 1);
        AddCard<TroopCard>(context, "wallbreaker description", 1, 1, "wallbreaker", Quality.Normal, "wallbreaker", 100, 100, 1);
        AddCard<TroopCard>(context, "witch description", 1, 1, "witch", Quality.Normal, "witch", 100, 100, 1);
        AddCard<TroopCard>(context, "wizard description", 1, 1, "wizard", Quality.Normal, "wizard", 100, 100, 1);
        AddCard<TroopCard>(context, "zap description", 1, 1, "zap", Quality.Normal, "zap", 100, 100, 1);
        AddCard<TroopCard>(context, "zapMachine description", 1, 1, "zapMachine", Quality.Normal, "zapMachine", 100, 100, 1);
        AddCard<TroopCard>(context, "zappies description", 1, 1, "zappies", Quality.Normal, "zappies", 100, 100, 1);
      }
  }
}
