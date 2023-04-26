import React from "react";
import "./Result.css";
import { DataTable, Column } from "primereact";

export default function Result(valuesObject) {
  return (
    <>
      <DataTable className={'movieTable'} value={valuesObject} tableStyle={{ minWidth: "50rem" }}>
        <Column field="code" header="Code"></Column>
        <Column field="name" header="Name"></Column>
        <Column field="category" header="Category"></Column>
        <Column field="quantity" header="Quantity"></Column>
      </DataTable>
    </>
  );
}
