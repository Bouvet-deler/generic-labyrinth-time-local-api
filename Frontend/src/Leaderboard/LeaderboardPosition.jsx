import React from "react";

const LeaderboardPosition = ({ place }) => {

    const getFillColor = (place) => {
        if (place == 1) {
            return "#FFC700"
        }
        else if (place == 2) {
            return "#8EB0B2"
        }
        else if (place == 3) {
            return "#F47A25"
        }
        else {
            return "none"
        }
    }

    return (
        <>
            <svg width="40" height="40" viewBox="0 0 40 40" xmlns="http://www.w3.org/2000/svg">
                <circle cx="20" cy="20" r="15" fill={getFillColor(place)} />
                <text style={{ fontSize: "20px", fontFamily: "InterBold" }} x="50%" y="50%" textAnchor="middle" dy="0.3em" fill={[1, 2, 3].includes(place) ? "#ffffff" : "#11133A"}>
                    {place}
                </text>
            </svg>
        </>

    )
}

export default LeaderboardPosition;