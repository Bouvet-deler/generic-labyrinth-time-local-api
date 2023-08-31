import React, { useState, useEffect } from "react";

function Stopwatch() {
  const [startTime, setStartTime] = useState(0);
  const [running, setRunning] = useState(false);
  const [elapsedTime, setElapsedTime] = useState(0);

  useEffect(() => {
    let timer;
    if (running) {
      timer = setInterval(() => {
        const currentTime = Date.now() - startTime;
        setElapsedTime(currentTime);
      }, 10); // Update every 10 milliseconds
    } else {
      clearInterval(timer);
    }
    return () => clearInterval(timer);
  }, [running, startTime]);

  const start = async () => {
    while((true)){
    await new Promise(r => setTimeout(r, 1000));
    fetch("https://localhost:5050/EndTime", {
      method: "GET",
    })
      .then(async (res) => {
        let text = await new Response(res.body).text();
        if(text === "true"){
         // if (!this.running) {
            this.startTime = Date.now() - this.startTime;
            this.running = true;
            this.updateDisplay();
          //}
        }
      })
    
  }};


  const stop = () => {
    if (running) {
      setRunning(false);
    }
  };

  const reset = () => {
    setStartTime(0);
    setRunning(false);
    setElapsedTime(0);
  };

  const formatTime = (time) => {
    const minutes = Math.floor(time / 60000);
    const seconds = Math.floor((time % 60000) / 1000);
    const milliseconds = time % 1000;
    return `${padZero(minutes)}:${padZero(seconds)}:${padZero(milliseconds, 3)}`;
  };

  const padZero = (num, width = 2) => {
    return String(num).padStart(width, "0");
  };

  return (
    <div className="stopwatch">
      <p>{formatTime(elapsedTime)}</p>
      <button onClick={start}>{running ? "Pause" : "Start"}</button>
      <button onClick={stop}>Stop</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}

export default Stopwatch;
