import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { TextField } from "@mui/material";
import UpdatedPopUp from "../Popup/UpdatedPopUp";
import PortalPopup from "../Popup/PortalPopup";
import "./Form.css";
import profilePicture from "../../../images/profile-img@2x.png";
import editProfileIcon from "../../../images/photocamerainterfacesymbolforbutton-1@2x.png";

const Form = () => {
  const [isUpdatedPopUpPopupOpen, setUpdatedPopUpPopupOpen] = useState(false);
  const [name, setName] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchName = async () => {
      try {
        const response = await fetch("http://localhost:7077/api/players/1");
        const data = await response.json();
        setName(data.name);
      } catch (error) {
        console.log("Error fetching name:", error);
      }
    };

    fetchName();
  }, []);

  const openUpdatedPopUpPopup = useCallback(() => {
    setUpdatedPopUpPopupOpen(true);
  }, []);

  const closeUpdatedPopUpPopup = useCallback(() => {
    setUpdatedPopUpPopupOpen(false);
  }, []);

  const onCancelButtonClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  return (
    <>
      <div className="form3">
        <div className="profile1">
          <div className="profile-picture-container">
            <img className="profile-picture-img" alt="" src={profilePicture} />
          </div>
          <div className="edit-profile-icon">
            <div className="edit-profile-icon-child" />
            <img
              className="photo-camera-interface-symbol-icon"
              alt=""
              src={editProfileIcon}
            />
          </div>
        </div>
        <div className="profile-details">
          <b className="name1">{name}</b>
          <div className="profile-type">Player</div>
        </div>
        <div className="subtitle1">
          <div className="customize-your-profile">
            Customize your profile to align with your career in the orchestra
            world!
          </div>
        </div>
        <form className="input-fields1">
          <TextField
            className="username-input"
            color="primary"
            variant="outlined"
            type="text"
            label="Name"
            size="medium"
            margin="none"
            required
          />
          <TextField
            className="username-input"
            color="primary"
            variant="outlined"
            type="text"
            label="Username"
            size="medium"
            margin="none"
            required
          />
          <TextField
            className="username-input"
            color="primary"
            variant="outlined"
            type="email"
            label="Email"
            size="medium"
            margin="none"
            required
          />
        </form>
        <button className="save-button" onClick={openUpdatedPopUpPopup}>
          <div className="button16">Save</div>
        </button>
        <button className="cancel-button1" onClick={onCancelButtonClick}>
          <div className="button17">Cancel</div>
        </button>
      </div>
      {isUpdatedPopUpPopupOpen && (
        <PortalPopup
          overlayColor="rgba(113, 113, 113, 0.3)"
          placement="Centered"
          onOutsideClick={closeUpdatedPopUpPopup}
        >
          <UpdatedPopUp onClose={closeUpdatedPopUpPopup} />
        </PortalPopup>
      )}
    </>
  );
};

export default Form;
