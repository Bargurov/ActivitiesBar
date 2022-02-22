import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Activity } from "../models/activity";

export default class ActivityStore {
	activityMap = new Map<string, Activity>();
	selectedActivity: Activity | undefined = undefined;
	editMode = false;
	loading = false;
	loadingInitial = true;

	constructor() {
		makeAutoObservable(this);
	}

	get ActivitiesByDate() {
		return Array.from(this.activityMap.values()).sort(
			(a, b) => Date.parse(a.date) - Date.parse(b.date),
		);
	}
	get groupOfActivities() {
		return Object.entries(
			this.ActivitiesByDate.reduce((activities, activity) => {
				const date = activity.date;
				activities[date] = activities[date]
					? [...activities[date], activity]
					: [activity];
				return activities;
			}, {} as { [key: string]: Activity[] }),
		);
	}

	loadActivities = async () => {
		this.loadingInitial = true;

		try {
			const activities = await agent.Activities.list();

			activities.forEach((activity) => {
				this.setActivity(activity);
			});
			this.setLoadingInitial(false);
		} catch (err) {
			console.log(err);
			this.setLoadingInitial(false);
		}
	};
	loadActivity = async (id: string) => {
		let activity = this.getActivity(id);
		if (activity) {
			this.selectedActivity = activity;
			return activity;
		} else {
			this.loading = true;
			try {
				activity = await agent.Activities.details(id);
				this.setActivity(activity);
				runInAction(() => {
					this.selectedActivity = activity;
				});
				this.setLoadingInitial(false);
				return activity;
			} catch (err) {
				console.log(err);
				this.setLoadingInitial(false);
			}
		}
	};
	private setActivity(activity: Activity) {
		activity.date = activity.date.split("T")[0];
		this.activityMap.set(activity.id, activity);
	}
	private getActivity = (id: string) => {
		return this.activityMap.get(id);
	};
	setLoadingInitial = (state: boolean) => {
		this.loadingInitial = state;
	};

	createActivity = async (activity: Activity) => {
		this.loading = true;
		try {
			await agent.Activities.create(activity);
			runInAction(() => {
				this.activityMap.set(activity.id, activity);
				this.selectedActivity = activity;
				this.editMode = false;
				this.loading = false;
			});
		} catch (err) {
			console.log(err);
			runInAction(() => {
				this.loading = false;
			});
		}
	};
	updateActivity = async (activity: Activity) => {
		this.loading = true;
		try {
			await agent.Activities.update(activity);
			runInAction(() => {
				this.activityMap.set(activity.id, activity);
				this.selectedActivity = activity;
				this.editMode = false;
				this.loading = false;
			});
		} catch (err) {
			console.log(err);
			runInAction(() => {
				this.loading = false;
			});
		}
	};

	deleteActivity = async (id: string) => {
		this.loading = true;
		try {
			await agent.Activities.delete(id);
			runInAction(() => {
				this.activityMap.delete(id);
				this.loading = false;
			});
		} catch (err) {
			console.log(err);
			runInAction(() => {
				this.loading = false;
			});
		}
	};
}
