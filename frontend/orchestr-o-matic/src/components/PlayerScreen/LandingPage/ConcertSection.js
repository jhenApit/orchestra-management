import "./ConcertSection.css";
import concertCardImageLP1 from "../../../images/ppo-concert@2x.png";
import concertCardImageLP2 from "../../../images/discovery-gala-concert@2x.png";
import concertCardImageLP3 from "../../../images/frenchship-concert@2x.png";
import concertCardImageLP4 from "../../../images/ppo50thanniversarygala@2x.png";

const ConcertSection = () => {
  return (
    <div className="concert-section">
      <div className="concert-section-header">
        <div className="assigning-musicians">ASSIGNING MUSICIANS</div>
        <div className="to-concert-sections">To Concert Sections</div>
      </div>
      <div className="concert-cards-column">
        <div className="card-frame-1">
          <button className="image-holder">
            <img
              className="ppo-concert-icon"
              alt=""
              src= {concertCardImageLP1}
            />
          </button>
        </div>
        <div className="card-frame-2">
          <button className="image-holder1">
            <img
              className="ppo-concert-icon"
              alt=""
              src= {concertCardImageLP2}
            />
          </button>
        </div>
        <div className="card-frame-2">
          <button className="image-holder1">
            <img
              className="ppo-concert-icon"
              alt=""
              src= {concertCardImageLP3}
            />
          </button>
        </div>
        <div className="card-frame-4">
          <button className="image-holder1">
            <img
              className="ppo-concert-icon"
              alt=""
              src= {concertCardImageLP4}
            />
          </button>
        </div>
      </div>
    </div>
  );
};

export default ConcertSection;
