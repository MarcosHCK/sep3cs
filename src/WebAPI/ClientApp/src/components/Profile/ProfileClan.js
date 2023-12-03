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
import './Profile.css'
import { Alert, Button, Form, FormGroup, Input, Label } from 'reactstrap'
import { ClanClient, ClanRole, ClanType, DeleteClanCommand } from '../../webApiClient.ts'
import { CreateClanWithChiefCommand } from '../../webApiClient.ts'
import { IntegerInput } from '../IntegerInput'
import { ProfilePage } from './ProfilePage'
import { UpdateClanCommand } from '../../webApiClient.ts'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import React, { useEffect, useState } from 'react'

export function ProfileClan (props)
{
  const { playerProfile, userProfile } = props
  const [ clanClient ] = useState (new ClanClient ())
  const [ clanDescription, setClanDescription ] = useState ()
  const [ clanId, setClanId ] = useState ()
  const [ clanName, setClanName ] = useState ()
  const [ clanRegion, setClanRegion ] = useState ()
  const [ clanRole, setClanRole ] = useState ()
  const [ clanTotalTrophiesToEnter, setClanTotalTrophiesToEnter ] = useState ()
  const [ clanTotalTrophiesWonOnWar, setClanTotalTrophiesWonOnWar ] = useState ()
  const [ clanType, setClanType ] = useState ()
  const [ hasClan, setHasClan ] = useState (false)
  const [ isLoading, setIsLoading ] = useState (false)
  const errorReporter = useErrorReporter ()

  const refreshClan = async () =>
    {
      if (!!playerProfile) try
        {
          const currentClan = await clanClient.getForCurrentPlayer ()

          if (currentClan === null)
            setHasClan (false)
          else
            {
              setHasClan (true)
              const clan = currentClan.clan
              const role = currentClan.role

              setClanDescription (clan.description)
              setClanId (clan.id)
              setClanName (clan.name)
              setClanRegion (clan.region)
              setClanRole (role)
              setClanTotalTrophiesToEnter (clan.totalTrophiesToEnter)
              setClanTotalTrophiesWonOnWar (clan.totalTrophiesWonOnWar)
              setClanType (clan.type)
            }
        }
      catch (error) { errorReporter (error) }
    }

  const createClan = async () =>
    {
      if (!!playerProfile) try
        {
          const command = new CreateClanWithChiefCommand ()

          command.description = `${playerProfile.nickname ?? userProfile.name}'s clan`
          command.name = `${playerProfile.nickname ?? userProfile.name}'s clan`
          command.region = 'Anywhere'
          command.totalTrophiesToEnter = 0
          command.totalTrophiesWonOnWar = 0
          command.type = ClanType.Normal

          await clanClient.createWithChief (command)
          await refreshClan ()
        }
      catch (error) { errorReporter (error) }
    }

  const deleteClan = async () =>
    {
      if (hasClan) try
        {
          const command = new DeleteClanCommand ()

          command.id = clanId

          await clanClient.delete (command)
          await refreshClan ()
        }
      catch (error) { errorReporter (error) }
    }

  const onSubmit = async (e) =>
    {
      if (hasClan) try
        {
          const command = new UpdateClanCommand ()

          command.description = clanDescription
          command.id = clanId
          command.name = clanName
          command.region = clanRegion
          command.totalTrophiesToEnter = clanTotalTrophiesToEnter
          command.totalTrophiesWonOnWar = clanTotalTrophiesWonOnWar
          command.type = clanType

          await clanClient.update (command)
        }
      catch (error) { errorReporter (error) }
    }

  useEffect (() =>
    {
      setIsLoading (true)
      refreshClan ().then (() => setIsLoading (false))
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [playerProfile])

  const clanTypes = Object.keys (ClanType).filter (k => !isNaN (Number (ClanType[k])))
  const clanRegions = [ 'Africa', 'Anywhere', 'Asia', 'Europa', 'North America', 'Oceania', 'South America' ]

  if (!playerProfile)
    return (<Alert color='warning'>User has not player status</Alert>)
  else
    return (
      isLoading
      ? <WaitSpinner />
      : <ProfilePage title='Clan'>
      { !hasClan
      ? <>
          <Alert color='info'>You do not belong to any clan</Alert>
          <div className='d-flex justify-content-end'>
            <Button color='primary'
                    onClick={() =>
              {
                setIsLoading (true)
                createClan ().then (() => setIsLoading (false))
              }}>+</Button>
          </div>
        </>
      : (<Form onSubmit={(e) => { e.preventDefault (); setIsLoading (true); onSubmit ().then (() => setIsLoading (false)) }}>
          <FormGroup floating>
            <Input id='clan-input-description' type='text'
              disabled={clanRole !== ClanRole.Chief}
              onChange={(e) => setClanDescription (e.target.value)}
              value={clanDescription} />
            <Label for='clan-input-description'>Clan description</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='clan-input-name' type='text'
              disabled={clanRole !== ClanRole.Chief}
              onChange={(e) => setClanName (e.target.value)}
              value={clanName} />
            <Label for='clan-input-name'>Clan name</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='clan-input-region' type='select'
              disabled={clanRole !== ClanRole.Chief}
              onChange={(e) => setClanRegion (e.target.value)}
              defaultValue={clanRegion}>
            { clanRegions.map (region => <option>{region}</option>) }
            </Input>
            <Label for='clan-input-region'>Clan region</Label>
          </FormGroup>
          <FormGroup floating>
            <IntegerInput id='clan-input-total-trophies-to-enter'
              defaultValue={clanTotalTrophiesToEnter}
              disabled={clanRole !== ClanRole.Chief}
              natural
              onChanged={value => setClanTotalTrophiesToEnter (value)}/>
            <Label for='clan-input-total-trophies-to-enter'>Throphies needed to enter</Label>
          </FormGroup>
          <FormGroup floating>
            <IntegerInput id='clan-input-total-trophies-won-on-war'
              defaultValue={clanTotalTrophiesWonOnWar}
              disabled={clanRole !== ClanRole.Chief}
              natural
              onChanged={value => setClanTotalTrophiesWonOnWar (value)}/>
            <Label for='clan-input-total-trophies-won-on-war'>Total trophies won on clan wars</Label>
          </FormGroup>
          <FormGroup floating>
            <Input id='clan-input-type' type='select'
              disabled={clanRole !== ClanRole.Chief}
              onChange={(e) => setClanType (ClanType[e.target.value])}
              value={clanTypes.find (k => k === ClanType[clanType])} >
            { clanTypes.map ((type) => !isNaN (Number (type)) ? <></> : <option>{type}</option>) }
            </Input>
            <Label for='clan-input-type'>Clan type</Label>
          </FormGroup>
      { clanRole !== ClanRole.Chief
        ? <></>
        : <FormGroup>
            <div className='d-flex gap-2'>
              <Button color='primary'>Submit</Button>
              <span className='flex-grow-1'/>
              <Button color='danger' onClick={() => { setIsLoading (true); deleteClan ().then (() => setIsLoading (false)) }}>Delete</Button>
            </div>
          </FormGroup>}
        </Form>)}
      </ProfilePage>)
}
