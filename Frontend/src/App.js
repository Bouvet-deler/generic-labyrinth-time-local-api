import { useEffect, useState } from "react";
import "./App.css";
import Leaderboard from "./Leaderboard/Leaderboard";
import SubmitForm from "./SubmitForm/SubmitForm";

function App() {
  const [title, setTitle] = useState("");
  const [users, setUsers] = useState("");
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
    // TODO: Handle fail
    fetch("https://localhost:5050/GetCurrentTopList", {
      method: "GET",
    })
      .then((res) => res.json())
      .then((res) => setUsers(res));
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <h1>{title}</h1>
      </header>
      <Leaderboard users={users} />
      <SubmitForm onSubmitted={fetchUsers} />
    </div>
  );
}

export default App;
