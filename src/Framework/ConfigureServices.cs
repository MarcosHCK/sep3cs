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
using DataClash.Framework.Identity;
using DataClash.Framework.Persistence;
using DataClash.Framework.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ConfigureServices
    {
      public static IServiceCollection AddFrameworkServices (this IServiceCollection services, IConfiguration configuration)
        {
          if (configuration.GetValue<bool> ("UseInMemoryDatabase"))

            services.AddDbContext<ApplicationDbContext> (options => options.UseInMemoryDatabase ("DataClashDb"));
          else
            {
              services.AddDbContext<ApplicationDbContext> (options =>
                    options.UseSqlite (configuration.GetConnectionString ("DefaultConnection"),
                      builder => builder.MigrationsAssembly (typeof (ApplicationDbContext).Assembly.FullName)));
            }

          services.AddScoped<IApplicationDbContext> (provider => provider.GetRequiredService<ApplicationDbContext> ());
          services.AddScoped<ApplicationDbContextInitialiser> ();

          services
              .AddDefaultIdentity<ApplicationUser> (options =>
                {
                  options.Password.RequireDigit = false;
                  options.Password.RequiredLength = 8;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = false;
                  options.Lockout.AllowedForNewUsers = true;
                  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (10);
                  options.Lockout.MaxFailedAccessAttempts = 5;
                  options.User.RequireUniqueEmail = true;
                })
              .AddRoles<IdentityRole> ()
              .AddEntityFrameworkStores<ApplicationDbContext> ();

          services
              .AddIdentityServer ()
              .AddApiAuthorization<ApplicationUser, ApplicationDbContext> ()
              .AddProfileService<ProfileService> ();

          services.AddTransient<IDateTime, DateTimeService> ();
          services.AddTransient<IIdentityService, IdentityService> ();

          services
              .AddAuthentication ()
              .AddIdentityServerJwt ();

          services.AddAuthorization (options =>
              {
                options.AddPolicy ("CanPurge", policy => policy.RequireRole ("Administrator"));
              });
        return services;
        }
    }
}
