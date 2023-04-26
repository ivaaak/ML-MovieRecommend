import React from "react";
import "./Loader.css";

export default function Loader() {
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
      
      <div id="status" className="loading">
        Loading model (9743 Movies)...
      </div>
    </>
  );
}
