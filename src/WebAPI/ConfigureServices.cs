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
using DataClash.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
  public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
      services.AddDatabaseDeveloperPageExceptionFilter ();
      services.AddScoped<ICurrentUserService, CurrentUserService> ();
      services.AddHttpContextAccessor ();
      services.AddHealthChecks ().AddDbContextCheck<ApplicationDbContext> ();
      services.AddControllersWithViews ();
      services.AddRazorPages ();
      services.AddScoped<FluentValidationSchemaProcessor> (provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>> ();
            var loggerFactory = provider.GetService<ILoggerFactory> ();
          return new FluentValidationSchemaProcessor (provider, validationRules, loggerFactory);
        });

      services.Configure<ApiBehaviorOptions> (options => options.SuppressModelStateInvalidFilter = true);
      services.AddOpenApiDocument ((configure, serviceProvider) =>
        {
          var fluentValidationSchemaProcessor = serviceProvider.CreateScope ().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor> ();

          configure.SchemaProcessors.Add (fluentValidationSchemaProcessor);
          configure.Title = "DataClash API";

          configure.AddSecurity ("JWT", Enumerable.Empty<string> (), new OpenApiSecurityScheme
            {
              Description = "Type into the textbox: Bearer {your JWT token}.",
              In = OpenApiSecurityApiKeyLocation.Header,
              Name = "Authorization",
              Type = OpenApiSecuritySchemeType.ApiKey,
            });

          configure.OperationProcessors.Add (new AspNetCoreOperationSecurityScopeProcessor ("JWT"));
        });
      return services;
    }
}
