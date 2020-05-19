import React, { Component } from 'react'
import './CalcBody.css'

export class CalcBody extends Component {
    static DisplayName = CalcBody.name;
    constructor(props) {
        super(props);
        this.state = { currentCount: "0"}
    }

    async doClick(button, type) {
        let data = {
            operation: type,
            number: button != null ? `${button}` : null
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };
        const response = await fetch('https://localhost:5001/calculator/press', requestOptions);
        const json = await response.json();
        this.setState({ currentCount: json.display });
    }

    render() {

        let buttons = {
            "Numbers": [7,8,9,4,5,6,1,2,3,0,'.'],
            "Operations": { "×": "Multiply", "÷": "Divide", "+": 'Add', "-": "Subtract" }
        };

        return(
            <div className="mainBody">
                <div className='result'> {this.state.currentCount}</div>
                <div className='numZone'>
                    <div className='buttons'>
                        {buttons.Numbers.map(button => <div className='button' onClick={() => this.doClick(button, "Number")}> {button} </div>)}
                        <div className='button' onClick={() => this.doClick(null, "Equals")} >=</div>
                        <div className='button cancel' onClick={() => this.doClick(null, "Cancel")} > C </div>
                    </div>
                    <div className='actions'>
                        {Object.keys(buttons.Operations).map(c => <div className='button' onClick={() => this.doClick(null, buttons.Operations[c])}> {c} </div>)}
                    </div>
                </div>
            </div>
            )
    }
}