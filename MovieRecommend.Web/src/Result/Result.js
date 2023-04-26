import React from "react";
import { DataTable, Column } from "primereact";

export default function Result(valuesObjectArray) {

  return (
    <>
      <DataTable
        className={"movieTable"}
        value={valuesObjectArray.value}
        tableStyle={{ minWidth: "50rem" }}
      >
        <Column
          field="evaluationMetricsError"
          header="Evaluation Metrics Error"
        ></Column>
        <Column field="movieTitleInputted" header="Movie Title"></Column>
        <Column field="userID" header="User ID"></Column>
        <Column
          field="predictedRatingResult"
          header="Predicted Rating"
        ></Column>
      </DataTable>
    </>
  );
}
