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
import './WaitSpinner.css'
import { Modal, ModalBody, Spinner } from 'reactstrap'
import React from 'react'

export function WaitSpinner ()
{
  return (
    <Modal centered className='wait-spinner' fade={false} fullscreen isOpen={true}>
      <ModalBody className='d-flex justify-content-center align-items-center'>
        <div className='wait-spinner-wrapper'>
          <Spinner className='wait-spinner-object' color='primary' />
        </div>
      </ModalBody>
    </Modal>)
}
