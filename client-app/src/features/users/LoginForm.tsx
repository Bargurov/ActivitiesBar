import { ErrorMessage, Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Header, Label } from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInputs";
import { useStore } from "../../app/stores/store";

const LoginForm = () => {
	const { userStore } = useStore();

	return (
		<Formik
			initialValues={{ email: "", password: "", error: null }}
			onSubmit={(values, { setErrors }) =>
				userStore
					.login(values)
					.catch((error) => setErrors({ error: "Invalid email or password" }))
			}
		>
			{({ handleSubmit, isSubmitting, errors }) => (
				<Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
					<Header
						as="h2"
						content="Login to ActivitiesBar"
						color="teal"
						textAlign="center"
					/>
					<MyTextInput name="email" placeholder="email" />
					<MyTextInput name="password" placeholder="password" type="password" />
					<ErrorMessage
						name="error"
						render={() => (
							<Label
								style={{ marginBottom: 10 }}
								basic
								color="red"
								content={errors.error}
							/>
						)}
					/>
					<Button
						positive
						content="Login"
						type="submit"
						fluid
						loading={isSubmitting}
					/>
				</Form>
			)}
		</Formik>
	);
};

export default observer(LoginForm);