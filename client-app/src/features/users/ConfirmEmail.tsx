import React, { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { Button, Header, Icon, Segment } from "semantic-ui-react";
import agent from "../../app/api/agent";
import useQuery from "../../app/common/util/hooks,";
import { useStore } from "../../app/stores/store";
import LoginForm from "./LoginForm";

const ConfirmEmail = () => {
	const { modalStore } = useStore();
	const email = useQuery().get("email") as string;
	const token = useQuery().get("token") as string;

	const Status = {
		Verifying: "Verifying",
		Failed: "Failed",
		Success: "Success",
	};
	const [status, setStatus] = useState(Status.Verifying);
	const handleEmailRsend = () => {
		agent.Account.resendEmailConfirm(email)
			.then(() => {
				toast.success("Verification email resent - please check your email");
			})
			.catch((err) => console.log(err));
	};
	useEffect(() => {
		agent.Account.verifyEmail(token, email)
			.then(() => {
				setStatus(Status.Success);
			})
			.catch((err) => {
				setStatus(Status.Failed);
				console.log(err);
			});
	}, [Status.Failed, Status.Success, token, email]);

	const getBody = () => {
		switch (status) {
			case Status.Verifying:
				return <p>Verifying</p>;

			case Status.Failed:
				return (
					<div>
						<p>
							Verification failed.You can try resending the verify link to your
							email
						</p>
						<Button
							primary
							onClick={handleEmailRsend}
							size="huge"
							content="Resend email"
						/>
					</div>
				);

			case Status.Success:
				return (
					<div>
						<p>Email has been Verified - you can now login</p>
						<Button
							primary
							onClick={() => modalStore.openModal(<LoginForm />)}
							size="huge"
							content="Login"
						/>
					</div>
				);
		}
	};

	return (
		<Segment placeholder textAlign="center">
			<Header icon>
				<Icon name="envelope" />
				Email verification
			</Header>
			<Segment.Inline>{getBody()}</Segment.Inline>
		</Segment>
	);
};

export default ConfirmEmail;
