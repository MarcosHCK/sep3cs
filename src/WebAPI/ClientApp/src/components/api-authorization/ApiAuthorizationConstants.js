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

const prefix = '/authentication'

export const ApplicationName = 'DataClash.WebUI'

export const LogoutActions =
{
  LoggedOut : 'logged-out',
  Logout : 'logout',
  LogoutCallback : 'logout-callback',
}

export const LoginActions =
{
  Login : 'login',
  LoginCallback : 'login-callback',
  LoginFailed : 'login-failed',
  Profile : 'profile',
  Register : 'register'
}

export const QueryParameterNames =
{
  Message : 'message',
  ReturnUrl : 'returnUrl',
}

export const UserRoles =
{
  Administrator : 'administrator',
  User : 'user'
}

export const ApplicationPaths =
{
  ApiAuthorizationClientConfigurationUrl : `/_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix : prefix,
  DefaultLoginRedirectPath : '/',
  IdentityManagePath : '/Identity/Account/Manage',
  IdentityRegisterPath : '/Identity/Account/Register',
  LoggedOut : `${prefix}/${LogoutActions.LoggedOut}`,
  Login : `${prefix}/${LoginActions.Login}`,
  LoginCallback : `${prefix}/${LoginActions.LoginCallback}`,
  LoginFailed : `${prefix}/${LoginActions.LoginFailed}`,
  LogOut : `${prefix}/${LogoutActions.Logout}`,
  LogOutCallback : `${prefix}/${LogoutActions.LogoutCallback}`,
  Profile : `${prefix}/${LoginActions.Profile}`,
  Register : `${prefix}/${LoginActions.Register}`
}
