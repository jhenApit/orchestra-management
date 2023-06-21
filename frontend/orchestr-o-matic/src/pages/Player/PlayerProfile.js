import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../../components/PlayerScreen/NavigationBars/Navbar(Player Profile)";
import HeroSection1 from "../../components/PlayerScreen/EditProfile/HeroSection";
import ConcertSection1 from "../../components/PlayerScreen/MyConcert/ConcertSection";
import instrumentCardIcon from "../../images/difference-and-comparison-between-coffee-and-tea-facebook-post-1@2x.png";
import sectionCardIcon from "../../images/difference-and-comparison-between-coffee-and-tea-facebook-post-1-1@2x.png";
import orchestraCardImgProfile1 from "../../images/orchestra-img4@2x.png";

import "../../styles/Player/PlayerProfile.css";

const MyProfile = () => {
  const navigate = useNavigate();

  const onMyProfileTextClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  const onInstrumentCardClick = useCallback(() => {}, []);

  const onSectionCardClick = useCallback(() => {}, []);

  const [playerData, setPlayerData] = useState(null);

  useEffect(() => {
    const fetchPlayerData = async () => {
      try {
        const response = await fetch("http://localhost:7077/api/players/2");
        const data = await response.json();
        setPlayerData(data);
      } catch (error) {
        console.log("Error fetching player data:", error);
      }
    };

    fetchPlayerData();
  }, []);

  return (
    <div className="my-profile">
      <Navbar1
        myConcert="My Profile"
        onMyConcertTextClick={onMyProfileTextClick}
      />
      <HeroSection1 />
      <div className="player-details-section">
        <div className="instrument-and-section-section">
          <i className="instrument-section">{`Instrument & Section `}</i>
          <div className="is-container">
            <button className="instrument-card" onClick={onInstrumentCardClick}>
              <img
                className="difference-and-comparison-betw"
                alt=""
                src={instrumentCardIcon}
              />
              <div className="instrument-details">
                <div className="instrument-details-inner">
                  <div className="instrument-wrapper">
                    <div className="instrument">{playerData?.instrument}</div>
                  </div>
                </div>
              </div>
            </button>
            <button className="section-card" onClick={onSectionCardClick}>
              <img
                className="difference-and-comparison-betw"
                alt=""
                src={sectionCardIcon}
              />
              <div className="section-details">
                <div className="section-details-child">
                  <div className="section-wrapper">
                    <div className="section">{playerData?.section}</div>
                  </div>
                </div>
              </div>
            </button>
          </div>
        </div>
        <ConcertSection1 />
        <div className="orchestra-section1">
          <i className="instrument-section">{`Orchestras `}</i>
          <div className="orchestras-container">
            <img
              className="orchestra-img-icon2"
              alt=""
              src={orchestraCardImgProfile1}
            />
            <div className="orchestra-details6">
              <div className="frame"></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MyProfile;
