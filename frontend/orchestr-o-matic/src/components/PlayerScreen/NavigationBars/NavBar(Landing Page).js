import { useMemo } from "react";
import "./NavBar(Landing Page).css";
import logoNav from "../../../images/logo@2x.png";
const NavBar = ({
  propCursor,
  onHomeClick,
  onAboutUsClick,
  onContactUsClick,
  onLoginClick,
  onSignUpClick,
}) => {
  useMemo(() => {
    return {
      cursor: propCursor,
    };
  }, [propCursor]);

  return (
    <div className="navigation-bar">
      <button className="logo-container">
        <img className="logo-icon" alt="" src={logoNav} />
        <i className="orchestr-o-matic">Orchestr-o-matic</i>
      </button>
      <div className="header-menu">
        <button className="home" onClick={onHomeClick}>
          Home
        </button>
        <button className="about-us3" onClick={onAboutUsClick}>
          About Us
        </button>
        <button className="about-us3" onClick={onContactUsClick}>
          Contact Us
        </button>
      </div>
      <div className="buttons">
        <button className="login-button" onClick={onLoginClick}>
          <div className="login">Login</div>
        </button>
        <button className="signup-button" onClick={onSignUpClick}>
          <div className="signup">Signup</div>
        </button>
      </div>
    </div>
  );
};

export default NavBar;
