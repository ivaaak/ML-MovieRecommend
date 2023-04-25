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
        const response = fetch('https://localhost:7033/ML/calculate', {    
            method: 'GET',    
            withCredentials: true,    
            crossorigin: true,    
            mode: 'no-cors',       
          })    
            .then((res) => res.json())    
            .then((data) => {    
              console.log(data);    
            })    
            .catch((error) => {    
              console.error(error);    
            });    
        console.log("response", response)
        const data = await response.json();
        this.setState({ mlDataObject: data, loading: false });
    };

    render() {
        return (
            <div>
                <h1 id="tabelLabel" > ML Algorythms </h1>
                <p>This component demonstrates fetching data from the server.</p>
                {}

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
