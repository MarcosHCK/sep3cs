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
using DataClash.Framework.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DataClash.Application.IntegrationTests
{
  using static Testing;

  internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
      protected override void ConfigureWebHost (IWebHostBuilder builder)
        {
          builder.ConfigureAppConfiguration (configurationBuilder =>
            {
              var integrationConfig = new ConfigurationBuilder ()
                    .AddJsonFile ("appsettings.json")
                    .AddEnvironmentVariables ()
                    .Build ();

              configurationBuilder.AddConfiguration (integrationConfig);
            });

          builder.ConfigureServices ((builder, services) =>
            {
              DropAllSqlite (builder.Configuration.GetConnectionString ("DefaultConnection"))
                .Wait ();

              services
                .Remove<ICurrentUserService> ()
                .AddTransient (provider => Mock.Of<ICurrentUserService> (s =>
                        s.UserId == GetCurrentUserId ()));

              services
                .Remove<DbContextOptions<ApplicationDbContext>> ()
                .AddDbContext<ApplicationDbContext> ((sp, options) =>
                    options.UseSqlite (builder.Configuration.GetConnectionString ("DefaultConnection"),
                        builder => builder.MigrationsAssembly (typeof (ApplicationDbContext).Assembly.FullName)));
            });
        }
    }
}
