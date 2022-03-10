import { observer } from "mobx-react-lite";
import React from "react";
import Calendar from "react-calendar";
import { Header, Menu } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

const ActivityFilters = () => {
	const {
		activityStore: { predicate, setPrediacte },
	} = useStore();

	return (
		<>
			<Menu vertical size="large" style={{ width: "100%", marginTop: 25 }}>
				<Header icon="filter" attached color="teal" content="Filters" />
				<Menu.Item
					content="All Activities"
					active={predicate.has("all")}
					onClick={() => setPrediacte("all", "true")}
				/>
				<Menu.Item
					content="I'm going"
					active={predicate.has("isGoing")}
					onClick={() => setPrediacte("isGoing", "true")}
				/>
				<Menu.Item
					content="I'm hosting"
					active={predicate.has("isHost")}
					onClick={() => setPrediacte("isHost", "true")}
				/>
			</Menu>
			<Header />
			<Calendar
				calendarType="Hebrew"
				onChange={(date) => setPrediacte("startDate", date as Date)}
				value={predicate.get("startDate") || new Date()}
			/>
		</>
	);
};

export default observer(ActivityFilters);
