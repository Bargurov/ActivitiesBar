import { observer } from "mobx-react-lite";
import React from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, Image, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";

const NavBar = () => {
	const {
		userStore: { user, logout, isLoggedIn },
	} = useStore();
	return (
		<Menu inverted fixed="top">
			<Container>
				<Menu.Item as={NavLink} exact to="/" header>
					<img
						src="/assets/logo.png"
						alt="logo"
						style={{ marginRight: "10px" }}
					/>
					ActivitiesBar
				</Menu.Item>
				{isLoggedIn && (
					<>
						<Menu.Item as={NavLink} to="/activities" name="Activities" />
						<Menu.Item>
							<Button
								as={NavLink}
								to="/createActivity"
								positive
								content="Create Activity"
							/>
						</Menu.Item>
						<Menu.Item position="right">
							<Image
								src={user?.image || "assets/user.png"}
								avatar
								spaced="right"
							/>
							<Dropdown pointing="top left" text={user?.displayName}>
								<Dropdown.Menu>
									<Dropdown.Item
										as={Link}
										to={`/profiles/${user?.username}`}
										text="My Profile"
										icon="user"
									/>
									<Dropdown.Item text="Logout" icon="power" onClick={logout} />
								</Dropdown.Menu>
							</Dropdown>
						</Menu.Item>
					</>
				)}
			</Container>
		</Menu>
	);
};

export default observer(NavBar);
