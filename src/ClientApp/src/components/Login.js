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
import { Button, Input, Label, Row } from 'reactstrap'
import { Card, CardBody, CardHeader } from 'reactstrap'
import { Container } from 'reactstrap'
import { Form, FormGroup } from 'reactstrap'
import { useNavigate }  from 'react-router-dom'
import axios from 'axios'
import React, { useState } from 'react'

export function Login (props)
{
  let [firstName, setFirstName] = useState ('');
  let [isAdministrator, setIsAdministrator] = useState (false);
  let [lastName, setLastName] = useState ('');
  let [loginMode, setLoginMode] = useState (props.isSignUp ? 'signup' : 'signin');
  let [password, setPassword] = useState ('');
  let [username, setUsername] = useState ('');
  let navigate = useNavigate ();

  const switchLoginMode = () =>
    {
      setLoginMode (loginMode === 'signin' ? 'signup' : 'signin')
    }

  const handleSubmit = async (e) =>
    {
      e.preventDefault ();

      try
        {
          // Hacer una solicitud POST a tu servidor para autenticar al usuario
          let request

          if (loginMode === 'signin')
            {
              request =
                {
                  administrator : isAdministrator,
                  password : password,
                  username : username,
                }
            }
          else
            {
              request =
                {
                  administrator : isAdministrator,
                  firstname : firstName,
                  lastname : lastName,
                  password : password,
                  username : username,
                }
            }

          const response = await axios.post ('api/account/login', request);

          localStorage.setItem ('admin', isAdministrator);
          localStorage.setItem ('token', response.data.token);
          navigate ('/');
        }
			catch (error)
        {
          // Manejar el error
          console.error ('Error al iniciar sesión:', error);
        }
    };

  return (loginMode === 'signin'
    ?
      <Container fluid="sm">
        <Card>
          <CardHeader>
            <h2 className="text-center">
              Sign in
            </h2>
          </CardHeader>
          <CardBody>
            <Container>
              <Row>
                <p className="text-center">
                  Not registered yet?
                    {' '}
                  <span className="link-primary" onClick={switchLoginMode}>
                    Sign Up
                  </span>
                </p>
              </Row>
              <Row>
                <Form onSubmit={handleSubmit}>
                  <FormGroup>
                    <Input type="text" placeholder="Username" value={username} onChange={e => setUsername (e.target.value)} />
                  </FormGroup>
                  <FormGroup>
                    <Input type="password" placeholder="Password" value={password} onChange={e => setPassword (e.target.value)} />
                  </FormGroup>
                  <FormGroup>
                    <Input type="checkbox" value={isAdministrator} onChange={e => setIsAdministrator (e.target.value)} />
                      {' '}
                    <Label check>
                      Log in with administrative powers
                    </Label>
                  </FormGroup>
                  <Button type="submit">Submit</Button>
                </Form>
              </Row>
            </Container>
          </CardBody>
        </Card>
      </Container>
    :
      <Container fluid="sm">
        <Card>
          <CardHeader>
            <h2 className="text-center">
              Sign up
            </h2>
          </CardHeader>
          <CardBody>
            <Container>
              <Row>
                <p className="text-center">
                  Already registered?
                    {' '}
                  <span className="link-primary" onClick={switchLoginMode}>
                    Sign In
                  </span>
                </p>
              </Row>
              <Row>
                <Form onSubmit={handleSubmit}>
                  <FormGroup>
                    <Input type="text" placeholder="Name" value={firstName} onChange={e => setFirstName (e.target.value)} />
                  </FormGroup>
                  <FormGroup>
                    <Input type="text" placeholder="Last Name" value={lastName} onChange={e => setLastName (e.target.value)} />
                  </FormGroup>
                  <FormGroup>
                    <Input type="text" placeholder="Username" value={username} onChange={e => setUsername (e.target.value)} />
                  </FormGroup>
                  <FormGroup>
                    <Input type="password" placeholder="Password" value={password} onChange={e => setPassword (e.target.value)} />
                  </FormGroup>
                  <Button type="submit">Submit</Button>
                </Form>
              </Row>
            </Container>
          </CardBody>
        </Card>
      </Container>)
}
