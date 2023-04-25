import React, { useState } from "react";

export default function App() {
  const [mlDataObject, setMlDataObject] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  async function startML() {
    setLoading(true);
    try {
      const response = await fetch("https://localhost:7033/ML/calculate", {
        method: "GET",
        withCredentials: true,
        crossorigin: true,
      });
      const data = await response.json();
      console.log(data);
      setMlDataObject(data);
    } catch (error) {
      setError(error);
    } finally {
      setLoading(false);
    }
  }

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return (
    <div>
      <h1 id="tabelLabel"> ML Algorythms </h1>
      <p>This component demonstrates fetching data from the server.</p>
      <br /> <br /> <br />
      <button onClick={startML}>Start ML Algo</button>
      <button onClick={startML}>Run Recommend</button>
      <div>
        {/* display the fetched data */}
        {mlDataObject && (
          <pre>{JSON.stringify(mlDataObject, null, 2)}</pre>
        )}
      </div>
    </div>
  );
}