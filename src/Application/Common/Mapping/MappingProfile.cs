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
using AutoMapper;
using System.Reflection;

namespace DataClash.Application.Common.Mappings
{
  public class MappingProfile : Profile
    {
      public MappingProfile ()
        {
          ApplyMappingsFromAssembly (Assembly.GetExecutingAssembly ());
        }

      private void ApplyMappingsFromAssembly (Assembly assembly)
        {
          var mapFromType = typeof (IMapFrom<>);
          var mappingMethodName = nameof (IMapFrom<object>.Mapping);

          bool HasInterface (Type t) => t.IsGenericType && t.GetGenericTypeDefinition () == mapFromType;

          var types = assembly.GetExportedTypes ().Where (t => t.GetInterfaces ().Any (HasInterface)).ToList ();
          var argumentTypes = new Type[] { typeof (Profile) };

          foreach (var type in types)
            {
              var instance = Activator.CreateInstance (type);
              var methodInfo = type.GetMethod (mappingMethodName);

              if (methodInfo != null)

                methodInfo.Invoke (instance, new object[] { this });
              else
                {
                  var interfaces = type.GetInterfaces ().Where (HasInterface).ToList ();

                  if (interfaces.Count > 0)
                    {
                      foreach (var @interface in interfaces)
                        {
                          var interfaceMethodInfo = @interface.GetMethod (mappingMethodName, argumentTypes);
                          interfaceMethodInfo?.Invoke (instance, new object[] { this });
                        }
                    }
                }
            }
        }
    }
}
