import React, { useState } from "react";
import Spinner from "../Components/Spinner/Spinner";
import "./SubmitForm.css";

const SubmitForm = ({ onSubmitted }) => {
  const [username, setUsername] = useState("");
  const [time, setTime] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [username2, setUsername2] = useState("");
  const [time2, setTime2] = useState("");
  const [email2, setEmail2] = useState("");
  const [phoneNumber2, setPhoneNumber2] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const [successfulPost, setSuccessfulPost] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setSuccessfulPost(false);
    setErrorMessage("");
    setLoading(true);
    fetch(`https://localhost:5050/NewTimeEntry?username=${username}&email=${email}&phoneNumber=${phoneNumber}`, {
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
        setEmail("");
        setPhoneNumber("");
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

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  }

  const handlePhoneNumberChange = (e) => {
    setPhoneNumber(e.target.value);
  }

  const handleUserNameChange2 = (e) => {
    setSuccessfulPost(false);
    setUsername2(e.target.value);
  };

  const handleTimeChange2 = (e) => {
    setTime2(e.target.value);
  };

  const handleEmailChange2 = (e) => {
    setEmail2(e.target.value);
  }

  const handlePhoneNumberChange2 = (e) => {
    setPhoneNumber2(e.target.value);
  }

  return (
    <article className="submit-form__wrapper">
      {loading ? (
        <Spinner />
      ) : (
        <form className="submit-form" onSubmit={handleSubmit} id="submit-form" aria-describedby="submit-form__message">
          <fieldset className="submit-form__fieldset">
            <ul>
              <li>
                <label className="submit-form__label">Name</label>
                <input className="submit-form__input" type="text" value={username} onChange={handleUserNameChange} required />
              </li>
              <li>
                <label className="submit-form__label">Time</label>
                <input className="submit-form__input" type="text" value={time} onChange={handleTimeChange} required />
              </li>
              <li>
                <label className="submit-form__label">Email</label>
                <input className="submit-form__input" type="text" value={email} onChange={handleEmailChange} required />
              </li>
              <li>
                <label className="submit-form__label">Phone number</label>
                <input className="submit-form__input" type="text" value={phoneNumber} onChange={handlePhoneNumberChange} required />
              </li>
            </ul>
          </fieldset>
          <fieldset className="submit-form__fieldset">
            <ul>
              <li>
                <label className="submit-form__label">Name</label>
                <input className="submit-form__input" type="text" value={username2} onChange={handleUserNameChange2} required />
              </li>
              <li>
                <label className="submit-form__label">Time</label>
                <input className="submit-form__input" type="text" value={time2} onChange={handleTimeChange2} required />
              </li>
              <li>
                <label className="submit-form__label">Email</label>
                <input className="submit-form__input" type="text" value={email2} onChange={handleEmailChange2} required />
              </li>
              <li>
                <label className="submit-form__label">Phone number</label>
                <input className="submit-form__input" type="text" value={phoneNumber2} onChange={handlePhoneNumberChange2} required />
              </li>
            </ul>
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
