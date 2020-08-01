import React, { Component } from 'react';
import { Button } from './Button';
import './CalcBody.css';

export class CalcBody extends Component {

    constructor(props) {
        super(props);
        this.state = { calculationResult: '' };

        this.doClick = this.doClick.bind(this);
    }

    async componentDidMount(){
        const response = await fetch('http://localhost:5000/calculator', {
            headers: { 'Content-Type': 'application/json' }
        });
        const json = await response.json();
        this.setState({ calculationResult: json.display});
    }

    async doClick(operation, number) {

        let data = {
            operation: operation,
            number: number != null ? `${number}` : null
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };

        const response = await fetch('http://localhost:5000/calculator/press', requestOptions);
        const json = await response.json();
        this.setState({ calculationResult: json.display});
    }

    render() {
        const numbers = [7, 8, 9, 4, 5, 6, 1, 2, 3, 0, '.'];
        const operations = ['Multiply', 'Divide', 'Add', 'Subtract'];

        return (
            <div className='mainBody'>
                <div className='result'> {this.state.calculationResult}</div>
                <div className='buttonsZone'>
                    <div className='leftButtons'>
                        {numbers.map(num => <Button action={this.doClick} operation='Number' number={num} />)}
                        <Button action={this.doClick} operation='Equals' />
                        <Button action={this.doClick} operation='Cancel' />
                    </div>
                    <div className='rightButtons'>
                        {operations.map(c => <Button action={this.doClick} operation={c} />)}
                    </div>
                </div>
            </div>
        );
    }
}