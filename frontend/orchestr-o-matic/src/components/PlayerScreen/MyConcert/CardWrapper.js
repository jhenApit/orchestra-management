import "./CardWrapper.css";
import concertCardImage1 from "../../../images/concert-img@2x.png";
import concertCardImage2 from "../../../images/concert-img1@2x.png";
import concertCardImage3 from "../../../images/concert-img2@2x.png";

const CardWrapper = () => {
  return (
    <div className="cardwrapper2">
      <div className="concerts1">
        <div className="rec-card-12">
          <img className="concert-img-icon" alt="" src= {concertCardImage1} />
          <div className="concert-details">
            <div className="concert-details-inner">
              <div className="concert-id-parent">
                <div className="concert-id">0001</div>
                <div className="concert-name">Spring Concert</div>
              </div>
            </div>
          </div>
        </div>
        <div className="rec-card-22">
          <img className="concert-img-icon" alt="" src= {concertCardImage2} />
          <div className="concert-details1">
            <div className="concert-details-inner">
              <div className="concert-id-parent">
                <div className="concert-id">0002</div>
                <div className="concert-name">Trans-Siberian</div>
              </div>
            </div>
          </div>
        </div>
        <div className="rec-card-22">
          <img className="concert-img-icon" alt="" src={concertCardImage3} />
          <div className="concert-details1">
            <div className="concert-details-inner">
              <div className="concert-id-parent">
                <div className="concert-id">0002</div>
                <div className="concert-name">Metamorphoseon</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CardWrapper;
