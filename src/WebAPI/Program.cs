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

namespace DataClash
{
  public class Program
    {
      public static async Task<int> Main (string[] args)
        {
          var builder = WebApplication.CreateBuilder (args);

          builder.Services.AddApplicationServices ();
          builder.Services.AddFrameworkServices (builder.Configuration);
          builder.Services.AddWebUIServices ();

          var app = builder.Build ();

          if (app.Environment.IsDevelopment () == false)
            {
              app.UseHsts ();

              using (var scope = app.Services.CreateScope ())
                {
                  await scope
                    .ServiceProvider
                    .GetRequiredService<IApplicationDbContextSeeder> ()
                    .SeedAsync ();
                }
            }
          else
            {
              app.UseDeveloperExceptionPage ();
              app.UseMigrationsEndPoint ();

              using (var scope = app.Services.CreateScope ())
                {
                  var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser> ();
                  var seeder = scope.ServiceProvider.GetRequiredService<IApplicationDbContextSeeder> ();

                  await initializer.InitialiseAsync ();
                  await initializer.SeedAsync ();
                  await seeder.SeedAsync ();
                }
            }

          app.UseHealthChecks ("/health");
          app.UseHttpsRedirection ();
          app.UseStaticFiles ();

          app.UseSwaggerUi3 (settings =>
            {
              settings.Path = "/api";
            });

          app.UseRouting ();

          app.UseAuthentication ();
          app.UseIdentityServer ();
          app.UseAuthorization ();

          app.MapControllerRoute (name : "default", pattern : "{controller}/{action=Index}/{id?}");
          app.MapRazorPages ();
          app.MapFallbackToFile ("index.html");
          app.Run ();
        return 0;
        }
    }
}
