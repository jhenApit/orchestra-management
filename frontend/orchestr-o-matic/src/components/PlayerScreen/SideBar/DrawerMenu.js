import { useCallback, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import "./DrawerMenu.css";
import dashboardIcon from "../../../images/dashboardqueue@2x.png";
import logoutIcon from "../../../images/logout-icon@2x.png";
import orchestraIcon from "../../../images/orchestraqueue@2x.png";
import concertIcon from "../../../images/concertqueue@2x.png";
import profileIcon from "../../../images/profile-icon.svg";
import profilePicture from "../../../images/profile-img@2x.png";
import { UserContext } from "../../../context/user";

const DrawerMenu = ({ onClose }) => {
  const navigate = useNavigate();
  const { user: userProfile, logoutUser } = useContext(UserContext);

  useEffect(() => {
    const scrollAnimElements = document.querySelectorAll(
      "[data-animate-on-scroll]"
    );
    const observer = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (entry.isIntersecting || entry.intersectionRatio > 0) {
            const targetElement = entry.target;
            targetElement.classList.add("animate");
            observer.unobserve(targetElement);
          }
        }
      },
      {
        threshold: 0.15,
      }
    );

    for (let i = 0; i < scrollAnimElements.length; i++) {
      observer.observe(scrollAnimElements[i]);
    }

    return () => {
      for (let i = 0; i < scrollAnimElements.length; i++) {
        observer.unobserve(scrollAnimElements[i]);
      }
    };
  }, []);

  const onProfileItemClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  const onConcertItemClick = useCallback(() => {
    navigate("/my-concerts");
  }, [navigate]);

  const onOrchestraItemClick = useCallback(() => {
    navigate("/my-orchestra");
  }, [navigate]);

  const onDashboardItemClick = useCallback(() => {
    navigate("/dashboard");
  }, [navigate]);

  return (
    <div className="drawer-menu" data-animate-on-scroll>
      <div className="menu">
        <div className="profile-picture">
          <div className="profile-pic-frame">
            <img className="profile-img-icon" alt="" src={profilePicture} />
          </div>
        </div>
        <div className="player-detail">
          <b className="player-name">{userProfile?.username}</b>
          <div className="account-type">Player</div>
        </div>
        <div className="drawer-menu-items">
          <button className="profile-item" onClick={onProfileItemClick}>
            <img className="profile-icon" alt="" src={profileIcon} />
            <div className="profile">Profile</div>
          </button>
          <button className="dashboard-item" onClick={onDashboardItemClick}>
            <img className="dashboard-queue-icon" alt="" src={dashboardIcon} />
            <div className="profile">Dashboard</div>
          </button>
          <button className="dashboard-item" onClick={onConcertItemClick}>
            <img className="dashboard-queue-icon" alt="" src={concertIcon} />
            <div className="profile">Concerts</div>
          </button>
          <button className="orchestra-item" onClick={onOrchestraItemClick}>
            <img className="dashboard-queue-icon" alt="" src={orchestraIcon} />
            <div className="profile">Orchestras</div>
          </button>
        </div>
      </div>

      <button
        className="logout-item"
        onClick={() => {
          logoutUser();
        }}
      >
        <img className="logout-icon" alt="" src={logoutIcon} />
        <div className="logout">Logout</div>
      </button>
    </div>
  );
};

export default DrawerMenu;
