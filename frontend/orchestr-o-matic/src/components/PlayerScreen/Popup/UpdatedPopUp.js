import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import "./UpdatedPopUp.css";
import updatePopupIcon from "../../../images/11@2x.png";

const UpdatedPopUp = ({ onClose }) => {
  const navigate = useNavigate();

  const onGoToProfileButtonClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  return (
    <div className="updated-pop-up">
      <div className="form1">
        <div className="form-icon">
          <img className="icon1" alt="" src= {updatePopupIcon} />
        </div>
        <div className="form-text1">
          <b className="yey-your-profile">
            Yey, your profile has been successfully updated!
          </b>
        </div>
        <button
          className="go-to-profile-button"
          onClick={onGoToProfileButtonClick}
        >
          <div className="button12">Go to Profile</div>
        </button>
      </div>
    </div>
  );
};

export default UpdatedPopUp;
