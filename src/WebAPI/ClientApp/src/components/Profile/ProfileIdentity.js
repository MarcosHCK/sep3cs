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
import { Button, Form, FormGroup, Input, Label } from 'reactstrap'
import { ProfilePage } from './ProfilePage'
import { useErrorReporter } from '../ErrorReporter'
import React, { useState } from 'react'

export function ProfileIdentity (props)
{
  const { userProfile } = props
  const [ userName, setUserName ] = useState (userProfile.name)
  const [ userNick, setUserNick ] = useState (userProfile.nickname)
  const [ userEmail, setUserEmail ] = useState (userProfile.email)
  const errorReporter = useErrorReporter ()

  const onSubmit = (e) =>
    {
      e.preventDefault ()

      if (userProfile.name !== userName)
        {
          errorReporter (new Error ('Unimplemented'))
        }
    }

  return (
    <ProfilePage title='Identity'>
      <Form onSubmit={(e) => { onSubmit (e) }}>
        <FormGroup floating>
          <Input id='identity-input-useremail' type='text' onChange={(e) => setUserEmail (e.target.value)} value={userEmail} disabled/>
          <Label for='identity-input-useremail'>Email</Label>
        </FormGroup>
        <FormGroup floating>
          <Input id='identity-input-username' type='text' onChange={(e) => setUserName (e.target.value)} value={userName} />
          <Label for='identity-input-username'>Username</Label>
        </FormGroup>
        <FormGroup floating>
          <Input id='identity-input-usernick' type='text' onChange={(e) => setUserNick (e.target.value)} value={userNick} />
          <Label for='identity-input-usernick'>Nickname</Label>
        </FormGroup>
        <Button color='primary'>Update</Button>
      </Form>
    </ProfilePage>)
}
