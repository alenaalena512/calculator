import React, { Component } from 'react';
import {Container, Navbar, NavbarBrand} from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export class NavMenu extends Component {
  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">Calculator</NavbarBrand>
          </Container>
        </Navbar>
      </header>
    );
  }
}
