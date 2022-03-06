import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Header, Item, Segment, Image, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { Link } from "react-router-dom";
import { format } from "date-fns";
import { useStore } from "../../../app/stores/store";

const activityImageStyle = {
	filter: "brightness(30%)",
};

const activityImageTextStyle = {
	position: "absolute",
	bottom: "5%",
	left: "5%",
	width: "100%",
	height: "auto",
	color: "white",
};

interface Props {
	activity: Activity;
}

const ActivityDetailedHeader = ({ activity }: Props) => {
	const {
		activityStore: { updateAttendance, loading, cancelActivityToggle },
	} = useStore();
	return (
		<Segment.Group>
			{activity.isCancelled && (
				<Label
					style={{ position: "absolute", zIndex: 100, left: -14, top: 20 }}
					ribbon
					color="red"
					content="Cancelled"
				/>
			)}
			<Segment basic attached="top" style={{ padding: "0" }}>
				<Image
					src={`/assets/categoryImages/${activity.category}.jpg`}
					fluid
					style={activityImageStyle}
				/>
				<Segment style={activityImageTextStyle} basic>
					<Item.Group>
						<Item>
							<Item.Content>
								<Header
									size="huge"
									content={activity.title}
									style={{ color: "white" }}
								/>
								<p>{format(activity.date!, "ddd MMM yyyy")}</p>
								<p>
									Hosted by{" "}
									<strong>
										<Link to={`/profiles/${activity.host?.displayName}`}>
											{activity.host?.displayName}
										</Link>
									</strong>
								</p>
							</Item.Content>
						</Item>
					</Item.Group>
				</Segment>
			</Segment>
			<Segment clearing attached="bottom">
				{activity.isHost ? (
					<>
						<Button
							color={activity.isCancelled ? "green" : "red"}
							floated="left"
							basic
							content={
								activity.isCancelled
									? "Re-activate Activity"
									: "Cancel Activity"
							}
							onClick={cancelActivityToggle}
							loading={loading}
						/>
						<Button
							disabled={activity.isCancelled}
							color="orange"
							floated="right"
							as={Link}
							to={`/manage/${activity.id}`}
						>
							Manage Event
						</Button>
					</>
				) : activity.isGoing ? (
					<Button onClick={updateAttendance} loading={loading}>
						Cancel attendance
					</Button>
				) : (
					<Button
						color="teal"
						onClick={updateAttendance}
						loading={loading}
						disabled={activity.isCancelled}
					>
						{" "}
						Join Activity
					</Button>
				)}
			</Segment>
		</Segment.Group>
	);
};

export default observer(ActivityDetailedHeader);