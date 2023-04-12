import React, { Component } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';

export default class App extends Component {
    static displayName = App.name;

    products = [
        { code: 'P001', name: 'Product 1', category: 'Category A', quantity: 10 },
        { code: 'P002', name: 'Product 2', category: 'Category B', quantity: 5 },
        { code: 'P003', name: 'Product 3', category: 'Category A', quantity: 8 },
        { code: 'P004', name: 'Product 4', category: 'Category C', quantity: 12 },
        { code: 'P005', name: 'Product 5', category: 'Category B', quantity: 3 },
    ];
      
    mlDataObject = {};

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    async startMLAlgorythm() {
        await fetch('https://localhost:7033/ML')
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.mlDataObject = data;
            })
            .catch(error => {
                console.error(error);
            });
    };

    async populateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading... Please refresh once the .NET backend has started. See 
                <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact
                </a> for more details.</em></p>
            : App.renderForecastsTable(this.state.forecasts);

        return (
            <div>
                <h1 id="tabelLabel" > ML Algorythms </h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}

                <br /> <br /> <br />
                <button onClick={this.fetchData}>Fetch Data</button>

                <DataTable value={this.products} tableStyle={{ minWidth: '50rem' }}>
                    <Column field="code" header="Code"></Column>
                    <Column field="name" header="Name"></Column>
                    <Column field="category" header="Category"></Column>
                    <Column field="quantity" header="Quantity"></Column>
                </DataTable>
            </div>
        );
    };
};
