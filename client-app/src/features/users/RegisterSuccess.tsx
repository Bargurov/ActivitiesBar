import React from "react";
import { toast } from "react-toastify";
import { Button, Header, Icon, Segment } from "semantic-ui-react";
import agent from "../../app/api/agent";
import useQuery from "../../app/common/util/hooks,";

const RegisterSuccess = () => {
	const email = useQuery().get("email") as string;

	const handleEmailRsend = () => {
		agent.Account.resendEmailConfirm(email)
			.then(() => {
				toast.success("Verification email resent - please check your email");
			})
			.catch((err) => console.log(err));
	};
	return (
		<Segment placeholder textAlign="center">
			<Header icon color="green">
				<Icon name="check" />
				Successfully registered!
			</Header>
			<p>Please check your email for Verification email</p>
			{email && (
				<>
					<p>Didn't receive the email? Click the button below to resend</p>
					<Button
						primary
						onClick={handleEmailRsend}
						content="Resend email"
						size="huge"
					/>
				</>
			)}
		</Segment>
	);
};

export default RegisterSuccess;
