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
using DataClash.Domain.Exceptions;
using DataClash.Domain.ValueObjects;
using FluentValidation;

namespace DataClash.Application.Clans.Commands.CreateClanWithChief
{
  public class CreateClanWithChiefCommandValidator : AbstractValidator<CreateClanWithChiefCommand>
    {
      private static Region? RegionTryFrom (string code)
        {
          try { return Region.From (code); }
          catch (UnknownRegionException) {}
        return null;
        }

      public CreateClanWithChiefCommandValidator ()
        {
          RuleFor (v => v.Description).NotEmpty ().MaximumLength (256);
          RuleFor (v => v.Name).NotEmpty ().MaximumLength (128);
          RuleFor (v => v.Region).NotEmpty ().NotNull ()
            .Must (p => null != RegionTryFrom (p!));
          RuleFor (v => v.TotalTrophiesToEnter).GreaterThanOrEqualTo (0);
          RuleFor (v => v.TotalTrophiesWonOnWar).GreaterThanOrEqualTo (0);
          RuleFor (v => v.Type).IsInEnum ();
        }
    }
}
