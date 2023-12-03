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
using MediatR;
using FluentValidation;

namespace DataClash.Application.Statistics.AllClanNames
{
   public record GetAllClanNamesQuery : IRequest<List<string>>;

   public class GetAllClanNamesQueryHandler : IRequestHandler<GetAllClanNamesQuery, List<string>>
   {
       private readonly IApplicationDbContext _context;

       public GetAllClanNamesQueryHandler(IApplicationDbContext context)
       {
           _context = context;
       }

       public async Task<List<string>> Handle(GetAllClanNamesQuery request, CancellationToken cancellationToken)
       {
            var allClanNames = _context.Clans.Select(c => c.Name).ToList();
            return allClanNames;
       }
   }

   public class GetAllClanNamesQueryValidator : AbstractValidator<GetAllClanNamesQuery>
   {
       public GetAllClanNamesQueryValidator()
       {
           // Add validation rules here
       }
   }
}
