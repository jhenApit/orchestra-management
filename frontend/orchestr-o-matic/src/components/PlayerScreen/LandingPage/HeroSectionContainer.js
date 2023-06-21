import "./HeroSectionContainer.css";
import heroImage from "../../../images/Hero Image@2x.png";


const HeroSectionContainer = () => {
  return (
    <div className="hero-section-container">
      <div className="hero-image">
        <img className="hero-image-icon" alt="" src= {heroImage} />
        <div className="hero-mask" />
      </div>
      <div className="hero-content">
        <div className="content-title">
          <i className="content-subtitle">Orchestr-o-matic</i>
          <b className="content-maintitle">
            <span className="an">An </span>
            <span className="orchestra">ORCHESTRA</span>
            <span className="manager"> Manager API</span>
            <span className="span">{` `}</span>
          </b>
        </div>
      </div>
    </div>
  );
};

export default HeroSectionContainer;
