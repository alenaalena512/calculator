import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { CalcBody } from './components/CalcBody';

export default class App extends Component {

  render () {
    return (
      <Layout>
            <Route exact path='/' component={CalcBody} />
      </Layout>
    );
  }
}
