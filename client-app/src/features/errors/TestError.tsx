import React, { useState } from "react";
import { Button, Header, Segment } from "semantic-ui-react";
import axios from "axios";
import ValidationErrors from "./ValidationErrors";

const TestErrors = () => {
	const baseUrl = "http://localhost:5000/api/";
	const [errors, setErrors] = useState(null);

	const handleNotFound = () => {
		axios
			.get(baseUrl + "buggy/not-found")
			.catch((err) => console.log(err.response));
	};

	const handleBadRequest = () => {
		axios
			.get(baseUrl + "buggy/bad-request")
			.catch((err) => console.log(err.response));
	};

	const handleServerError = () => {
		axios
			.get(baseUrl + "buggy/server-error")
			.catch((err) => console.log(err.response));
	};

	const handleUnauthorised = () => {
		axios
			.get(baseUrl + "buggy/unauthorised")
			.catch((err) => console.log(err.response));
	};

	const handleBadGuid = () => {
		axios.get(baseUrl + "activities/notaguid").catch((err) => console.log(err));
	};

	const handleValidationError = () => {
		axios.post(baseUrl + "activities", {}).catch((err) => setErrors(err));
	};

	return (
		<>
			<Header as="h1" content="Test Error component" />
			<Segment>
				<Button.Group widths="7">
					<Button onClick={handleNotFound} content="Not Found" basic primary />
					<Button
						onClick={handleBadRequest}
						content="Bad Request"
						basic
						primary
					/>
					<Button
						onClick={handleValidationError}
						content="Validation Error"
						basic
						primary
					/>
					<Button
						onClick={handleServerError}
						content="Server Error"
						basic
						primary
					/>
					<Button
						onClick={handleUnauthorised}
						content="Unauthorised"
						basic
						primary
					/>
					<Button onClick={handleBadGuid} content="Bad Guid" basic primary />
				</Button.Group>
			</Segment>
			{errors && <ValidationErrors errors={errors} />}
		</>
	);
};
export default TestErrors;
