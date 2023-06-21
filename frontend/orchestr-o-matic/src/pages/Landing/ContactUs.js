import { useCallback } from "react";
import { TextField } from "@mui/material";
import { useNavigate } from "react-router-dom";
import NavBar from "../../components/PlayerScreen/NavigationBars/NavBar(Landing Page)";
import "../../styles/Landing/ContactUs.css";
import contactUsBg from "../../images/hero-section-img@2x.png";

const ContactUs = () => {
  const navigate = useNavigate();

  const onHomeClick = useCallback(() => {
    navigate("/");
  }, [navigate]);

  const onAboutUsClick = useCallback(() => {
    navigate("/about-us");
  }, [navigate]);

  const onLoginClick = useCallback(() => {
    navigate("/signin");
  }, [navigate]);

  const onSignUpClick = useCallback(() => {
    navigate("/signup");
  }, [navigate]);

  const onSubmitButtonClick = useCallback(() => {
    navigate("/");
  }, [navigate]);

  return (
    <div className="contact-us">
      <div className="hero-section-background">
        <img className="hero-section-img" alt="" src={contactUsBg} />
      </div>
      <div className="hero-section-header">
        <b className="hero-section-title">Contact Us</b>
      </div>
      <div className="form">
        <div className="form-text">
          <div className="fill-in-your">Fill in your details!</div>
        </div>
        <div className="form-fields">
          <div className="form-text">
            <TextField
              className="input"
              color="primary"
              variant="outlined"
              type="text"
              label="Your name"
              size="medium"
              margin="none"
            />
          </div>
          <div className="form-text">
            <TextField
              className="input"
              color="primary"
              variant="outlined"
              type="email"
              label="Email address"
              size="medium"
              margin="none"
              required
            />
          </div>
          <TextField
            className="queries-input"
            color="primary"
            variant="outlined"
            multiline
            rows={3}
            label="Your queries"
            margin="none"
            fullWidth
          />
        </div>
      </div>
      <div className="header-container">
        <b className="header-title">Let’s answer your queries!</b>
      </div>
      <button className="submit-button" onClick={onSubmitButtonClick}>
        <div className="button">Submit</div>
      </button>
      <NavBar
        onHomeClick={onHomeClick}
        onAboutUsClick={onAboutUsClick}
        onLoginClick={onLoginClick}
        onSignUpClick={onSignUpClick}
      />
    </div>
  );
};

export default ContactUs;
