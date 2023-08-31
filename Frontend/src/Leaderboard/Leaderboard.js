import React from "react";
import "./Leaderboard.css";

const Leaderboard = ({ users }) => {
  return (
    <article className="leaderboard--wrapper">
      <h2 className="leaderboard__header">Leaderboard</h2>
      {users && users.length > 0 ? (
        <div className="leaderboard">
          {users &&
            users.slice(0, 10).map((user, index) => (
              <div key={user.name} className="leaderboard-item">
                <span>{index + 1}:</span>
                <span>{user.name}</span>
                <span>{user.time}</span>
              </div>
            ))}
        </div>
      ) : (
        <div className="no_scores_container">No registered scores</div>
      )}
    </article>
  );
};

export default Leaderboard;
