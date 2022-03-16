import { makeAutoObservable, runInAction } from "mobx";
import { history } from "../..";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";

export default class UserStore {
	user: User | null = null;
	fbAccessToken: string | null = null;
	fbLoading = false;
	refreshTokenTimeout: any;

	constructor() {
		makeAutoObservable(this);
	}

	get isLoggedIn() {
		return !!this.user;
	}
	login = async (creds: UserFormValues) => {
		try {
			const user = await agent.Account.login(creds);
			store.commonStore.setToken(user.token);
			this.startRefreshTokenTimer(user);
			runInAction(() => (this.user = user));
			history.push("/activities");
			store.modalStore.closeModal();
		} catch (error) {
			throw error;
		}
	};
	logout = () => {
		store.commonStore.setToken(null);
		window.localStorage.removeItem("jwt");
		this.user = null;
		history.push("/");
	};

	getUser = async () => {
		try {
			const user = await agent.Account.current();
			store.commonStore.setToken(user.token);
			runInAction(() => (this.user = user));
			this.startRefreshTokenTimer(user);
		} catch (err) {
			console.log(err);
		}
	};

	register = async (creds: UserFormValues) => {
		try {
			await agent.Account.register(creds);
			history.push(`/account/registerSuccess?email=${creds.email}`);
			store.modalStore.closeModal();
		} catch (error) {
			throw error;
		}
	};
	setImage = (image: string) => {
		if (this.user) this.user.image = image;
	};
	setDisplayName = (name: string) => {
		if (this.user) this.user.displayName = name;
	};
	facebookLogin = () => {
		this.fbLoading = true;
		const apiLogin = (accessToken: string) => {
			agent.Account.FbLogin(accessToken)
				.then((user) => {
					store.commonStore.setToken(user.token);
					this.startRefreshTokenTimer(user);
					runInAction(() => {
						this.user = user;
						this.fbLoading = false;
					});
					history.push("/activities");
				})
				.catch((err) => {
					console.log(err);
					runInAction(() => (this.fbLoading = false));
				});
		};
		if (this.fbAccessToken) {
			apiLogin(this.fbAccessToken);
		} else {
			window.FB.login(
				(response) => {
					console.log(response);
					apiLogin(response.authResponse.accessToken);
				},
				{ scope: "public_profile,email" },
			);
		}
	};
	getFacebookLoginStatus = async () => {
		window.FB.getLoginStatus((response) => {
			console.log(response);
			if (response.authResponse === null) return;
			if ((response.status = "connected"))
				this.fbAccessToken = response.authResponse.accessToken;
		});
	};
	refreshToken = async () => {
		this.stopRefreshTokenTimeout();
		try {
			const user = await agent.Account.refreshToken();
			runInAction(() => {
				this.user = user;
				store.commonStore.setToken(user.token);
				this.startRefreshTokenTimer(user);
			});
		} catch (err) {
			console.log(err);
		}
	};
	private startRefreshTokenTimer(user: User) {
		this.refreshTokenTimeout = setTimeout(this.refreshToken, 300000);
	}
	private stopRefreshTokenTimeout() {
		clearTimeout(this.refreshTokenTimeout);
	}
}
