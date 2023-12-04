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
import React from 'react';
import './HomeTable.css';


export function Mural() {
  return (
  <div className="mural">
    <div style={{ display: 'flex', alignItems: 'center' }}>
      <img src="welcome.jfif" alt="Welcome" style={{ marginRight: '20px' }} />
      <h2 style={{ color: '#63a5b5' }}>Welcome to DataClash</h2>
    </div>
    <p style={{ fontSize: '20px', fontFamily: 'unset' }}>Discover the world of Clash Royale like never before! Here you can find detailed statistics of the best clans, players, and most popular cards in Clash Royale. But that's not all, you can also create your own profile and manage your data. Come and join the Clash Royale community!</p>
    <p style={{ fontSize: '20px', fontFamily: 'unset' }}>Explore our different sections to get more information.Dive even deeper into the exciting world of Clash Royale. We're thrilled to have you with us!</p>
  </div>
  );
 }
 
 

function convertToTableFormat(columns, data) {
  return data.map(item => {
    let obj = {};
    columns.forEach((column, index) => {
      obj[column] = item[index];
    });
    return obj;
  });
}

const Table = ({ columns, data }) => {
  const tableData = convertToTableFormat(columns, data);

  return (
    <table>
      <thead>
        <tr>
          {columns.map((column, index) => (
            <th key={index}>{column}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {tableData.map((row, index) => (
          <tr key={index}>
            {columns.map((column, index) => (
              <td key={index}>{row[column]}</td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Table;
