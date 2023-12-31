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
import { Button, Form, Input, Modal, ModalBody, ModalHeader } from 'reactstrap'
import { CreateErrorReportCommand, ReportClient } from '../webApiClient.ts'
import React, { createContext, useContext, useState } from 'react'
const thisContext = createContext ()

export function useErrorReporter ()
{
  return useContext (thisContext)
}

export function ErrorReporterForm (props)
{
  const { hasNext, onPopAll, onPopError } = props
  const [ description, setDescription ] = useState ('')
  const [ reportClient ] = useState (new ReportClient ())

  const onSubmit = (e) =>
    {
      e.preventDefault ()

      const command = new CreateErrorReportCommand ()

      command.userId = ''
      command.message = description

      // Exception inside exception handler, uff...
      reportClient.create (command)

      setDescription ('')
      onPopError (false)
    }

  return (
    <Form onSubmit={(e) => onSubmit (e)}>
      <Input placeholder='Please let us know what you were doing when the error showed up'
        type='textarea' value={description} onChange={(e) => setDescription (e.target.value)} />
      <div className='d-flex gap-2 justify-content-end mt-2'>
        {
          !hasNext
            ? (<Button onClick={() => { onPopAll () }}>Close</Button>)
            : (<>
              <Button onClick={() => { onPopError () }}>Show next error</Button>
              <Button onClick={() => { onPopAll () }}>Clear all errors</Button>
            </>)
        }
        <Button>Submit</Button>
      </div>
    </Form>)
}

export function ErrorReporterProvider (props)
{
  const [ isOpen, setIsOpen ] = useState (false)
  const [ stack, setStack ] = useState ([])

  return (
    <thisContext.Provider
      value={(error) => { setStack ([...stack, error]); setIsOpen (true) }}>
      <ErrorReporterWidget
        errorObject={stack[stack.length - 1]}
        hasNext={stack.length > 1}
        isOpen={isOpen}
        onClosed={() => { setIsOpen (false); }}
        onPopAll={() => { setIsOpen (false); setStack ([]) }}
        onPopError={(alsoClose = isOpen) =>
          {
            setIsOpen (alsoClose)
            setStack (stack.slice (0, -1))
          }} />
      { props.children }
    </thisContext.Provider>
  )
}

export function ErrorReporterWidget (props)
{
  const { errorObject, hasNext, isOpen, onClosed, onPopAll, onPopError } = props

  return (
    <Modal fade isOpen={isOpen}>
      <ModalHeader close={<Button close onClick={() => onClosed ()} />}>
        Something bad had happened
      </ModalHeader>

      <ModalBody>
        <p>{ !errorObject ? null : errorObject.message }</p>
        <ErrorReporterForm hasNext={hasNext} onPopAll={onPopAll} onPopError={onPopError} />
      </ModalBody>
    </Modal>)
}
