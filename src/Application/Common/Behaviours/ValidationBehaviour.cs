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
using FluentValidation;
using MediatR;
using ValidationException = DataClash.Application.Common.Exceptions.ValidationException;

namespace DataClash.Application.Common.Behaviours
{
  public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : notnull
    {
      private readonly IEnumerable<IValidator<TRequest>> _validators;

      public ValidationBehaviour (IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

      public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
          if (_validators.Any ())
          {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll (_validators.Select (v => v.ValidateAsync (context, cancellationToken)));
            var failures = validationResults.Where (r => r.Errors.Any ()).SelectMany (r => r.Errors).ToList ();

            if (failures.Any ())
              throw new ValidationException (failures);
          }
        return await next();
        }
    }
}
