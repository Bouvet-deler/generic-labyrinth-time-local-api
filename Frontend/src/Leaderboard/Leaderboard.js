import React, { useEffect } from "react";
import "./Leaderboard.css";
import { useState } from "react";

const Leaderboard = ({ users }) => {

  const newestUsers = users.slice(-2);
  const sortedUsers = [...users]

  sortedUsers.sort((a, b) => {
    const timeA = parseTime(a.time);
    const timeB = parseTime(b.time);
    return timeA - timeB;
  });

  function parseTime(timeString) {
    const [minutes, seconds, milliseconds] = timeString.split(':').map(Number);
    return minutes * 60000 + seconds * 1000 + milliseconds;
  }

  return (
    <article className="leaderboard--wrapper">
      <h2 className="leaderboard__header">Leaderboard</h2>
      {/* 
      {console.log(users, "VANLIG")}
      {console.log(newestUsers)}
      {console.log(sortedUsers, "SORTERT")} */}

      {users && users.length > 0 ? (
        <>
          {users && sortedUsers.slice(0, 10).map((user, index) => (
            <div className="leaderboard" style={{ backgroundColor: user === newestUsers[0] || user === newestUsers[1] ? "#1D43C6" : "none" }} key={user.index}>
              <div className="leaderboard-item">{index + 1}</div>
              <div className="leaderboard-item">{user.name}</div>
              <div className="leaderboard-item">{user.time}</div>
            </div>
          ))
          }
        </>
      ) : (
        <div className="no_scores_container">No registered scores</div>
      )
      }
    </article>
  );
};

export default Leaderboard;
