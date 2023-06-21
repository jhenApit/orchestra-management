import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../PlayerScreen/NavigationBars/Navbar(Player Profile)";
import Form from "../PlayerScreen/EditProfile/Form";
import "./PlayerEditProfile.css";

const EditProfile = () => {
  const navigate = useNavigate();

  const onEditProfileTextClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  return (
    <div className="edit-profile">
      <Navbar1
        myConcert="Edit Profile"
        onMyConcertTextClick={onEditProfileTextClick}
      />
      <div className="main-wrapper">
        <Form />
      </div>
    </div>
  );
};

export default EditProfile;
