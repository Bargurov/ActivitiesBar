import React, { useEffect, useState } from "react";
import { Container } from "semantic-ui-react";
import { Activity } from "../models/activity";
import Navbar from "./navbar";
import ActivityDashBoard from "../../features/activities/dashboard/ActivityDashBoard";
import { v4 as uuid } from "uuid";
import agent from "../api/agent";
import LoadingComponent from "./LoadingComponent";

function App() {
	const [activities, setActivities] = useState<Activity[]>([]);
	const [selectedActivity, setSelectedActivity] = useState<
		Activity | undefined
	>(undefined);
	const [editMode, setEditMode] = useState(false);
	const [loading, setLoading] = useState(true);
	const [submit, setSubmit] = useState(false);

	useEffect(() => {
		agent.Activities.list().then((response) => {
			let activities: Activity[] = [];
			response.forEach((activity) => {
				activity.date = activity.date.split("T")[0];
				activities.push(activity);
			});
			setActivities(activities);
			setLoading(false);
		});
	}, []);

	const handleSelectActivity = (id: string) => {
		setSelectedActivity(activities.find((activity) => activity.id === id));
	};
	const handleCancelSelectedActivity = () => {
		setSelectedActivity(undefined);
	};

	const handleFormOpen = (id?: string) => {
		id ? handleSelectActivity(id) : handleCancelSelectedActivity();
		setEditMode(true);
	};
	const handleFormClose = () => {
		setEditMode(false);
	};

	const handleCreateOrEditActivity = (activity: Activity) => {
		setSubmit(true);
		if (activity.id) {
			agent.Activities.update(activity).then(() => {
				setActivities([
					...activities.filter((x) => x.id !== activity.id),
					activity,
				]);
				setSelectedActivity(activity);
				setEditMode(false);
				setSubmit(false);
			});
		} else {
			activity.id = uuid();
			agent.Activities.create(activity).then(() => {
				setActivities([...activities, activity]);
				setSelectedActivity(activity);
				setEditMode(false);
				setSubmit(false);
			});
		}
	};

	const handleDeleteActivity = (id: string) => {
		setSubmit(true);
		agent.Activities.delete(id).then(() => {
			setActivities([...activities.filter((x) => x.id !== id)]);
			setSubmit(false);
		});
	};

	if (loading) return <LoadingComponent content="Loading App" />;

	return (
		<>
			<Navbar openForm={handleFormOpen} />
			<Container style={{ marginTop: "7em" }}>
				<ActivityDashBoard
					activities={activities}
					selectedActivity={selectedActivity}
					selectActivity={handleSelectActivity}
					cancelSelectedActivity={handleCancelSelectedActivity}
					editMode={editMode}
					openForm={handleFormOpen}
					closeForm={handleFormClose}
					createOrEdit={handleCreateOrEditActivity}
					deleteActivity={handleDeleteActivity}
					submitting={submit}
				/>
			</Container>
		</>
	);
}

export default App;
