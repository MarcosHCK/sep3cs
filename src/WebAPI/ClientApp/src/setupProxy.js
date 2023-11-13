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
const { createProxyMiddleware } = require ('http-proxy-middleware');
const { env } = require ('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
                                         : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:32831';

const context = [
  "/_configuration",
  "/_framework",
  "/.well-known",
  "/api",
  "/ApplyDatabaseMigrations",
  "/connect",
  "/Identity",
];

const onError = (err, req, resp, target) => {
    console.error(`${err.message}`);
}

module.exports = function (app)
{
  const appProxy = createProxyMiddleware (
    context,
    {
      headers : {
        Connection: 'Keep-Alive'
          },
      onError : onError,
      proxyTimeout : 10000,
      secure : false,
      target : target,
    });

  app.use (appProxy);
};
