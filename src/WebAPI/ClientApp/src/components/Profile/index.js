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
import { PlayerClient } from '../../webApiClient.ts'
import { Profile } from './Profile'
import { useAuthorize } from '../../services/AuthorizeProvider'
import { useErrorReporter } from '../ErrorReporter'
import React, { useEffect, useState } from 'react'
import { WaitSpinner } from '../WaitSpinner.js'
import { Alert } from 'reactstrap'

export { Profile }

export function CurrentProfile ()
{
  const { isAuthorized, userProfile } = useAuthorize ()
  const [ playerProfile, setPlayerProfile ] = useState ()
  const [ playerClient ] = useState (new PlayerClient ())
  const errorReporter = useErrorReporter ()

  const downProps = { playerProfile, userProfile }

  useEffect (() =>
    {
      const refreshPlayer = async () =>
        {
          if (isAuthorized) try
            {
              return await playerClient.getCurrent ()
            }
          catch (error)
            {
              errorReporter (error)
            }
        }

      setPlayerProfile (undefined)
      refreshPlayer ().then ((player) => setPlayerProfile (player))
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isAuthorized])

  if (!isAuthorized || !userProfile)
    return <WaitSpinner />
  else if (!playerProfile)
    return <Alert color='danger'>User is not a player</Alert>
  else
return <Profile {...downProps} />
}
