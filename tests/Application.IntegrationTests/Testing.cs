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
using DataClash.Framework.Identity;
using DataClash.Framework.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests
{
  [SetUpFixture]
  public partial class Testing
    {
      private static WebApplicationFactory<Program> _factory = null!;
      private static IServiceScopeFactory _scopeFactory = null!;
      private static string? _currentUserId = null;

      [TestFixture]
      public abstract class BaseTestFixture
        {
          [SetUp]
          public async Task TestSetUp ()
            {
              /*
               * Respawn has no support for SQLite databases
               * and i have no patience to create a trasaction-based
               * checkpointing system, so recreate the application
               * for every test should do it.
               */
              _currentUserId = null;

              using (var scope = _scopeFactory.CreateScope ())
                {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
                  var facade = context.Database;

                  var script = "PRAGMA writable_schema = 1;"
                         + "delete from sqlite_master where type in ('table', 'index', 'trigger');"
                         + "PRAGMA writable_schema = 0; VACUUM;";
                  await facade.ExecuteSqlRawAsync (script);
                  await facade.MigrateAsync ();
                }
            }
        }

      [OneTimeSetUp]
      public void RunBeforeAnyTests ()
        {
          _factory = new CustomWebApplicationFactory ();
          _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory> ();
        }

      [OneTimeTearDown]
      public void RunAfterAnyTests ()
        {
        }

      public static async Task AddAsync<TEntity> (TEntity entity)
            where TEntity : class
        {
          using var scope = _scopeFactory.CreateScope ();
          var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
            context.Add (entity);
          await context.SaveChangesAsync ();
        }

      public static async Task<int> CountAsync<TEntity> ()
            where TEntity : class
        {
          using var scope = _scopeFactory.CreateScope ();
          var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
        return await context.Set<TEntity> ().CountAsync ();
        }

      public static async Task DropAllSqlite (string? target)
        {
          using (var connection = new SqliteConnection (target))
            {
              var script = "PRAGMA writable_schema = 1;"
                         + "delete from sqlite_master where type in ('table', 'index', 'trigger');"
                         + "PRAGMA writable_schema = 0; VACUUM;";
              var command = new SqliteCommand (script, connection);

              connection.Open ();
              command.ExecuteNonQuery ();
            }

          await Task.CompletedTask;
        }

      public static async Task<TEntity?> FindAsync<TEntity> (params object[] keyValues)
            where TEntity : class
        {
          using var scope = _scopeFactory.CreateScope ();
          var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
        return await context.FindAsync<TEntity> (keyValues);
        }

      public static string? GetCurrentUserId ()
        {
          return _currentUserId;
        }

      public static async Task<long> GetPlayerIdForUser (string userId)
        {
          using (var scope = _scopeFactory.CreateScope ())
            {
              var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
              var playerId = await context.Users.FindAsync (userId);
              return playerId == null
                ? -1 : (playerId.PlayerId.HasValue == false
                ? -1 : playerId.PlayerId.Value);
            }
        }

      public static async Task<string> RunAsAdministratorAsync ()
        {
          return await RunAsUserAsync ("administrator@local", "Administrator1234!", new[] { "Administrator" });
        }

      public static async Task<string> RunAsDefaultUserAsync ()
        {
          return await RunAsUserAsync ("test@local", "Testing1234!", Array.Empty<string>());
        }

      public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
          using var scope = _scopeFactory.CreateScope();

          var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>> ();
          var user = new ApplicationUser { UserName = userName, Email = userName };
          var result = await userManager.CreateAsync (user, password);

          if (roles.Any ())
            {
              var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>> ();

              foreach (var role in roles)
                {
                  await roleManager.CreateAsync (new IdentityRole (role));
                }

              await userManager.AddToRolesAsync (user, roles);
            }

          if (result.Succeeded)
            {
              _currentUserId = user.Id;
              return _currentUserId;
            }

          var errors = string.Join (Environment.NewLine, result.ToApplicationResult ().Errors);
          throw new Exception ($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

      public static async Task SendAsync (IBaseRequest request)
        {
          using var scope = _scopeFactory.CreateScope ();
          var mediator = scope.ServiceProvider.GetRequiredService<ISender> ();
          await mediator.Send (request);
        }

      public static async Task<TResponse> SendAsync<TResponse> (IRequest<TResponse> request)
        {
          using var scope = _scopeFactory.CreateScope ();
          var mediator = scope.ServiceProvider.GetRequiredService<ISender> ();
        return await mediator.Send (request);
        }
    }
}
