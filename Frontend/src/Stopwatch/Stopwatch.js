import React, { useState, useEffect } from "react";

function Stopwatch() {
  const [startTime, setStartTime] = useState(0);
  const [running, setRunning] = useState(false);
  const [laps, setLaps] = useState([]);
  const [p1d, setP1d] = useState(false);
  const [p2d, setP2d] = useState(false);
  const [elapsedTime, setElapsedTime] = useState(0);
  const [lapTime, setLapTime] = useState(0);
  const [lapTime2, setLapTime2] = useState(0);


  




  const [doneTime, setDoneTime] = useState(0);

  useEffect(() => {
    let timer;
    if (running) {
      timer = setInterval(() => {
        const currentTime = Date.now() - startTime;
        setElapsedTime(currentTime);
        //console.log(currentTime);
      }, 10); // Update every 10 milliseconds
    } else {
      clearInterval(timer);
    }
    return () => clearInterval(timer);
  }, [running, startTime]);


  useEffect(() => {
    if(p1d) {
      getLapTime();
    }
    if(p2d){
      getLapTime2();
    }
    if(p1d && p2d) {
      setRunning(false);
    }


  }, [p1d, p2d]);



  const start = async () => {

   // reset();
    let started = false;
    while(!started){
    await new Promise(r => setTimeout(r, 100));
    fetch("https://localhost:5050/StartTime", {
      method: "GET",
    })
      .then(async (res) => {
        let text = await new Response(res.body).text();
        if(started === true){
          return;
        }
        if(text === "true"){
            started = true;
            setStartTime(Date.now() - elapsedTime);
            setRunning(true);
            getEnd();
        }
      })
    
  }
setDoneTime(elapsedTime);
};

  const getEnd = async () => {
    let playerOneDone = false;
    let playerTwoDone = false;
    let allPlayersDone = false;

    while(allPlayersDone === false){

      if(playerOneDone === false){
      await new Promise(r => setTimeout(r, 100));
      fetch("https://localhost:5050/EndTime", {
        method: "GET",
      })
        .then(async (res) => {
          let text = await new Response(res.body).text();
          if(playerOneDone === true){
            return;
          }
          if(text === "true"){
             playerOneDone = true;
             setP1d(true);

          }
        })
      }
      if (playerTwoDone === false){
        await new Promise(r => setTimeout(r, 100));
        fetch("https://localhost:5050/EndTime2", {
          method: "GET",
        })
          .then(async (res) => {
            let text = await new Response(res.body).text();
            if(playerTwoDone === true){
              return;
            }
            if(text === "true"){
                playerTwoDone = true;
                setP2d(true);

            }
          })
      }
      if(playerOneDone && playerTwoDone){
        //setRunning(false);
        allPlayersDone = true;
      }
  }
};



  const reset = () => {
    fetch(`https://localhost:5050/ResetTime`, {
      method: "GET",
    })
      .then((res) => {
        
        setStartTime(0);
        setP1d(false);
        setP2d(false);
         setRunning(false);
         //running = false;
         setElapsedTime(0);
        if (!res.ok) {
          throw res;
        }
      })

  };


  const getLapTime = () => {
    fetch('https://localhost:5050/sendTimePlayer1')
    .then(async (res) => {
      if (!res.ok) {
        throw res;
      }
      const runTime = await res.text().then((text) => {
        return text.toString();
      })
      console.log("LAP1 - " , runTime)
      setLapTime(runTime);
    });

  }

  const getLapTime2 = () => {
    fetch('https://localhost:5050/sendTimePlayer2')
    .then(async (res) => {
      if (!res.ok) {
        throw res;
      }
      const runTime = await res.text().then((text) => {
        return text.toString();
      })
      console.log(runTime);
      setLapTime2(runTime);
    });

  }

 
  const lap = () => {
    if (running) {
      const now = Date.now();
      const lapTime = now - startTime;
      setLaps([...laps, lapTime]);
    }
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
      <button onClick={start}>Ready</button>
      <button onClick={reset}>New run</button>
      {lapTime !== 0 && (
        <p>{formatTime(lapTime)}</p>
      ) }
      {lapTime2 != 0 &&(
        <p>{formatTime(lapTime2)}</p>
      )}
    </div>
  );
}


export default Stopwatch;
