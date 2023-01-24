import React from "react";
import "./Leaderboard.css";

const Leaderboard = ({ users }) => {
  return (
    <article>
      <h2>Leaderboard</h2>
      {/* Eventuelt bruk <ol> og <li> Eventuelt table*/}
      {users && users.length > 0 ? (
        <ol>
          {users &&
            users.map((user) => (
              <li key={user.name}>
                <span>{user.name}</span>
                <span>{user.points}</span>
              </li>
            ))}
        </ol>
      ) : (
        <div>No registered scores</div>
      )}
    </article>
  );
};

export default Leaderboard;
