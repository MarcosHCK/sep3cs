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
import { HomeCarousel } from './HomeCarousel'
import React from 'react';

export function Home ()
{
  return (
  <div>
    <HomeCarousel items={[
      { caption : 'Administrator working', src : '/comic/administrator.jpeg', },
      { caption : 'Querying cards statistics', src : '/comic/query_card_stats.jpeg', },
      { caption : 'Querying challenges statistics', src : '/comic/query_challenge_stats.jpeg', },
      { caption : 'Querying clans statistics', src : '/comic/query_clan_stats.jpeg', },
      { caption : 'Querying wars statistics', src : '/comic/query_clan_war_stats.jpeg', },
      { caption : 'Querying matches statistics', src : '/comic/query_match_stats.jpeg', },
      { caption : 'User signing up for DataClash', src : '/comic/registration.jpeg', },
    ]} />
  </div>)
}
