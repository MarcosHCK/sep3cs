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
using DataClash.Domain.ValueObjects;

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
        
        AddCard<TroopCard>(context, "Spawns a pair of leveled up Barbarians. They're like regular Barbarians, only harder, better, faster and stronger.", 6, 1, "angry barbarian", Quality.Normal, "angry_barbarian", 150, 524, 2);
        AddCard<TroopCard>(context, "A pair of lightly armored ranged attackers. They'll help you take down ground and air units, but you're on your own with hair coloring advice.", 3, 1, "archer", Quality.Normal, "archer", 42, 119, 2);
        AddCard<TroopCard>(context, "She is fast, deadly and hard to catch. Beware of her crossbow bolts and try not to blink - you might miss her!", 5, 11, "archerqueen", Quality.Champion, "archerqueen", 225, 1000, 1);
        AddCard<TroopCard>(context, "Burps fireballs from the sky that deal area damage. Baby dragons hatch cute, hungry and ready for a barbeque.", 4, 1, "baby dragon", Quality.Normal, "baby_dragon", 100, 720, 1);
        AddCard<TroopCard>(context, "The Bandit dashes to her target and delivers an extra big hit! While dashing, she can't be touched. The mask keeps her identity safe, and gives her bonus cool points!", 3, 9, "bandit", Quality.Legendary, "bandit", 160, 750, 1);
        AddCard<TroopCard>(context, "A horde of melee attackers with mean mustaches and even meaner tempers", 5, 1, "barbarian_card", Quality.Normal, "barbarian_card", 75, 262, 5);
        AddCard<StructCard>(context, "Building that periodically spawns Barbarians to fight the enemy. Time to make the Barbarians!", 7, 3, "barbarian_hut", Quality.Normal, "barbarian_hut", 550, 75, 57);
        AddCard<MagicCard>(context, "It rolls over and damages anything in its path, then breaks open and out pops a Barbarian! How did he get inside?!", 2, 6, "barbarian barrel", Quality.Epic, "barb_barrel",2.5, 100, 100, new TimeSpan(0,0,2));
        AddCard<TroopCard>(context, "Spawns a handful of tiny flying creatures. Think of them as sweet, purple... balls of DESTRUCTION!", 2, 1, "bats", Quality.Normal, "bats", 32, 32, 5);
        AddCard<TroopCard>(context, "With each attack, she unleashes a powerful healing aura that restores Hitpoints to herself and friendly Troops. When she isn't attacking, she passively heals herself!", 4, 3, "battle healer", Quality.Rare, "battle_healer", 70, 810, 1);
        AddCard<TroopCard>(context, "Two Barbarians holding a big log charge at the nearest building, dealing significant damage if they connect; then they go to town with their swords!", 4, 3, "battle ram", Quality.Rare, "battle_ram", 135, 456, 2);
        AddCard<TroopCard>(context, "Runs fast, shoots far and chews gum. How does he blow darts with a mouthful of Double Trouble Gum? Years of didgeridoo lessons.", 3, 3, "blowdart goblin", Quality.Rare, "blowdart_goblin", 62, 123, 1);
        AddCard<TroopCard>(context, "Small, lightly protected skeleton who throws bombs. Deals area damage that can wipe out a swarm of enemies.", 2, 1, "bomber", Quality.Normal, "bomber", 87, 130, 1);
        AddCard<StructCard>(context, "Defensive building that houses a Bomber. Deals area damage to anything dumb enough to stand near it.", 4, 3, "bomb_tower", Quality.Rare, "bomb_tower", 640, 105, 58);
        AddCard<StructCard>(context, "This big blue dude digs the simple things in life - Dark Elixir drinks and throwing rocks. His massive boulders roll through their target, hitting everything behind for a strike!", 5, 6, "bowler", Quality.Epic, "bowler", 180, 1300, 1);
        AddCard<StructCard>(context, "You gotta spend Elixir to make Elixir! This building makes 8 Elixir over its Lifetime. Does not appear in your starting hand.", 1, 1, "building_elixir_collector", Quality.Rare, "building_elixir_collector", 505, 1, 1);
        AddCard<StructCard>(context, "Defensive building, roasts targets for damage that increases over time. Burns through even the biggest and toughest enemies!", 5, 3, "building inferno", Quality.Rare, "building_inferno", 825, 300, 550);
        AddCard<StructCard>(context, "Defensive building with a long range. Shoots big boulders that deal area damage, but cannot hit targets that get too close!", 4, 1, "building mortar", Quality.Normal, "building_mortar", 535, 104, 20);
        AddCard<StructCard>(context, "Defensive building. Whenever it's not zapping the enemy, the power of Electrickery is best kept grounded.", 4, 1, "building tesla", Quality.Normal, "building_tesla", 450, 90, 81);
        AddCard<TroopCard>(context, "Nice tower you got there. Would be a shame if this X-Bow whittled it down from this side of the Arena...", 6, 6, "building xbow", Quality.Epic, "building_xbow", 1000, 26, 86);
        AddCard<TroopCard>(context, "A Cannon on wheels?! Bet they won't see that coming! Once you break its shield, it becomes a Cannon not on wheels.", 5, 6, "cannon_cart", Quality.Epic, "cannon_cart", 133, 558, 1);
        AddCard<StructCard>(context, "Defensive building. Shoots cannonballs with deadly effect, but cannot target flying troops", 3, 1, "chaos cannon", Quality.Normal, "chaos_cannon", 322, 83, 92);
        AddCard<TroopCard>(context, "Slow but durable, only attacks buildings. When destroyed, explosively splits into two Golemites and deals area damage!", 8, 1, "Golem", Quality.Epic, "chr_golem", 195, 3200, 1);
        AddCard<MagicCard>(context, "Duplicates all friendly troops in the target area. Cloned troops are fragile, but pack the same punch as the original! Doesn't affect buildings.", 3, 6, "clone", Quality.Epic, "copy",3, 100, 100,new TimeSpan (0,0,1));
        AddCard<TroopCard>(context, "The Dark Prince deals area damage and lets his spiked club do the talking for him - because when he does talk, it sounds like he has a bucket on his head.", 4, 1, "dark prince", Quality.Epic, "dark_prince", 155, 750, 1);
        AddCard<MagicCard>(context, "Deals Damage per second to Troops and Crown Towers. Deals huge Building Damage! Does not affect flying units (it is an EARTHquake, after all).", 3, 3, "earth", Quality.Rare, "earthquake",3.5, 100, 100, new TimeSpan(0,0,3));
        AddCard<StructCard>(context, "He channels electricity through his Zap Pack, a unique device that stuns and damages any Troop attacking him within its range. Don't tell him that his finger guns aren't real! He'll zap you.", 7, 6, "electro giant", Quality.Epic, "electrogiant", 102, 2410, 1);
        AddCard<TroopCard>(context, "Jumps on enemies, dealing Area Damage and stunning up to 9 enemy Troops. Locked in an eternal battle with Knight for the best mustache.", 1, 1, "electro spirit", Quality.Normal, "electrospirit", 39, 90, 1);
        AddCard<TroopCard>(context, "Spits out bolts of electricity hitting up to three targets. Suffers from middle child syndrome to boot.", 5, 6, "electro dragon", Quality.Epic, "electro_dragon", 120,594, 1);
        AddCard<TroopCard>(context, "He lands with a POW!, stuns nearby enemies and shoots lightning with both hands! What a show off.", 4, 9, "electro wizard", Quality.Legendary, "electro_wizard", 159, 590, 1);
        AddCard<TroopCard>(context, "Splits into two Elixir Golemites when destroyed, which split into two sentient Blobs when defeated. Each part of the Elixir Golem gives your opponent some Elixir when destroyed!", 3, 3, "elixir golem", Quality.Rare, "elixir_golem", 120, 740, 1);
        AddCard<TroopCard>(context, "He throws his axe like a boomerang, striking all enemies on the way out AND back. It's a miracle he doesn't lose an arm.", 5, 6, "executioner", Quality.Epic, "executioner", 212, 800, 1);
        AddCard<TroopCard>(context, "Shoots a firework that explodes upon impact, damaging the target and showering anything directly behind it with sparks. This is what happens when Archers get bored!", 3, 1, "firecracker", Quality.Normal, "firecracker", 125, 119, 1);
        AddCard<TroopCard>(context, "The Furnace spawns one Fire Spirit at a time. It also makes great brick-oven pancakes.", 4, 3, "firespirit hut", Quality.Normal, "firespirit_hut", 400, 90, 81);
        AddCard<MagicCard>(context, "Annnnnd... Fireball. Incinerates a small area, dealing high damage. Reduced damage to Crown Towers.", 4, 3, "fire fireball", Quality.Rare, "fire_fireball", 2.5, 325, 97,new TimeSpan(0,0,10));
        AddCard<TroopCard>(context, "Building that spawns Spear Goblins. Don't look inside... you don't want to see how they're made.", 5, 3, "goblin hut", Quality.Rare, "fire_furnace", 400, 32, 18);
        AddCard<TroopCard>(context, "The Fire Spirit is on a kamikaze mission to give you a warm hug. It'd be adorable if it wasn't on fire.", 1, 1, "fire spirits", Quality.Normal, "fire_spirits", 81, 90, 1);
        AddCard<TroopCard>(context, "His Ranged Attack can pull enemies towards him, and pull himself to enemy buildings. He's also mastered the ancient art of 'Fish Slapping'.", 3, 9, "fisherman", Quality.Legendary, "fisherman", 160, 720, 1);
        AddCard<TroopCard>(context, "The Master Builder has sent his first contraption to the Arena! It's a fast and fun flying machine, but fragile!",4, 3, "flying machine", Quality.Rare, "flying_machine", 81, 290, 1);
        AddCard<MagicCard>(context, "Freezes and damages enemy troops and buildings, making them unable to move or attack. Everybody chill. Reduced damage to Crown Towers.", 4, 6, "freeze", Quality.Epic, "freeze", 3, 72, 21,new TimeSpan(0,0,4));
        AddCard<TroopCard>(context, "He drifts invisibly through the Arena until he's startled by an enemy... then he attacks! Then he's invisible again! Zzzz.", 3, 9, "ghost", Quality.Legendary, "ghost", 216, 1000, 1);
        AddCard<TroopCard>(context, "Slow but durable, only attacks buildings. A real one-man wrecking crew!", 5, 1, "giant", Quality.Rare, "giant", 120, 1930, 1);
        AddCard<TroopCard>(context, "The bigger the skeleton, the bigger the bomb. Carries a bomb that blows up when the Giant Skeleton dies.", 6, 1, "giant skeleton", Quality.Epic, "giant_skeleton", 167, 2140, 1);
        AddCard<TroopCard>(context, "Building capable of burrowing underground and appearing anywhere in the Arena. Spawns Goblins one at a time until destroyed. Then spawns a few more, to make sure everything nearby has been properly stabbed.", 4, 6, "goblindrill", Quality.Rare, "goblindrill", 1000, 1, 1);
        AddCard<TroopCard>(context, " Four fast, unarmored melee attackers. Small, fast, green and mean!", 2, 1, "goblins", Quality.Normal, "goblins", 47, 79, 4);
        AddCard<TroopCard>(context, "Three unarmored ranged attackers. Who the heck taught these guys to throw spears!? Who thought that was a good idea?!", 2, 1, "goblin archer", Quality.Normal, "goblin_archer", 32, 52, 3);
        AddCard<MagicCard>(context, "Spawns three Goblins anywhere in the Arena. It's going to be a thrilling ride, boys!", 13, 1, "goblin barrel", Quality.Normal, "goblin_barrel", 100, 100, 100,new TimeSpan(0,0,5));
        AddCard<TroopCard>(context, "When the Goblin Cage is destroyed, a Goblin Brawler is unleashed into the Arena! Goblin Brawler always skips leg day.", 4, 3, "goblin cage", Quality.Rare, "goblin_cage", 350, 159, 144);
        AddCard<TroopCard>(context, "Spawns six Goblins - three with knives, three with spears - at a discounted Elixir cost. It's like a Goblin Value Pack!", 3, 1, "goblin_gang", Quality.Normal, "goblin_gang", 47, 79, 3);
        AddCard<TroopCard>(context, "This jolly green Goblin Giant stomps towards enemy buildings. He carries two Spear Goblins everywhere he goes. It's a weird but functional arrangement.", 6, 6, "goblin giant", Quality.Epic, "goblin_giant", 110, 2085, 3);
        AddCard<TroopCard>(context, "A warrior with luxurious hair and outstanding flexibility. Demonstrates his aerobics skills on demand.", 4,11, "goldenknight", Quality.Champion, "goldenknight", 160, 1800, 1);
        AddCard<MagicCard>(context, "Surprise! It's a party. A Skeleton party, anywhere in the Arena. Yay!", 5, 9, "graveyard", Quality.Legendary, "graveyard", 4,100, 100, new TimeSpan(0,0,9));
        AddCard<MagicCard>(context, "A mischievous Spirit that leaps at enemies, dealing Damage and leaving behind a powerful healing effect that restores Hitpoints to friendly Troops! R.I.P. Heal 2017 - 2020 Alas, we hardly used ye.", 1, 1, "healspirit", Quality.Normal, "healspirit", 100, 100, 1,new TimeSpan(0,0,3));
        AddCard<TroopCard>(context, "Terry and his trusty companion Pim Pim bring their unconventional Hog Riding skills to the Arena!", 4, 11, "hogrider champion", Quality.Legendary, "hogrider_champion", 350, 2300, 1);
        AddCard<TroopCard>(context, "Fast melee troop that targets buildings and can jump over the river. He followed the echoing call of Hog Riderrrrr all the way through the Arena doors.", 4, 1, "hog rider", Quality.Rare, "hog_rider", 150, 800, 1);
        AddCard<TroopCard>(context, "He deals BIG damage up close - not so much at range. What he lacks in accuracy, he makes up for with his impressively bushy eyebrows.", 4, 6, "hunter", Quality.Epic, "hunter", 53, 524, 1);
        AddCard<TroopCard>(context, "He's tough, targets buildings and explodes when destroyed, slowing nearby enemies. Made entirely out of ice... or is he?! Yes.", 3, 3, "icegolem", Quality.Rare, "icegolem", 40, 565, 1);
        AddCard<TroopCard>(context, "This chill caster throws ice shards that slow down enemies' movement and attack speed. Despite being freezing cold, he has a handlebar mustache that's too hot for TV.", 3, 1, "ice wizard", Quality.Legendary, "ice_wizard", 75, 569, 1);
        AddCard<TroopCard>(context, "Shoots a focused beam of fire that increases in damage over time. Wears a helmet because flying can be dangerous", 4, 9, "inferno dragon", Quality.Legendary, "inferno_dragon", 170, 1070, 1);
        AddCard<TroopCard>(context, "A tough melee fighter. The Barbarian's handsome, cultured cousin. Rumor has it that he was knighted based on the sheer awesomeness of his mustache alone.", 1, 1, "knight", Quality.Normal, "knight", 79, 690, 1);
        AddCard<TroopCard>(context, "The Lava Hound is a majestic flying beast that attacks buildings. The Lava Pups are less majestic angry babies that attack anything.", 7, 9, "lava hound", Quality.Legendary, "lava_hound", 45, 2960, 1);
        AddCard<MagicCard>(context, "Bolts of lightning damage and stun up to three enemy troops or buildings with the most hitpoints in the target area. Reduced damage to Crown Towers.", 6, 6, "lightning", Quality.Epic, "lightning", 3.5, 100, 100,new TimeSpan(0,0,2));
        AddCard<TroopCard>(context, "Who let their nephew into the Arena?! This mischievous Royal gains Hit Speed when firing, as long as he stands still! He may look weak, but he's always being watched over from afar...", 3, 11, "little prince", Quality.Champion, "little_prince", 110, 700, 1);
        AddCard<TroopCard>(context, "Not quite a Wizard, nor an Archer - he shoots a magic arrow that passes through and damages all enemies in its path. It's not a trick, it's magic!", 4, 9, "magic_archer", Quality.Legendary, "magic_archer", 111,440, 1);
        AddCard<TroopCard>(context, "He lands with the force of 1,000 mustaches, then jumps from one foe to the next dealing huge area damage. Stand aside!", 7, 9, "mega knight", Quality.Legendary, "mega_knight", 222, 3300, 1);
        AddCard<TroopCard>(context, "Flying, armored and powerful. What could be its weakness?! Cupcakes.", 3, 3, "mega minion", Quality.Rare, "mega_minion", 147, 395, 1);
        AddCard<TroopCard>(context, "Walk softly ... and carry a big drill! This Champion deals increasing Damage to his target and can switch lanes to escape combat or change attack plans. This makes him not only the mightiest, but also the sneakiest Miner in the Arena.", 4, 11, "mightyminer", Quality.Champion, "mightyminer", 400,2250, 1);
        AddCard<TroopCard>(context, "The Miner can burrow his way underground and appear anywhere in the Arena. It's not magic, it's a shovel. A shovel that deals reduced damage to Crown Towers", 3, 9, "miner", Quality.Legendary, "miner", 160, 1000, 1);
        AddCard<TroopCard>(context, "Three fast, unarmored flying attackers. Roses are red, minions are blue, they can fly, and will crush you!", 1, 1, "minion", Quality.Normal, "minion", 46, 90, 3);
        AddCard<TroopCard>(context, "Six fast, unarmored flying attackers. Three's a crowd, six is a horde!", 5, 1, "minion horde", Quality.Normal, "minion_horde", 46, 90, 6);
        AddCard<TroopCard>(context, "The Arena is a certified butterfly-free zone. No distractions for P.E.K.K.A, only destruction", 4, 1, "minipekka", Quality.Rare, "minipekka", 340, 642, 1);
        AddCard<MagicCard>(context, "Mirrors your last card played for +1 Elixir. Does not appear in your starting hand.", 1, 1, "mirror", Quality.Normal, "mirror", 100, 100, 100,new TimeSpan(0,0,3));
        AddCard<TroopCard>(context, "Monk has spent many isolated years perfecting a new style of combat. He fires off a 3-hit combo, with the last blow dealing extra Damage and pushing enemies back!", 5, 11, "monk", Quality.Champion, "monk", 280, 2000, 1);
        AddCard<TroopCard>(context, "Places a curse on enemy Troops with each attack. When a cursed Troop is destroyed, it turns into a building-targeting Hog that fights alongside the Mother Witch! She also bakes great cookies.", 4, 9, "motherwitch", Quality.Legendary, "motherwitch", 121, 484, 1);
        AddCard<TroopCard>(context, "Don't be fooled by her delicately coiffed hair, the Musketeer is a mean shot with her trusty boomstick.", 4, 1, "musketeer", Quality.Rare, "musketeer", 103, 340, 1);
        AddCard<TroopCard>(context, "Summons Bats to do her bidding! If you get too close, she's not afraid of pitching in with her mean-looking battle staff.", 4, 9, "nightwitch", Quality.Legendary, "nightwitch", 260, 750, 1);
        AddCard<MagicCard>(context, "Arrows pepper a large area, damaging all enemies hit. Reduced damage to Crown Towers.", 3, 1, "arrows", Quality.Normal, "order_volley", 1.4, 48, 14,new TimeSpan(0,0,5));
        AddCard<TroopCard>(context, "Building that spawns Spear Goblins. Don't look inside... you don't want to see how they're made.", 5, 9, "party hut", Quality.Legendary, "party_hut", 704, 109, 155);
        AddCard<MagicCard>(context, "Deals high damage to a small area. Looks really awesome doing it. Reduced damage to Crown Towers.", 5, 1, "party rocket", Quality.Normal, "party_rocket", 100, 100, 1,new TimeSpan(0,0,1));
        AddCard<TroopCard>(context, "A heavily armored, slow melee fighter. Swings from the hip, but packs a huge punch.", 7, 1, "pekka", Quality.Uncommon, "pekka", 510,2350, 1);
        AddCard<TroopCard>(context, "This mystical creature will be reborn as an egg when destroyed. If it hatches, it returns to the fight! What an egg-cellent ability. Being born again is tiring, so a hatched Phoenix will have slightly less Hitpoints and Damage (80% of its total).", 4, 9, "phoenix", Quality.Legendary, "phoenix", 180, 870, 1);
        AddCard<MagicCard>(context, "Covers the area in a deadly toxin, damaging enemy troops and buildings over time. Yet somehow leaves the grass green and healthy. Go figure! Reduced damage to Crown Towers.", 4, 6, "poison", Quality.Epic, "poison", 3.5,100, 100,new TimeSpan(0,0,8));
        AddCard<TroopCard>(context, "Don't let the little pony fool you. Once the Prince gets a running start, you WILL be trampled. Deals double damage once he gets charging.", 1, 1, "prince", Quality.Epic, "prince", 245, 1200, 1);
        AddCard<TroopCard>(context, "This stunning Princess shoots flaming arrows from long range. If you're feeling warm feelings towards her, it's probably because you're on fire.", 3, 1, "princess", Quality.Legendary, "princess", 140, 216, 1);
        AddCard<MagicCard>(context, "Increases troop movement and attack speed. Buildings attack faster and summon troops quicker, too. Chaaaarge!", 2, 1, "rage", Quality.Normal, "rage", 100, 100, 100,new TimeSpan(0,0,10));
        AddCard<TroopCard>(context, "He chops trees by day and hunts The Log by night. His bottle of Rage spills everywhere when he's defeated.", 4, 9, "Lumber Jack", Quality.Legendary, "rage_barbarian", 200, 1060, 1);
        AddCard<TroopCard>(context, "Together they charge through the Arena; snaring enemies, knocking down towers ... and chewing grass!?", 5, 9, "ram rider", Quality.Legendary, "ram_rider", 220, 1461, 1);
        AddCard<TroopCard>(context, "Spawns a mischievous trio of Rascals! The boy takes the lead, while the girls pelt enemies from behind... with slingshots full of Double Trouble Gum!", 5, 1, "rascals", Quality.Normal, "rascals", 52, 758, 3);
        AddCard<MagicCard>(context, "Deals high damage to a small area. Looks really awesome doing it. Reduced damage to Crown Towers.", 6, 3, "rocket", Quality.Rare, "rocket", 2.0, 700, 175,new TimeSpan(0,0,3));
        AddCard<TroopCard>(context, "Destroying enemy buildings with his massive cannon is his job; making a raggedy blond beard look good is his passion.", 6, 1, "royalgiant", Quality.Normal, "royalgiant", 120, 1200, 1);
        AddCard<MagicCard>(context, "No need to sign for this package! Dropped from the sky, it deals Area Damage to enemy Troops before delivering a Royal Recruit. The empty box is also handy for espionage.", 3, 1, "royal_delivery", Quality.Normal, "royal_delivery", 2,100, 100,new TimeSpan(0,0,2));
        AddCard<TroopCard>(context, "The King's personal pets are loose! They love to chomp on apples and towers alike - who let the hogs out?!", 5, 3, "royal_hog", Quality.Rare, "royal_hog", 35, 395,4);
        AddCard<TroopCard>(context, "Deploys a line of recruits armed with spears, shields and wooden buckets. They dream of ponies and one day wearing metal buckets.", 7, 1, "royal recruits", Quality.Normal, "royal_recruits", 52, 208, 6);
        AddCard<TroopCard>(context, "This pair of skeletal scorchers deal Area Damage and fly above the Arena. They also play a mean rib cage xylophone duet.", 4, 1, "skeletondragon", Quality.Normal, "skeletondragon", 63, 220, 2);
        AddCard<TroopCard>(context, "The King of the undead himself. He sometimes feels lonely (could be due to his non flattering features) and will summon friends to join him in the battle even after death. Tough guys have feelings too!", 4, 11, "skeletonking", Quality.Champion, "skeletonking", 205, 2300, 1);
        AddCard<TroopCard>(context, "Three fast, very weak melee fighters. Surround your enemies with this pile of bones!", 1, 1, "skeletons", Quality.Normal, "skeletons_card", 32, 32, 3);
        AddCard<TroopCard>(context, "As pretty as they are, you won't want a parade of THESE balloons showing up on the horizon. Drops powerful bombs and when shot down, crashes dealing area damage.", 1, 1, "skeleton_balloon", Quality.Normal, "skeleton_balloon", 400, 1050, 1);
        AddCard<TroopCard>(context, "Spawns an army of Skeletons. Meet Larry and his friends Harry, Gerry, Terry, Mary, etc.", 3, 1, "skeleton army", Quality.Epic, "skeleton_horde", 51, 51, 15);
        AddCard<TroopCard>(context, "Three ruthless bone brothers with shields. Knock off their shields and all that's left are three ruthless bone brothers.", 3, 1, "skeleton warriors", Quality.Epic, "skeleton_warriors", 76, 51, 3);
        AddCard<MagicCard>(context, "It's HUGE! Once it began rolling down Frozen Peak, there was no stopping it. Enemies hit are knocked back and slowed down. Reduced damage to Crown Towers.", 2, 1, "snowball", Quality.Normal, "snowball",2.5, 100, 100,new TimeSpan(0,0,1) );
        AddCard<TroopCard>(context, "Spawns one lively little Ice Spirit to freeze a group of enemies. Stay frosty.", 1, 1, "ice spirit", Quality.Normal, "snow_spirits", 43, 90, 1);
        AddCard<TroopCard>(context, "Sparky slowly charges up, then unloads MASSIVE area damage. Overkill isn't in her vocabulary.", 6, 9, "sparky", Quality.Legendary, "zapMachine", 1100, 1200, 1);
        AddCard<TroopCard>(context, "A pair of lightly armored ranged attackers. They'll help you take down ground and air units, but you're on your own with hair coloring advice.", 3, 9, "superarcher", Quality.Legendary, "superarcher", 100, 580, 2);
        AddCard<TroopCard>(context, "Fast melee troop that targets buildings and can jump over the river. He followed the echoing call of Hog Riderrrrr all the way through the Arena doors.", 5, 9, "Santa hog rider", Quality.Legendary, "superhogrider", 265, 1400, 1);
        AddCard<TroopCard>(context, "He's tough, targets buildings and explodes when destroyed, slowing nearby enemies. Made entirely out of ice... or is he?! Yes.", 4, 9, "super ice golem", Quality.Legendary, "supericegolem", 100, 3000, 1);
        AddCard<TroopCard>(context, "Not quite a Wizard, nor an Archer - he shoots a magic arrow that passes through and damages all enemies in its path. It's not a trick, it's magic!", 5, 9, "supermagicarcher", Quality.Legendary, "supermagicarcher", 250, 580, 1);
        AddCard<TroopCard>(context, "The Arena is a certified butterfly-free zone. No distractions for P.E.K.K.A, only destruction.", 5, 9, "superminipekka", Quality.Legendary, "superminipekka", 850, 1333, 1);
        AddCard<TroopCard>(context, "Summons Skeletons, shoots destructo beams, has glowing pink eyes that unfortunately don't shoot lasers.", 6, 9, "superwitch", Quality.Legendary, "superwitch", 110, 880, 1);
        AddCard<TroopCard>(context, "The Lava Hound is a majestic flying beast that attacks buildings. The Lava Pups are less majestic angry babies that attack anything.", 8, 9, "super lava hound", Quality.Legendary, "super_lava_hound", 45, 2800, 1);
        AddCard<MagicCard>(context, "A spilt bottle of Rage turned an innocent tree trunk into The Log. Now, it seeks revenge by crushing anything in its path! Reduced damage to Crown Towers.", 2, 9, "the log", Quality.Legendary, "the_log", 3,240 , 48, new TimeSpan(0,0,4));
        AddCard<TroopCard>(context, "Trio of powerful, independent markswomen, fighting for justice and honor. Disrespecting them would not be just a mistake, it would be a cardinal sin!", 9, 1, "three musketeers", Quality.Rare, "three_musketeers", 103, 340, 3);
        AddCard<TroopCard>(context, "Building that periodically spawns Skeletons to fight the enemy... and when destroyed, spawns 4 more Skeletons! Creepy!", 3, 3, "tombstone", Quality.Rare, "tombstone", 250, 250, 1);
        AddCard<MagicCard>(context, "Drags enemy troops to its center while dealing damage over time, just like a magnet. A big, swirling, Tornado-y magnet.", 3, 6, "tornado", Quality.Epic, "tornado",5.5, 100, 100, new TimeSpan(0,0,1));
        AddCard<TroopCard>(context, "Tough melee fighter, deals area damage around her. Swarm or horde, no problem! She can take them all out with a few spins.", 4, 1, "valkyrie", Quality.Rare, "valkyrie", 126, 900, 1);
        AddCard<TroopCard>(context, "A daring duo of dangerous dive bombers. Nothing warms a Wall Breaker's cold and undead heart like blowing up buildings.", 2, 6, "wallbreaker", Quality.Normal, "wallbreaker", 245, 207, 2);
        AddCard<TroopCard>(context, "Summons Skeletons, shoots destructo beams, has glowing pink eyes that unfortunately don't shoot lasers.", 1, 1, "witch", Quality.Epic, "witch", 84, 524, 1);
        AddCard<TroopCard>(context, "The most awesome man to ever set foot in the Arena, the Wizard will blow you away with his handsomeness... and/or fireballs", 5, 1, "wizard", Quality.Rare, "wizard", 133, 340, 1);
        AddCard<MagicCard>(context, "Zaps enemies, briefly stunning them and dealing damage inside a small radius. Reduced damage to Crown Towers.", 2, 1, "zap", Quality.Normal, "zap",2.5, 75, 22, new TimeSpan(0,0,1));

        AddCard<TroopCard>(context, "Spawns a pack of miniature Zap machines. Who controls them...? Only the Master Builder knows.", 4, 3, "zappies", Quality.Rare, "zappies", 55, 250, 3);
      }
  }
}
