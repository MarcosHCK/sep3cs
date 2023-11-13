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
using DataClash.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ConfigureServices
    {
      public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
          services.AddAutoMapper (Assembly.GetExecutingAssembly ());
          services.AddValidatorsFromAssembly (Assembly.GetExecutingAssembly ());
          services.AddMediatR (cfg =>
            {
              cfg.RegisterServicesFromAssembly (Assembly.GetExecutingAssembly ());
              cfg.AddBehavior (typeof (IPipelineBehavior<,>), typeof (AuthorizationBehaviour<,>));
              cfg.AddBehavior (typeof (IPipelineBehavior<,>), typeof (UnhandledExceptionBehaviour<,>));
              cfg.AddBehavior (typeof (IPipelineBehavior<,>), typeof (ValidationBehaviour<,>));
            });
          return services;
        }
    }
}
