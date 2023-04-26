import React, { useState } from "react";
import "./App.css";
import Loader from "./Loader/Loader";
import Result from "./Result/Result";

export default function App() {
  const [mlDataObject, setMlDataObject] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [userId, setUserId] = useState("");
  const [movieId, setMovieId] = useState("");

  async function startML(selectedMovieId, selectedUserId) {
    setLoading(true);
    console.log(`https://localhost:7033/ML/parametrized?movieID=${selectedMovieId}&userID=${selectedUserId}`);
    try {
      const response = await fetch(
        `https://localhost:7033/ML/parametrized?movieID=${selectedMovieId}&userID=${selectedUserId}`, 
        {
          method: "GET",
          withCredentials: true,
          crossorigin: true
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

  function trainTheMLModel() {
    setLoading(true);
    setTimeout(() => setLoading(false), 5000);
  }

  if (loading) {
    return (
      <Loader />
    );
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return (
    <>
      <div className="content">
        <div className="preamble">
          <h1>
            Movie Rating Prediction: <br /> Matrix Factorization
          </h1>
          <p className="about">
            The algorithm for this recommendation task is Matrix Factorization,
            which is a supervised machine learning algorithm performing
            collaborative filtering. Try it below!
          </p>
        </div>

        <div className="form-group">
          <label htmlFor="userId">userId: {userId.value}</label>
          <input
            id="userId"
            className="form-control"
            type="number"
            onChange={e => setUserId({ value: e.target.value })}
          />
          <label htmlFor="movieId">movieId: {movieId.value}</label>
          <input
            id="movieId"
            className="form-control"
            type="number"
            onChange={e => setMovieId({ value: e.target.value })}
          />
        </div>

        <div className="bottom" id="controls" disabled>
          <div style={{ textAlign: "center" }}>
            <button onClick={trainTheMLModel}>Train the ML model</button>
            <button onClick={() => startML(movieId.value, userId.value)}>Run Recommend Engine</button>
          </div>
          <div className="horizontal">
            <div id="style1">
              <h2>Style 1</h2>
              <button id="sample1">Random</button>
            </div>
            <input id="alpha" type="range" min="0" max="5" defaultValue="2" />
            <div id="style2">
              <h2>Style 2</h2>
              <button id="sample2">Random</button>
            </div>
          </div>
        </div>
      </div>
      <p className="fineprint">
        Made with .NET, React, ML.NET, PostgreSQL <br />
        Designed by ____ <br />
        Using the MovieLens dataset which comes <br /> with movie ratings,
        titles and genres. <br />
        May work poorly on mobile.
      </p>
      { mlDataObject && <Result value={mlDataObject}/>}
    </>
  );
}
