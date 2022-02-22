import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import ActivityFilters from "./ActivityFilters";
import ActivityList from "./ActivityList";

const ActivityDashBoard = () => {
	const { activityStore } = useStore();
	const { loadActivities, activityMap } = activityStore;

	useEffect(() => {
		if (activityMap.size <= 1) loadActivities();
	}, [activityMap, loadActivities]);

	if (activityStore.loadingInitial)
		return <LoadingComponent content="Loading App" />;

	return (
		<Grid>
			<Grid.Column width="10">
				<ActivityList />
			</Grid.Column>
			<Grid.Column width="6">
				<ActivityFilters/>
			</Grid.Column>
		</Grid>
	);
};

export default observer(ActivityDashBoard);
