import React, { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../../context/user";
import { editUser } from "../../api/user";
import * as AiIcons from "react-icons/ai";
import "./ConductorEditProfile.css";

function EditProfile_Popup(props) {
  const { user: userProfile, updateUser } = useContext(UserContext);
  const [username, setUsername] = useState();
  const [email, setEmail] = useState();
  const [password, setPassword] = useState("");

  useEffect(() => {
    setUsername(userProfile?.username);
    setEmail(userProfile?.email);
  }, [userProfile]);

  const saveEdit = async (e) => {
    e.preventDefault();
    let data = {
      username,
      email,
      password,
      role: userProfile?.role === "Conductor" ? 1 : 2,
    };

    await editUser(userProfile?.id, data);
    updateUser();
    setUsername("");
    setEmail("");
    setPassword("");
    props.setTrigger(false);
  };

  return props.trigger ? (
    <div className="edit-profile-popup">
      <div className="edit-profile-popup-inner">
        <div className="edit-profile-popup-header">
          <Link
            to="#"
            className="profile-close-btn"
            onClick={() => props.setTrigger(false)}
          >
            <AiIcons.AiFillCloseCircle />
          </Link>
          {props.children}
        </div>
        <div className="edit-profile-popup-infos">
          <form>
            <div className="column">
              <label>Username</label>
              <input
                type="text"
                required
                className="name-input-wrapper"
                onChange={(e) => setUsername(e.target.value)}
                value={username}
              ></input>
              <br />
              <label>Email</label>
              <input
                type="email"
                required
                className="email-input-wrapper"
                onChange={(e) => setEmail(e.target.value)}
                value={email}
              ></input>
              <label>Confirm or Change Password</label>
              <input
                type="password"
                required
                className="password-input-wrapper"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              ></input>
              <br />
            </div>
          </form>
          <div className="buttons">
            <button
              type="button"
              className="profile-cancel-btn"
              onClick={() => props.setTrigger(false)}
            >
              Cancel
            </button>
            <button className="profile-save-btn" onClick={saveEdit}>
              Save
            </button>
          </div>
        </div>
      </div>
    </div>
  ) : (
    ""
  );
}

export default EditProfile_Popup;
