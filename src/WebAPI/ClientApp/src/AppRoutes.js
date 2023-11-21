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
import { ApplicationPaths } from './services/AuthorizeConstants'
import { Home } from './components/Home'
import { Login } from './components/Login'
import { LoginActions } from './services/AuthorizeConstants'
import { Logout } from './components/Logout'
import { LogoutActions } from './services/AuthorizeConstants'
import { Players } from './components/Players'
import { Profile } from './components/Profile'
import { RequireAuth } from './components/RequireAuth'
import { Route, Routes } from 'react-router-dom'
import { Wars } from './components/Wars'

const loginAction = (name) => (<Login action={name}></Login>)
const logoutAction = (name) => (<Logout action={name}></Logout>)

const AppRoutes = () => (
    <Routes>
      <Route path={'/'} element={<Home />} index={true} />

      <Route path={'/cards'} element={<RequireAuth><p>Cards component placeholdes</p></RequireAuth>}/>
      <Route path={'/challenges'} element={<RequireAuth><p>Challenges component placeholdes</p></RequireAuth>}/>
      <Route path={'/clans'} element={<RequireAuth><p>Clans component placeholdes</p></RequireAuth>}/>
      <Route path={'/matches'} element={<RequireAuth><p>Matches component placeholdes</p></RequireAuth>}/>
      <Route path={'/players'} element={<RequireAuth role='Administrator'><Players /></RequireAuth>}/>
      <Route path={'/profile/*'} element={<RequireAuth><Profile /></RequireAuth>}/>
      <Route path={'/wars'} element={<RequireAuth><Wars /></RequireAuth>}/>

      <Route path={ApplicationPaths.ApiAuthorizationPrefix}>
        <Route path={LoginActions.Login} element={ loginAction (LoginActions.Login) }/>
        <Route path={LoginActions.LoginFailed} element={ loginAction (LoginActions.LoginFailed) } />
        <Route path={LoginActions.LoginCallback} element={ loginAction (LoginActions.LoginCallback) } />
        <Route path={LoginActions.Profile} element={ loginAction (LoginActions.Profile) } />
        <Route path={LoginActions.Register} element={ loginAction (LoginActions.Register) } />
        <Route path={LogoutActions.Logout} element={ logoutAction (LogoutActions.Logout) } />
        <Route path={LogoutActions.LogoutCallback} element={ logoutAction (LogoutActions.LogoutCallback) } />
        <Route path={LogoutActions.LoggedOut} element={ logoutAction (LogoutActions.LoggedOut) } />
      </Route>
    </Routes>
)

export default AppRoutes;
