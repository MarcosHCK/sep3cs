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

namespace DataClash.WebUI.AcceptanceTests.Pages
{
  public class LoginPage : BasePage
    {
      public LoginPage (IBrowser browser, IPage page)
        {
          Browser = browser;
          Page = page;
        }

      public override string PagePath => $"{BaseUrl}/authentication/login";
      public override IBrowser Browser { get; }
      public override IPage Page { get; set; }
      public Task SetEmail (string email) => Page.FillAsync ("#Input_Email", email);
      public Task SetPassword( string password) => Page.FillAsync ("#Input_Password", password);
      public Task ClickLogin () => Page.Locator("#login-submit").ClickAsync ();
      public Task<string?> ProfileLinkText () => Page.Locator ("a[href='/authentication/profile']").TextContentAsync ();
      public Task<bool> InvalidLoginAttemptMessageVisible () => Page.Locator ("text=Invalid login attempt.").IsVisibleAsync ();
    }
}
