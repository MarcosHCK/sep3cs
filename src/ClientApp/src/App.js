import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Login from './components/Login';
import { Link } from 'react-router-dom';

function Navigation() {
    return (
        <nav>
            <Link to="/login">Login</Link>
            {/* Aquí puedes agregar otros enlaces si los necesitas */}
        </nav>
    );
}

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div>
        <Navigation />
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
          <Route path="/login" element={<Login />} />
        </Routes>
      </div>
    );
  }
}
