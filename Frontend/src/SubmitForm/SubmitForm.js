import React, { useState } from "react";
import Spinner from "../Components/Spinner/Spinner";
import "./SubmitForm.css";

const SubmitForm = ({ onSubmitted }) => {
  const [username, setUsername] = useState("");
  const [time, setTime] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const [successfulPost, setSuccessfulPost] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setSuccessfulPost(false);
    setErrorMessage("");
    setLoading(true);
    fetch(`https://localhost:5050/NewTimeEntry?username=${username}&time=${time}`, {
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
        setTime("");
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

  const handleUserNameChange = (e) => {
    setSuccessfulPost(false);
    setUsername(e.target.value);
  };

  const handleTimeChange = (e) => {
    setTime(e.target.value);
  };

  return (
    <article className="submit-form__wrapper">
      {loading ? (
        <Spinner />
      ) : (
        <form className="submit-form" onSubmit={handleSubmit} id="submit-form" aria-describedby="submit-form__message">
          <fieldset className="submit-form__fieldset">
            <label className="submit-form__label">Name</label>
            <input
              className="submit-form__input"
              type="text"
              value={username}
              onChange={handleUserNameChange}
              required
            />
          </fieldset>
          <fieldset className="submit-form__fieldset">
            <label className="submit-form__label">Time</label>
            <input className="submit-form__input" type="text" value={time} onChange={handleTimeChange} required />
          </fieldset>
          {errorMessage && (
            <p className="submit-form__error" id="submit-form__message">
              {errorMessage}
            </p>
          )}
          {successfulPost && <p className="submit-form__success">Registered!</p>}
          <button type="submit" disabled={time === "" || username === ""} className="submit-form__submit">
            Send
          </button>
        </form>
      )}
    </article>
  );
};

export default SubmitForm;
