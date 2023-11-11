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
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace DataClash.Application.UnitTests.Common.Exceptions
{
  public class ValidationExceptionTests
    {
      [Test]
      public void DefaultConstructorCreatesAnEmptyErrorDictionary ()
        {
          var actual = new ValidationException ().Errors;
          actual.Keys.Should ().BeEquivalentTo (Array.Empty<string> ());
        }

      [Test]
      public void SingleValidationFailureCreatesASingleElementErrorDictionary()
        {
          var failures = new List<ValidationFailure>
            {
              new ValidationFailure ("Age", "must be over 18"),
            };

          var actual = new ValidationException (failures).Errors;

          actual.Keys.Should ().BeEquivalentTo (new string [] { "Age" });
          actual ["Age"].Should ().BeEquivalentTo (new string [] { "must be over 18" });
        }

        [Test]
      public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
        {
          var failures = new List<ValidationFailure>
            {
                new ValidationFailure ("Age", "must be 18 or older"),
                new ValidationFailure ("Age", "must be 25 or younger"),
                new ValidationFailure ("Password", "must contain at least 8 characters"),
                new ValidationFailure ("Password", "must contain a digit"),
                new ValidationFailure ("Password", "must contain upper case letter"),
                new ValidationFailure ("Password", "must contain lower case letter"),
            };

          var actual = new ValidationException (failures).Errors;

          actual.Keys.Should ().BeEquivalentTo (new string [] { "Password", "Age" });

          actual ["Age"].Should ().BeEquivalentTo (new string []
            {
              "must be 25 or younger",
              "must be 18 or older",
            });

          actual ["Password"].Should ().BeEquivalentTo (new string[]
            {
              "must contain lower case letter",
              "must contain upper case letter",
              "must contain at least 8 characters",
              "must contain a digit",
            });
        }
    }
}
