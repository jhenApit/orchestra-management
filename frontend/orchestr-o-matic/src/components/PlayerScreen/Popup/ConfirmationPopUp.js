import "./ConfirmationPopUp.css";
import confirmPopupIcon from "../../../images/9@2x.png";

const ConfirmationPopUp = ({ onClose }) => {
  return (
    <div className="confirmation-pop-up">
      <div className="form2">
        <div className="form-icon1">
          <img className="icon2" alt="" src= {confirmPopupIcon} />
        </div>
        <div className="form-text2">
          <div className="we-have-received">
            We have received your details and will get back to you as soon as
            possible regarding your application.
          </div>
          <b className="thank-you-for-container">
            <p className="thank-you-for">{`Thank you for expressing your interest in joining our orchestra! `}</p>
            <p className="thank-you-for">&nbsp;</p>
          </b>
        </div>
        <button className="go-to-dashboard-button">
          <div className="button13">Go to Dashboard</div>
        </button>
      </div>
    </div>
  );
};

export default ConfirmationPopUp;
