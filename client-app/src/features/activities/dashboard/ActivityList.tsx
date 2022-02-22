import { observer } from "mobx-react-lite";
import { Fragment } from "react";
import {  Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import ActivityListItem from "./ActivityListItem";

const ActivityList = () => {
	const { activityStore } = useStore();
	const { groupOfActivities } = activityStore;

	return (
		<>
			{groupOfActivities.map(([group, activities]) => (
				<Fragment key={group}>
					<Header sub color="teal">
						{group}
					</Header>
					{activities.map((activity) => (
						<ActivityListItem activity={activity} key={activity.id} />
					))}
				</Fragment>
			))}
		</>
	);
};

export default observer(ActivityList);
