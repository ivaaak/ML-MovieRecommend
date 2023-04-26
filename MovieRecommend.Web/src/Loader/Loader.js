import React, { useState, useEffect } from "react";
import "./Loader.css";

export default function Loader() {
  const [showLoadingMessage, setShowLoadingMessage] = useState(false);
  const [showLoadingMessage2, setShowLoadingMessage2] = useState(false);
  const [showLoadingMessage3, setShowLoadingMessage3] = useState(false);

  setTimeout(() => {
    setShowLoadingMessage(true);
  }, 1000);
  setTimeout(() => {
    setShowLoadingMessage2(true);
  }, 2000); setTimeout(() => {
    setShowLoadingMessage3(true);
  }, 3000);

  return (
    <>
      <div className="loader loader3">
        <div>
          <div>
            <div>
              <div>
                <div>
                  <div>
                    <div>
                      <div>
                        <div>
                          <div>
                            <div>
                              <div>
                                <div>
                                  <div></div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <div id="status" className="loading"> Building model (193609 Movies)...</div>
      {showLoadingMessage && <div id="status" className="loading">=============== Training the model ===============</div>}
      {showLoadingMessage2 && <div id="status" className="loading">=============== Evaluating the model ===============</div>}
      {showLoadingMessage3 && <div id="status" className="loading"> Getting Prediction Result... </div>}

    </>
  );
}
