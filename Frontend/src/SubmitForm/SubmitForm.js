import React, { useState } from "react";
import Spinner from "../Components/Spinner/Spinner";

const SubmitForm = ({ onSubmitted }) => {
  const [username, setUsername] = useState("");
  const [code, setCode] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const [successfulPost, setSuccessfulPost] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setSuccessfulPost(false);
    setLoading(true);
    fetch(`https://localhost:5050/NewCodeEntry?username=${username}&code=${code}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => {
        if (!res.ok) {
          throw res;
        }
      })
      .then(() => {
        setErrorMessage("");
        setUsername("");
        setCode("");
        setSuccessfulPost(true);
      })
      .catch((error) => {
        error.text().then((errorText) => {
          setErrorMessage(errorText || "Something went wrong!");
        });
      })
      .finally(() => {
        setLoading(false);
        onSubmitted();
      });
  };
  return (
    <article>
      {successfulPost && <div>Registered!</div>}
      {loading ? (
        <Spinner />
      ) : (
        <form onSubmit={handleSubmit} id="submit-form" aria-describedby="submit-form__message">
          {errorMessage && <p id="submit-form__message">{errorMessage}</p>}
          <label>
            Name:
            <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} required />
          </label>
          <label>
            Code:
            <input type="text" value={code} onChange={(e) => setCode(e.target.value)} required />
          </label>
          <button type="submit" disabled={code === "" || username === ""}>
            Send
          </button>
        </form>
      )}
    </article>
  );
};

export default SubmitForm;
