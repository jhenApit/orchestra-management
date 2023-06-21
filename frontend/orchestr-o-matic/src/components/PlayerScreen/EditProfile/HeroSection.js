import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./HeroSection.css";
import profilePictureProfile from "../../../images/profile-img1@2x.png";

const HeroSection1 = () => {
  const navigate = useNavigate();
  const [name2, setName2] = useState("");

  const onEditButtonClick = useCallback(() => {
    navigate("/edit-profile");
  }, [navigate]);

  useEffect(() => {
    const fetchName = async () => {
      try {
        const response = await fetch("http://localhost:7077/api/players/1");
        const data = await response.json();
        setName2(data.name);
      } catch (error) {
        console.log("Error fetching name:", error);
      }
    };

    fetchName();
  }, []);

  return (
    <div className="hero-section1">
      <div className="hero-contents">
        <img className="profile-img-icon1" alt="" src={profilePictureProfile} />
        <div className="hero-right-contents">
          <div className="account-detail">
            <b className="name2">{name2}</b>
            <div className="account-type1">Player</div>
          </div>
          <button className="edit-button" onClick={onEditButtonClick}>
            <div className="edit-profile1">Edit Profile</div>
          </button>
        </div>
      </div>
    </div>
  );
};

export default HeroSection1;
