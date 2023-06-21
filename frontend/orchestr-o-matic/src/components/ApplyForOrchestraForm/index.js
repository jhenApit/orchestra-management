import { useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { TextField } from "@mui/material";
import ConfirmationPopUp from "../PlayerScreen/Popup/ConfirmationPopUp";
import PortalPopup from "../PlayerScreen/Popup/PortalPopup";
import Navbar1 from "../PlayerScreen/NavigationBars/Navbar(Player Profile)";
import orchestraPic from "../../images/orchestra-img2@2x.png";
import "./ApplyForOrchestraForm.css";

const ApplyForOrchestraForm = () => {
  const [isConfirmationPopUpPopupOpen, setConfirmationPopUpPopupOpen] =
    useState(false);
  const navigate = useNavigate();

  const openConfirmationPopUpPopup = useCallback(() => {
    setConfirmationPopUpPopupOpen(true);
  }, []);

  const closeConfirmationPopUpPopup = useCallback(() => {
    setConfirmationPopUpPopupOpen(false);
  }, []);

  const onApplyForOrchestraClick = useCallback(() => {
    navigate("/my-profilee");
  }, [navigate]);

  const onCancelButtonClick = useCallback(() => {
    navigate("/apply-for-orchestra");
  }, [navigate]);

  return (
    <>
      <div className="apply-for-orchestra-form">
        <Navbar1
          myConcert="Apply for Orchestra"
          onMyConcertTextClick={onApplyForOrchestraClick}
        />
        <div className="cards-container">
          <div className="orchestra-image-container">
            <img className="orchestra-img-icon" alt="" src={orchestraPic} />
          </div>
          <div className="form-card">
            <div className="form-container">
              <div className="form-contents">
                <div className="header">
                  <b className="main-title">UP Symphony Orchestra</b>
                  <div className="est-date">EST. January 11, 2020</div>
                </div>
                <div className="header-subtitle">
                  <div className="main-title">
                    We would be thrilled to have you as part of our orchestra!
                    Please provide us with the following information so we can
                    get to know you better and find the best way to incorporate
                    your skills and talents:
                  </div>
                </div>
                <div className="header">
                  <div className="form-fields1">
                    <TextField
                      className="section-input"
                      color="primary"
                      variant="outlined"
                      type="text"
                      label="Section"
                      size="medium"
                      margin="none"
                      required
                    />
                    <TextField
                      className="section-input"
                      color="primary"
                      variant="outlined"
                      type="text"
                      label="Instrument"
                      size="medium"
                      margin="none"
                      required
                    />
                    <TextField
                      className="section-input"
                      color="primary"
                      variant="outlined"
                      type="text"
                      label="Years of Experience"
                      size="medium"
                      margin="none"
                      required
                    />
                  </div>
                </div>
                <div className="buttons1">
                  <button
                    className="apply-button"
                    onClick={openConfirmationPopUpPopup}
                  >
                    <div className="button4">Apply</div>
                  </button>
                  <button
                    className="cancel-button"
                    onClick={onCancelButtonClick}
                  >
                    <div className="button5">Cancel</div>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      {isConfirmationPopUpPopupOpen && (
        <PortalPopup
          overlayColor="rgba(113, 113, 113, 0.3)"
          placement="Centered"
          onOutsideClick={closeConfirmationPopUpPopup}
        >
          <ConfirmationPopUp onClose={closeConfirmationPopUpPopup} />
        </PortalPopup>
      )}
    </>
  );
};

export default ApplyForOrchestraForm;
