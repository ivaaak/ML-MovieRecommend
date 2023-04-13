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
      
    constructor(props) {
        super(props);
        this.state = { mlDataObject: {}, loading: true };
    }

    componentDidMount() {
        this.unusedPopulateWeatherData();
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

    async startML() {
        const response = await fetch('ML');
        const data = await response.json();
        this.setState({ mlDataObject: data, loading: false });
    }

    async unusedPopulateWeatherData() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }


    render() {
        let contents = this.state.loading
            ? <p> Loading... Please refresh once the .NET backend has started. </p>
            : App.renderForecastsTable(this.state.mlDataObject);

        return (
            <div>
                <h1 id="tabelLabel" > ML Algorythms </h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}

                <br /> <br /> <br />
                <button onClick={this.startML}>Start ML Algo</button>
                <button onClick={this.startML}>Run Recommend</button>


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
