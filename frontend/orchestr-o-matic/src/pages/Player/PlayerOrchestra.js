import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../../components/PlayerScreen/NavigationBars/Navbar(Player Profile)";
import ContentSection from "../../components/PlayerScreen/MyOrchestra/ContentSection";
import "../../styles/Player/PlayerOrchestra.css";
const MyOrchestra = () => {
  const navigate = useNavigate();

  const onMyOrchestraTextClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  const onJoinButtonClick = useCallback(() => {
    navigate("/apply-for-orchestra");
  }, [navigate]);

  return (
    <div className="my-orchestra">
      <Navbar1
        myConcert="My Orchestra"
        onMyConcertTextClick={onMyOrchestraTextClick}
      />
      <div className="list-of-orchestra">
        <ContentSection />
      </div>
      <div className="footer-section2">
        <div className="wrapper">
          <div className="icon">
            <img
              className="material-symbolsmagic-button-icon"
              alt=""
              src="/materialsymbolsmagicbutton.svg"
            />
          </div>
          <div className="text">
            <div className="do-you-want">
              Do you want to join other orchestras?
            </div>
          </div>
          <button className="join-button" onClick={onJoinButtonClick}>
            <div className="join">Join</div>
          </button>
        </div>
      </div>
    </div>
  );
};

export default MyOrchestra;
