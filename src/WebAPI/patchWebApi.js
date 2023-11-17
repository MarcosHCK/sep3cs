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
const fs = require ('fs')

/*
 * This file is necesary because:
 * Unfortunately, there is no direct way to add an import statement in a TypeScript file using MSBuild.
 * MSBuild is a build system that is mainly used for compiling and building projects,
 * and it does not have built-in support for manipulating TypeScript files.
 * From: phind.com
 */

const filePath = './ClientApp/src/webApiClient.ts'
const importStatement = `import { ApiClientBase } from './services/ApiClientBase.ts'`

fs.readFile (filePath, 'utf8', (err, data) =>
{
  if (err)
    console.log (`Error reading '${filePath}': '${err}'`)
  else
    {
      const newData = `${importStatement}\n` + data

      fs.writeFile (filePath, newData, (err) =>
  {
    if (err)
      console.log (`Error writing '${filePath}': '${err}'`)
  })
    }
})
