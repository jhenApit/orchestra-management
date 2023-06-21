import "./OrchestraCardsColumn.css";
import musicianCardImg1 from "../../../images/franco-batiatto-saxophone-1@2x.png";
import musicianCardImg2 from "../../../images/pianist-img@2x.png";
import musicianCardImg3 from "../../../images/violinist-img@2x.png";

const OrchestraCardsColumn = () => {
  return (
    <div className="orchestra-cards-column">
      <div className="saxophonist-card">
        <div className="card-image">
          <img
            className="saxophonist-img-icon"
            alt=""
            src= {musicianCardImg1}
          />
        </div>
        <div className="card-text">
          <div className="musician-type">Saxophonist</div>
          <div className="musician-description">
            {" "}
            is a musician who plays the saxophone, a woodwind instrument that
            uses reeds and keys to produce sound.
          </div>
        </div>
      </div>
      <div className="saxophonist-card">
        <div className="card-image">
          <img
            className="saxophonist-img-icon"
            alt=""
            src= {musicianCardImg2}
          />
        </div>
        <div className="card-text">
          <div className="musician-type">Pianist</div>
          <div className="musician-description1">
            <p className="is-a-musician">
              is a musician who plays the piano, a keyboard instrument that uses
              hammers and strings to produce.
            </p>
          </div>
        </div>
      </div>
      <div className="saxophonist-card">
        <div className="card-image">
          <img
            className="violinist-img-icon"
            alt=""
            src= {musicianCardImg3}
          />
        </div>
        <div className="card-text">
          <div className="musician-type">Violinist</div>
          <div className="musician-description2">
            <p className="is-a-musician">
              a musician who plays the violin, a stringed instrument, using a
              bow to create a range of sounds.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrchestraCardsColumn;
