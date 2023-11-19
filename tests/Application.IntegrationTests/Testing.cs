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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataClash.Application.IntegrationTests
{
  [SetUpFixture]
  public partial class Testing
    {
      private static WebApplicationFactory<Program> _factory = null!;
      private static IConfiguration _configuration = null!;
      private static IServiceScopeFactory _scopeFactory = null!;
      private static string? _currentUserId;

      [OneTimeSetUp]
      public void RunBeforeAnyTests ()
        {
          _factory = new CustomWebApplicationFactory ();
          _configuration = _factory.Services.GetRequiredService<IConfiguration> ();
          _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory> ();
          CleanupDb ();
        }

      internal static void CleanupDb ()
        {
          using (var db = new SqliteConnection (_configuration.GetConnectionString ("DefaultConnection")))
            {
              var tableNames = new List<string> ();
              db.Open ();

              using (SqliteCommand cmd = new SqliteCommand ("SELECT name FROM sqlite_master WHERE type='table';", db))
                {
                  using (SqliteDataReader reader = cmd.ExecuteReader ())
                    {
                      while (reader.Read ())
                        {
                          tableNames.Add (reader.GetString (0));
                        }
                    }
                }

              tableNames.Remove ("__EFMigrationsHistory");

              foreach (string tableName in tableNames)
                {
                  using (SqliteCommand cmd = new SqliteCommand ($"DELETE FROM {tableName};", db))
                    {
                      cmd.ExecuteNonQuery ();
                    }
                }
            }
        }

      public static async Task<TResponse> SendAsync<TResponse> (IRequest<TResponse> request)
        {
          using var scope = _scopeFactory.CreateScope ();
          var mediator = scope.ServiceProvider.GetRequiredService<ISender> ();
        return await mediator.Send (request);
        }

      public static async Task SendAsync (IBaseRequest request)
        {
          using var scope = _scopeFactory.CreateScope ();
          var mediator = scope.ServiceProvider.GetRequiredService<ISender> ();
          await mediator.Send (request);
        }

      public static string? GetCurrentUserId ()
        {
          return _currentUserId;
        }

      public static async Task<string> RunAsDefaultUserAsync ()
        {
          return await RunAsUserAsync ("test@local", "Testing1234!", Array.Empty<string>());
        }

      public static async Task<string> RunAsAdministratorAsync ()
        {
          return await RunAsUserAsync ("administrator@local", "Administrator1234!", new[] { "Administrator" });
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

      public static async Task ResetState ()
        {
          _currentUserId = null;
          CleanupDb ();
          await Task.CompletedTask;
        }

      public static async Task<TEntity?> FindAsync<TEntity> (params object[] keyValues)
            where TEntity : class
        {
          using var scope = _scopeFactory.CreateScope ();
          var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext> ();
        return await context.FindAsync<TEntity> (keyValues);
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

      [OneTimeTearDown]
      public void RunAfterAnyTests ()
        {
        }
    }
}
