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

namespace DataClash.WebUI.AcceptanceTests.StepDefinitions
{
  [Binding]
  public sealed class LoginStepDefinitions
    {
      private readonly LoginPage _loginPage;

      public LoginStepDefinitions (LoginPage loginPage)
        {
          _loginPage = loginPage;
        }

      [BeforeFeature ("Login")]
      public static async Task BeforeLoginScenario (IObjectContainer container)
        {
          var playwright = await Playwright.CreateAsync ();
          var options = new BrowserTypeLaunchOptions ();
          var browser = await playwright.Chromium.LaunchAsync (options);
          var page = await browser.NewPageAsync ();
          var loginPage = new LoginPage (browser, page);

          container.RegisterInstanceAs (playwright);
          container.RegisterInstanceAs (browser);
          container.RegisterInstanceAs (loginPage);
        }

      [Given ("a logged out user")]
      public async Task GivenALoggedOutUser ()
        {
          await _loginPage.GotoAsync ();
        }

      [When ("the user logs in with valid credentials")]
      public async Task TheUserLogsInWithValidCredentials ()
        {
          await _loginPage.SetEmail ("administrator@localhost");
          await _loginPage.SetPassword ("Administrator1!");
          await _loginPage.ClickLogin ();
        }

      [Then ("they log in successfully")]
      public async Task TheyLogInSuccessfully ()
        {
          var profileLinkText = await _loginPage.ProfileLinkText ();

          profileLinkText.Should ().NotBeNull ();
          profileLinkText.Should ().Be ("Hello administrator@localhost");
        }

      [When("the user logs in with invalid credentials")]
      public async Task TheUserLogsInWithInvalidCredentials ()
        {
          await _loginPage.SetEmail ("hacker@localhost");
          await _loginPage.SetPassword ("l337hax!");
          await _loginPage.ClickLogin ();
        }

      [Then("an error is displayed")]
      public async Task AnErrorIsDisplayed ()
        {
          var errorVisible = await _loginPage.InvalidLoginAttemptMessageVisible ();

          errorVisible.Should ().BeTrue ();
        }

      [AfterFeature]
      public static async Task AfterScenario (IObjectContainer container)
        {
          var browser = container.Resolve<IBrowser> ();
          var playright = container.Resolve<IPlaywright> ();

          await browser.CloseAsync ();
          playright.Dispose ();
        }
    }
}


