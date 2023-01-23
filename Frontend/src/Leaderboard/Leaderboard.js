import React from "react";

const Leaderboard = ({ users }) => {
  return (
    <article>
      <h2>Leaderboard</h2>
      <dl>
        {users &&
          users.map((user) => (
            <div key={user.name}>
              <dt>{user.name}</dt>
              <dd>{user.points}</dd>
            </div>
          ))}
      </dl>
    </article>
  );
};

export default Leaderboard;
