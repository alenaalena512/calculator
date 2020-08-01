import React, { Component } from 'react';
import './Button.css'

export class Button extends Component {
    constructor(props) {
        super(props)
        this.state = {
            clicked: false
        }
    }


    getLabel(operation, number) {
        switch (operation) {
            case 'Cancel': return 'C';
            case 'Equals': return '=';
            case 'Multiply': return 'x';
            case 'Divide': return '÷';
            case 'Add': return '+';
            case 'Subtract': return '-';
            case 'Number': return `${number}`;
        }
    }

    render() {
        const className = 'button' + (this.props.operation == 'Cancel' ? ' cancel' : '') + (this.state.clicked? ' clicked': '' );
        const label = this.getLabel(this.props.operation, this.props.number);

        return (
            <div className={className} onMouseDown={() => this.setState({ clicked: true })} onMouseUp={() => this.setState({ clicked: false })} onClick={() => this.props.action(this.props.operation, this.props.number)}> {label} </div>
        );
    }
}