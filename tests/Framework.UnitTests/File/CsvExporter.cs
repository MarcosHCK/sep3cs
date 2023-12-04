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
using DataClash.Framework.Files;
using FluentAssertions;
using NUnit.Framework;

namespace DataClash.Framework.UnitTests
{
  public class CsvExporterTests
    {
      [Test]
      public async Task ShouldExport ()
        {
          var exporter = new CsvExporter<long> ();
          var longs = new List<long> { 1, 3, 5, 6 };

          FluentActions.Invoking (async () => await exporter.Export (longs)).Should ().NotBeNull ();
          await Task.CompletedTask;
        }

      [Test]
      public async Task ShouldNotThrow ()
        {
          var exporter = new CsvExporter<long> ();
          var longs = new List<long> { 1, 3, 5, 6 };

          await FluentActions.Invoking (() => exporter.Export (longs)).Should ().NotThrowAsync ();
        }
    }
}
