import "./ConcertSection.css";
import concertCardImageLProfile1 from "../../../images/concert-img3@2x.png";
import concertCardImageLProfile2 from "../../../images/concert-img4@2x.png";
import concertCardImageLProfile3 from "../../../images/concert-img5@2x.png";

const ConcertSection = () => {
  return (
    <div className="concert-section1">
      <i className="concert-header">Concerts</i>
      <div className="concerts-container">
        <div className="concert-1">
          <img
            className="concert-img-icon3"
            alt=""
            src={concertCardImageLProfile1}
          />
          <div className="concert-details3">
            <div className="concert-details-inner2">
              <div className="concert-name-wrapper">
                <div className="concert-name3">Spring Concert</div>
              </div>
            </div>
          </div>
        </div>
        <div className="concert-1">
          <img
            className="concert-img-icon3"
            alt=""
            src={concertCardImageLProfile2}
          />
          <div className="concert-details3">
            <div className="concert-details-inner2">
              <div className="concert-name-wrapper">
                <div className="concert-name3">
                  Trans-Siberian Orchestra Concert
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="concert-1">
          <img
            className="concert-img-icon3"
            alt=""
            src={concertCardImageLProfile3}
          />
          <div className="concert-details3">
            <div className="concert-details-inner2">
              <div className="concert-name-wrapper">
                <div className="concert-name3">Metamorphoseon Concert</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ConcertSection;
