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

const localStorageMock =
{
  getItem: jest.fn (),
  setItem: jest.fn (),
  removeItem: jest.fn (),
  clear: jest.fn (),
};

global.localStorage = localStorageMock;

// Mock the request issued by the react app to get the client configuration parameters.
window.fetch = () =>
{
  return Promise.resolve (
    {
      ok: true,
      json: () => Promise.resolve (
        {
          "authority": "https://localhost:7264",
          "client_id": "DataClash",
          "redirect_uri": "https://localhost:44444/authentication/login-callback",
          "post_logout_redirect_uri": "https://localhost:44444/authentication/logout-callback",
          "response_type": "id_token token",
          "scope": "WebAPIAPI openid profile"
        })
    });
};
