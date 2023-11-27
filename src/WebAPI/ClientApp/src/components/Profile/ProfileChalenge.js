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
import { ChallengeClient,UpdateChallengeCommand }   from '../../webApiClient.ts'
import { ProfilePage } from './ProfilePage'
import { useErrorReporter } from '../ErrorReporter'
import { WaitSpinner } from '../WaitSpinner'
import { ChallengeClient } from '../../webApiClient.ts'
import React, { useEffect, useState } from 'react'

/*necesito mostrar los chalenge existentes y que el usuario elija los que participa */

/*obtener todos los chalenge que estan en chalenge client y mostrarlos ponerlos como un boton que cuando seleccionas se añaden a los chalenge de los usuarios */
