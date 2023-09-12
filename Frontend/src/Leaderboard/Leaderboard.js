import React from "react";
import "./Leaderboard.css";
import LeaderboardPosition from "./LeaderboardPosition"

const Leaderboard = ({ users }) => {
	const newestUsers = users.slice(-2);
	const sortedUsers = [...users]

	sortedUsers.sort((a, b) => {
		const timeA = parseTime(a.time);
		const timeB = parseTime(b.time);
		return timeA - timeB;
	});

	function parseTime(timeString) {
		if (timeString !== null) {
			const [minutes, seconds, milliseconds] = timeString.split(':').map(Number);
			return minutes * 60000 + seconds * 1000 + milliseconds;
		}
	}

	return (
		<article className="leaderboard--wrapper">
			<h2 className="leaderboard__header">Leaderboard</h2>
			{users && users.length > 0 ? (
				<>
					{users && sortedUsers.slice(0, 15).map((user, index) => (
						<div key={index} className={`leaderboard-row ${user === newestUsers[0] || user === newestUsers[1] ? "leaderboard-row-new" : "leaderboard-row"}`}>
							<div className="leaderboard-item place-item"><LeaderboardPosition place={index + 1}></LeaderboardPosition></div>
							<div className="leaderboard-item name-item">{user.name}</div>
							<div className="leaderboard-item last-item">{user.time}</div>
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
