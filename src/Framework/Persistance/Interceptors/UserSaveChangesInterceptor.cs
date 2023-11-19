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
using DataClash.Domain.Entities;
using DataClash.Framework.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataClash.Framework.Persistence.Interceptors
{
  public class UserSaveChangesInterceptor : SaveChangesInterceptor
    {
      public UserSaveChangesInterceptor ()
        {
        }

      public override InterceptionResult<int> SavingChanges (DbContextEventData eventData, InterceptionResult<int> result)
        {
          UpdateEntities (eventData.Context);
          return base.SavingChanges (eventData, result);
        }

      public override ValueTask<InterceptionResult<int>> SavingChangesAsync (DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
          UpdateEntities (eventData.Context);
          return base.SavingChangesAsync (eventData, result, cancellationToken);
        }

      private static void UpdateEntities (DbContext? context)
        {
          if (context == null)
            return;

          var players = new List<Player> ();

          foreach (var entry in context.ChangeTracker.Entries<ApplicationUser> ())
            {
              if (entry.State != EntityState.Added)
                continue;

              var player = new Player
                {
                  Level = 1,
                  TotalCardsFound = 0,
                  TotalThrophies = 0,
                  TotalWins = 0
                };

              players.Add (player);
              entry.Entity.Player = player;
            }

          foreach (var player in players)
            {
              context.Add (player);
            }
        }
    }
}
