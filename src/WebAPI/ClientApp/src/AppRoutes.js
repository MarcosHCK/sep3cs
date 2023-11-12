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
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants'
import { Login } from './components/api-authorization/Login'
import { LoginActions } from './components/api-authorization/ApiAuthorizationConstants'
import { Logout } from './components/api-authorization/Logout'
import { LogoutActions } from './components/api-authorization/ApiAuthorizationConstants'
import { Home } from './components/Home'
import { Route, Routes } from 'react-router-dom'

const loginAction = (name) => (<Login action={name}></Login>)
const logoutAction = (name) => (<Logout action={name}></Logout>)

const AppRoutes = () => (
    <Routes>
      <Route path={'/'} element={<Home />} index={true} />
      <Route path={ApplicationPaths.Login} element={ loginAction (LoginActions.Login) }/>
      <Route path={ApplicationPaths.LoginFailed} element={ loginAction (LoginActions.LoginFailed) } />
      <Route path={ApplicationPaths.LoginCallback} element={ loginAction (LoginActions.LoginCallback) } />
      <Route path={ApplicationPaths.Profile} element={ loginAction (LoginActions.Profile) } />
      <Route path={ApplicationPaths.Register} element={ loginAction (LoginActions.Register) } />
      <Route path={ApplicationPaths.LogOut} element={ logoutAction (LogoutActions.Logout) } />
      <Route path={ApplicationPaths.LogOutCallback} element={ logoutAction (LogoutActions.LogoutCallback) } />
      <Route path={ApplicationPaths.LoggedOut} element={ logoutAction (LogoutActions.LoggedOut) } />
    </Routes>
)

export default AppRoutes;
