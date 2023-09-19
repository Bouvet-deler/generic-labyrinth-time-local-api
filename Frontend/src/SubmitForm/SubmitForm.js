import React, { useState } from "react";
import Spinner from "../Components/Spinner/Spinner";
import "./SubmitForm.css";

const SubmitForm = ({ onSubmitted, readyToRegister, setReadyToRegister }) => {
	const [username, setUsername] = useState("");
	const [email, setEmail] = useState("");
	const [time, setTime] = useState("");
	const [phoneNumber, setPhoneNumber] = useState("");
	const [username2, setUsername2] = useState("");
	const [time2, setTime2] = useState("");
	const [email2, setEmail2] = useState("");
	const [phoneNumber2, setPhoneNumber2] = useState("");
	const [errorMessage, setErrorMessage] = useState("");
	const [loading, setLoading] = useState(false);
	const [successfulPost, setSuccessfulPost] = useState(false);
	const [isSubmittingForm, setIsSubmittingForm] = useState(false);

	const handleTime = (() => {
		setIsSubmittingForm(true);
		handleAutofillTime();
		handleAutofillTime2();
	});

	const handleSend = (() => {
		if (successfulPost) {
			setIsSubmittingForm(false);
			setReadyToRegister(false);
		}
	})

	const handleSubmit = (e) => {
		e.preventDefault();
		setSuccessfulPost(false);
		setErrorMessage("");
		setLoading(true);
		fetch(`https://localhost:5050/NewTimeEntry?username=${username}&email=${email}&phoneNumber=${phoneNumber}&username2=${username2}&email2=${email2}&phoneNumber2=${phoneNumber2}`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
		}).then((res) => {
			if (!res.ok) {
				throw res;
			}
		}).then(() => {
			setErrorMessage("");
			setUsername("");
			setEmail("");
			setPhoneNumber("");
			setUsername2("");
			setEmail2("");
			setPhoneNumber2("");
			setSuccessfulPost(true);
			setIsSubmittingForm(false);
		}).catch((error) => {
			error.text().then((errorText) => {
				setErrorMessage(errorText || "Something went wrong!");
			});
		}).finally(() => {
			setLoading(false);
			onSubmitted();
		});
	};

	const handleAutofillTime = () => {
		fetch('https://localhost:5050/sendTimePlayer1')
			.then(async (res) => {
				if (!res.ok) {
					throw res;
				}
				const runTime = await res.text().then((text) => {
					return text.toString();
				})
				setTime(runTime);
			});
	};

	const handleAutofillTime2 = () => {
		fetch('https://localhost:5050/sendTimePlayer2')
			.then(async (res) => {
				if (!res.ok) {
					throw res;
				}
				const runTime = await res.text().then((text) => {
					return text.toString();
				})
				setTime2(runTime);
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
			) : (!isSubmittingForm ? (readyToRegister ? <button className="submit-form__submit" onClick={handleTime}>Register score</button> : null) : (
				<form className="submit-form" onSubmit={handleSubmit} id="submit-form" aria-describedby="submit-form__message">
					<div className="submit-form__fieldset-container">
						<fieldset className="submit-form__fieldset">

							<label className="submit-form__label" for="name1">Full Name</label>
							<input className="submit-form__input" type="text" id="name1" value={username} onChange={handleUserNameChange} required placeholder="Player1" />

							<label className="submit-form__label" for="time1">Time</label>
							<input className="submit-form__input" type="text" id="time1" value={time} onChange={handleTimeChange} required readOnly placeholder="00:00:000" />

							<label className="submit-form__label" for="email1">Email Address</label>
							<input className="submit-form__input" id="email1" type="email" value={email} onChange={handleEmailChange} required placeholder="email@example.com" />

							<label className="submit-form__label" for="phone1">Phone number</label>
							<input className="submit-form__input" type="text" id="phone1" value={phoneNumber} onChange={handlePhoneNumberChange} required />

						</fieldset>
						<fieldset className="submit-form__fieldset">

							<label className="submit-form__label" for="name2">Full Name</label>
							<input className="submit-form__input" type="text" id="name2" value={username2} onChange={handleUserNameChange2} required placeholder="Player2" />

							<label className="submit-form__label" for="time2">Time</label>
							<input className="submit-form__input" id="time2" type="text" value={time2} onChange={handleTimeChange2} required readOnly placeholder="00:00:000" />

							<label className="submit-form__label" for="email2">Email Address</label>
							<input className="submit-form__input" type="email" id="email2" value={email2} onChange={handleEmailChange2} required placeholder="email@example.com" />

							<label className="submit-form__label" for="phone2">Phone number</label>
							<input className="submit-form__input" type="text" id="phone2" value={phoneNumber2} onChange={handlePhoneNumberChange2} required />

						</fieldset>
					</div>
					{errorMessage && (
						<p className="submit-form__error" id="submit-form__message">
							{errorMessage}
						</p>
					)}
					<button type="submit" className="submit-form__submit" onClick={handleSend}>
						Send
					</button>
				</form>
			))
			}
		</article>
	);
};

export default SubmitForm;
