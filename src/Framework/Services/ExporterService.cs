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
using DataClash.Application.Common.Exceptions;
using DataClash.Application.Common.Interfaces;
using System.Reflection;

namespace DataClash.Framework.Services
{
  public class ExporterService<T> : IExporterService<T>
    {
      private readonly IEnumerable<Type> _types;

      public ExporterService ()
        {
          var fullTypes = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ());
          var exporterTypes = fullTypes.Where (t => IsGenericSubclass (t, typeof (IExporter<>)) && !t.IsInterface);
          _types = exporterTypes.Select (t => t.MakeGenericType (typeof (T)));
        }

      private static bool IsGenericSubclass (Type type, Type from)
        {
          return type.IsGenericType
              && type.GetInterfaces ().Any (type => type.IsGenericType
                                                 && type.GetGenericTypeDefinition ()
                                                 == from.GetGenericTypeDefinition ());
        }

      private static string? GetStaticProperty (Type type, string name)
        {
          var property = type.GetProperty (name, BindingFlags.Public | BindingFlags.Static);
        return (string?) property?.GetValue (null);
        }

      public async Task<byte[]> Export (string contentType, IEnumerable<T> values, CancellationToken cancellationToken = default)
        {
          var exporterType = _types.FirstOrDefault (t => contentType == GetStaticProperty (t, nameof (IExporter<T>.ContentType)));

          if (exporterType == null)
            throw new NotFoundException (nameof (IExporter<T>), new object[] { contentType });
          else
            {
              var exporter = (IExporter<T>) Activator.CreateInstance (exporterType)!;
              return await exporter.Export (values);
            }
        }
    }
}
