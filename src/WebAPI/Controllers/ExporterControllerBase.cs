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
using Microsoft.AspNetCore.Mvc;

namespace DataClash.WebUI.Controllers
{
  public abstract class ExporterControllerBase<T> : ApiControllerBase
    {
      private IExporterService<T>? _exporter;
      protected IExporterService<T> Exporter => _exporter ??= HttpContext.RequestServices.GetRequiredService<IExporterService<T>> ();

      protected delegate Task<List<T>> ExportAction ();
      protected async Task<FileResult> ExportResult (string contentType, string? fileName, ExportAction action)
        {
          var valueResult = await action ();
          var exportResult = await Exporter.Export (contentType, valueResult);
          return File (exportResult, contentType, fileName);
        }
    }
}
