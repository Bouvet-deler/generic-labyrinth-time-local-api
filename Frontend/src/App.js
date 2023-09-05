import { useEffect, useState } from "react";
import "./App.css";
import Leaderboard from "./Leaderboard/Leaderboard";
import SubmitForm from "./SubmitForm/SubmitForm";
import Stopwatch from "./Stopwatch/Stopwatch";

function App() {
  const [title, setTitle] = useState("");
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5050/GetCurrentTopListName", {
      method: "GET",
    })
      .then((res) => res.text())
      .then((res) => {
        setTitle(res);
      });
  }, []);

  const fetchUsers = () => {
    fetch("https://localhost:5050/GetCurrentTopList", {
      method: "GET",
    })
      .then((res) => {
        if (!res.ok) {
          return [];
        }
        return res.json();
      })
      .then((res) => setUsers(res));
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <>
      <div style={{ marginTop: -100 }}><img src="./Bouvet_Logo_blue.png" height={100}></img></div>
      <div className="app">
        <header className="header">
          <Stopwatch />
        </header>
        <div className="app-body">
          <Leaderboard users={users} />
          <SubmitForm onSubmitted={fetchUsers} />
        </div>
      </div>
      <img src="./illustration.png" style={{ marginTop: "-3rem", marginLeft: "2rem" }}></img>
    </>

  );
}

export default App;
