import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import HeroSectionContainer from "../../components/PlayerScreen/LandingPage/HeroSectionContainer";
import OrchestraCardsColumn from "../../components/PlayerScreen/LandingPage/OrchestraCardsColumn";
import ConcertSection from "../../components/PlayerScreen/LandingPage/ConcertSection";
import "../../styles/Landing/LandingPage.css";
import logo from "../../images/logo@2x.png";
import footerIcon from "../../images/footer-icon@2x.png";

const LandingPage = () => {
  const navigate = useNavigate();

  const onAboutUsClick = useCallback(() => {
    navigate("/about-us");
  }, [navigate]);

  const onContactUsClick = useCallback(() => {
    navigate("/contact-us");
  }, [navigate]);

  const onLoginClick = useCallback(() => {
    navigate("/signin");
  }, [navigate]);

  const onSignUpClick = useCallback(() => {
    navigate("/signup");
  }, [navigate]);

  return (
    <div className="landing-page">
      <div className="navigation-bar">
        <button className="logo-container">
          <img className="logo-icon" alt="" src={logo} />
          <i className="orchestr-o-matic">Orchestr-o-matic</i>
        </button>
        <div className="header-menu">
          <button className="home">Home</button>
          <button className="about-us2" onClick={onAboutUsClick}>
            About Us
          </button>
          <button className="contact-us1" onClick={onContactUsClick}>
            {`Contact Us  `}
          </button>
        </div>
        <div className="buttons">
          <button className="login-button" onClick={onLoginClick}>
            <div className="button1">Login</div>
          </button>
          <button className="signup-button" onClick={onSignUpClick}>
            <div className="button2">Signup</div>
          </button>
        </div>
      </div>
      <div className="hero-section">
        <HeroSectionContainer />
      </div>
      <div className="middle-section">
        <div className="orchestra-section">
          <div className="orchestra-section-header">
            <div className="orchestra-section-header1">{`Specify Instrumental Specialties  `}</div>
            <div className="orchestra-section-header2">
              To Orchestra Musicians
            </div>
          </div>
          <OrchestraCardsColumn />
        </div>
        <ConcertSection />
        <div className="footer-section1">
          <img className="footer-icon" alt="" src={footerIcon} />
          <div className="footer-details">
            <b className="footer-header">Become a Conductor</b>
            <div className="footer-details1">
              Join the ever-expanding community of Orchestr-O-Matic. Sign up as
              a conductor and ensure you register your orchestra as well.
            </div>
          </div>
          <button className="signup-button1">
            <div className="button2">Signup</div>
          </button>
        </div>
      </div>
    </div>
  );
};

export default LandingPage;
