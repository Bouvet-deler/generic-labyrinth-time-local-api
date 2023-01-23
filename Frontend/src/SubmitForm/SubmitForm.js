import React, { useState } from "react";

const SubmitForm = ({ onSuccess }) => {
  const [username, setUsername] = useState("");
  const [code, setCode] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    //TODO: Show loading
    fetch(`https://localhost:5050/NewCodeEntry?username=${username}&code=${code}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error("Failed");
        }
      })
      .then(() => {
        setUsername("");
        setCode("");
        onSuccess();
        // TODO: Show success messsage
      })
      .catch(() => {
        // TODO: Show something went wrong
      });
  };
  return (
    <article>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
        </label>
        <label>
          Code:
          <input type="text" value={code} onChange={(e) => setCode(e.target.value)} />
        </label>
        <button type="submit">Send</button>
      </form>
    </article>
  );
};

export default SubmitForm;
