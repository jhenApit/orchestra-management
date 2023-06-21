import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import NavBar from "../../components/PlayerScreen/NavigationBars/NavBar(Landing Page)";
import "../../styles/Landing/AboutUs.css";
import aboutUsBg from "../../images/hero-section-img1@2x.png";

const AboutUs = () => {
  const navigate = useNavigate();

  const onHomeClick = useCallback(() => {
    navigate("/");
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
    <div className="about-us">
      <div className="hero-section-background1">
        <img className="hero-section-img1" alt="" src={aboutUsBg} />
      </div>
      <div className="hero-section-header1">
        <b className="about-us1">About Us</b>
      </div>
      <div className="mid-section-contents">
        <div className="conductor-contents-frame">
          <div className="conductor-contents">
            {" "}
            you can easily manage concerts, orchestras, and musicians, and
            assign players to concert sections based on their instrumental
            specialties. Our API streamlines orchestra management, allowing
            conductors to focus on creating unforgettable musical experiences
            for their audiences. Conductors can also accept or reject
            applications from players who are interested in joining their
            orchestra.
          </div>
        </div>
        <div className="player-contents-frame">
          <div className="conductor-contents">
            {" "}
            you can apply for orchestras and view upcoming concerts and
            orchestras. Our API makes it easy to find the perfect orchestra to
            join based on your instrumental specialty. You can easily manage
            your applications and view the status of each one. With
            Orchestra-o-matic, you can connect with conductors and find new
            opportunities to showcase your musical talent.
          </div>
        </div>
        <div className="player-title">
          <i className="as-a-player-container">
            <span className="as">{`As `}</span>
            <span className="a">a</span>
            <span className="as"> Player</span>
          </i>
        </div>
        <div className="conductor-title">
          <i className="as-a-player-container">
            <span className="as">{`As `}</span>
            <span className="a">a</span>
            <span className="as"> conductor</span>
          </i>
        </div>
      </div>
      <div className="mid-section-header">
        <div className="mid-section-header1">{`Welcome to Orchestra-o-matic! `}</div>
        <div className="mid-section-header2">
          Our website is an orchestra manager API that caters to two primary
          users: conductors and players.
        </div>
      </div>
      <div className="footer-section">
        <i className="footer-contents">
          At Orchestra-o-matic, we're committed to providing the best orchestra
          management API for conductors and players. We're passionate about
          music and believe that our platform can help make the world a more
          musical place.. Thank you for choosing Orchestra-o-matic!
        </i>
      </div>
      <NavBar
        propCursor="pointer"
        onHomeClick={onHomeClick}
        onContactUsClick={onContactUsClick}
        onLoginClick={onLoginClick}
        onSignUpClick={onSignUpClick}
      />
    </div>
  );
};

export default AboutUs;
