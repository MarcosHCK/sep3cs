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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataClash.WebUI.Filters
{
  public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
      private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

      public ApiExceptionFilterAttribute ()
        {
          _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
              { typeof (ValidationException), HandleValidationException },
              { typeof (NotFoundException), HandleNotFoundException },
              { typeof (UnauthorizedAccessException), HandleUnauthorizedAccessException },
              { typeof (ForbiddenAccessException), HandleForbiddenAccessException },
            };
        }

      public override void OnException(ExceptionContext context)
        {
          HandleException (context);
          base.OnException (context);
        }

      private void HandleException (ExceptionContext context)
        {
          Type type = context.Exception.GetType ();

          if (_exceptionHandlers.ContainsKey (type))
            {
              _exceptionHandlers [type].Invoke (context);
              return;
            }

          if (!context.ModelState.IsValid)
            {
              HandleInvalidModelStateException (context);
              return;
            }
        }

      private void HandleValidationException(ExceptionContext context)
        {
          var exception = (ValidationException) context.Exception;
          var details = new ValidationProblemDetails (exception.Errors)
            {
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

          context.Result = new BadRequestObjectResult (details);
          context.ExceptionHandled = true;
        }

      private void HandleInvalidModelStateException (ExceptionContext context)
        {
          var details = new ValidationProblemDetails (context.ModelState)
            {
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

          context.Result = new BadRequestObjectResult (details);
          context.ExceptionHandled = true;
        }

      private void HandleNotFoundException (ExceptionContext context)
        {
          var exception = (NotFoundException) context.Exception;
          var details = new ProblemDetails ()
            {
              Detail = exception.Message,
              Title = "The specified resource was not found.",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            };

          context.Result = new NotFoundObjectResult (details);
          context.ExceptionHandled = true;
        }

      private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
          var details = new ProblemDetails
            {
              Status = StatusCodes.Status401Unauthorized,
              Title = "Unauthorized",
              Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            };

          context.Result = new ObjectResult(details)
            {
              StatusCode = StatusCodes.Status401Unauthorized,
            };

          context.ExceptionHandled = true;
        }

      private void HandleForbiddenAccessException(ExceptionContext context)
        {
          var details = new ProblemDetails
            {
              Status = StatusCodes.Status403Forbidden,
              Title = "Forbidden",
              Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            };

          context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status403Forbidden,
            };

          context.ExceptionHandled = true;
        }
    }
}
