import { Container } from "semantic-ui-react";
import Navbar from "./navbar";
import ActivityDashBoard from "../../features/activities/dashboard/ActivityDashBoard";
import { observer } from "mobx-react-lite";
import { Route, Switch, useLocation } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import TestErrors from "../../features/errors/TestError";
import { ToastContainer } from "react-toastify";
import NotFound from "../../features/errors/NotFound";

function App() {
	const location = useLocation();
	return (
		<>
			<ToastContainer position="bottom-right" hideProgressBar />
			<Route exact path="/" component={HomePage} />
			<Route
				path={"/(.+)"}
				render={() => (
					<>
						<Navbar />
						<Container style={{ marginTop: "7em" }}>
							<Switch>
								<Route exact path="/activities" component={ActivityDashBoard} />
								<Route path="/activities/:id" component={ActivityDetails} />
								<Route
									path={["/createActivity", "/editActivity/:id"]}
									component={ActivityForm}
									key={location.key}
								/>
								<Route path="/errors" component={TestErrors} />
								<Route component={NotFound} />
							</Switch>
						</Container>
					</>
				)}
			/>
		</>
	);
}

export default observer(App);
