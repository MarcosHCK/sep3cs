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

namespace Microsoft.Extensions.DependencyInjection
{
  internal class ApplicationDbContextSeeder : IApplicationDbContextSeeder
    {
      private readonly IApplicationDbContext _context;
      private readonly ApplicationDbContextSeederConfiguration _configuration;

      public ApplicationDbContextSeeder (
          IApplicationDbContext context,
          ApplicationDbContextSeederConfiguration configuration)
        {
          _context = context;
          _configuration = configuration;
        }

      public async Task SeedAsync ()
        {
          foreach (var providerType in _configuration.SeederProviders)
            {
              var instance = Activator.CreateInstance (providerType);
              var provider = (IApplicationDbContextSeederProvider) instance!;

              if (instance == null)
                throw new TypeInitializationException (providerType.FullName, null);
              else
                provider.SeedAsync (_context);
            }

          await _context.SaveChangesAsync (CancellationToken.None);
        }
    }

  public class ApplicationDbContextSeederConfiguration
    {
      private readonly List<Type> _seederProviders = new ();
      public IReadOnlyCollection<Type> SeederProviders => _seederProviders.AsReadOnly ();

      public void AddProvider<T> ()
          where T : IApplicationDbContextSeederProvider 
        {
          _seederProviders.Add (typeof (T));
        }
    }

  public static class ServiceCollectionExtensions
    {
      public static IServiceCollection AddApplicationDbContextSeeder (this IServiceCollection services, Action<ApplicationDbContextSeederConfiguration> configurator)
        {
          var configuration = new ApplicationDbContextSeederConfiguration ();
            configurator (configuration);
          return services.AddTransient<IApplicationDbContextSeeder> (provider =>
            {
              var context = provider.GetRequiredService <IApplicationDbContext> ();
              var instance = new ApplicationDbContextSeeder (context, configuration);
              return instance;
            });
        }
    }
}
