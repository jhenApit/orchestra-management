import React, { useContext, useState } from "react";
import "../../styles/Conductor/ConductorProfile.css";
import * as HiIcons from "react-icons/hi";
import * as MdIcons from "react-icons/md";
import Popup from "../../components/ConductorEditProfile";
import header from "../../images/music-header.jpeg";
import user from "../../images/user.png";
import Navbar from "../../components/Navbar";
import { UserContext } from "../../context/user";

function Profile() {
  const { user: userProfile } = useContext(UserContext);
  const [buttonPopup, setButtonPopup] = useState(false);

  return (
    <>
      <Navbar />
      <div className="profile">
        <img src={header} alt="profile-header" className="header-picture" />
        <div className="overlay">
          <div className="overlay-header">
            <img src={user} alt="user-profile" className="user-profile" />
            <div className="user-name">
              {userProfile?.username}
              <br />
              <div className="user-position"> {userProfile?.role}</div>
            </div>
            <button
              className="edit-profile-btn"
              onClick={() => setButtonPopup(true)}
            >
              Edit profile
            </button>
            <Popup trigger={buttonPopup} setTrigger={setButtonPopup}>
              <h3>Edit Profile</h3>
            </Popup>
          </div>
          <div className="profile-infos">
            <div className="personal-infos">
              <div className="personal-info-header">Personal Informations</div>
              <div className="personal-info-body">
                <div className="column">
                  <div className="name">
                    <HiIcons.HiUser className="name-icon" />
                    Username
                    <br />
                    <br />
                    {userProfile?.username}
                  </div>
                </div>
                <div className="column">
                  <div className="email">
                    <MdIcons.MdEmail className="email-icon" />
                    Email
                    <br />
                    <br />
                    {userProfile?.email}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default Profile;
