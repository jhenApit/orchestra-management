import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../PlayerScreen/NavigationBars/Navbar(Player Profile)";
import "./ApplyForOrchestra.css";
import msoOrchestra from "../../images/orchestra-img3@2x.png";
import ppoOrchestra from "../../images/orchestra--logo@2x.png";
import absOrchestra from "../../images/orchestra--logo1@2x.png";
import upOrchestra from "../../images/orchestra--logo2@2x.png";
import ustOrchestra from "../../images/orchestra--logo3@2x.png";
import cyoOrchestra from "../../images/orchestra--logo4@2x.png";

const ApplyForOrchestra = () => {
  const navigate = useNavigate();

  const onApplyForOrchestraClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  const onJoinButtonClick = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  const onJoinButton1Click = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  const onJoinButton2Click = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  const onJoinButton3Click = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  const onJoinButton4Click = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  const onJoinButton5Click = useCallback(() => {
    navigate("/apply-for-orchestra-form");
  }, [navigate]);

  return (
    <div className="apply-for-orchestra">
      <Navbar1
        myConcert="Apply for Orchestra"
        onMyConcertTextClick={onApplyForOrchestraClick}
      />
      <div className="list-of-orchestra1">
        <div className="upper-section">
          <div className="cardwrapper">
            <div className="orchestras">
              <div className="rec-card-1">
                <img className="orchestra-logo" alt="" src={msoOrchestra} />
                <div className="orchestra-details">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0001</div>
                      <div className="orchestra-name">
                        Manila Symphony Orchestra
                      </div>
                    </div>
                  </div>
                  <button className="join-button1" onClick={onJoinButtonClick}>
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
              <div className="rec-card-2">
                <img className="orchestra-logo" alt="" src={ppoOrchestra} />
                <div className="orchestra-details">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0002</div>
                      <div className="orchestra-name">
                        Philippine Philharmonic Orchestra
                      </div>
                    </div>
                  </div>
                  <button className="join-button1" onClick={onJoinButton1Click}>
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
              <div className="rec-card-2">
                <img className="orchestra-logo" alt="" src={absOrchestra} />
                <div className="orchestra-details2">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0003</div>
                      <div className="orchestra-name">
                        ABS-CBN Philharmonic Orchestra
                      </div>
                    </div>
                  </div>
                  <button className="join-button1" onClick={onJoinButton2Click}>
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="list-of-orchestra1">
        <div className="new">
          <div className="cardwrapper">
            <div className="orchestras">
              <div className="rec-card-11">
                <img className="orchestra-logo" alt="" src={upOrchestra} />
                <div className="orchestra-details">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0004</div>
                      <div className="orchestra-name">
                        UP Symphony Orchestra
                      </div>
                    </div>
                  </div>
                  <button className="join-button1" onClick={onJoinButton3Click}>
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
              <div className="rec-card-2">
                <img className="orchestra-logo3" alt="" src={ustOrchestra} />
                <div className="orchestra-details2">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0005</div>
                      <div className="orchestra-name">
                        UST Symphony Orchestra
                      </div>
                    </div>
                  </div>
                  <button
                    className="join-button1"
                    autoFocus
                    onClick={onJoinButton4Click}
                  >
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
              <div className="rec-card-2">
                <img className="orchestra-logo" alt="" src={cyoOrchestra} />
                <div className="orchestra-details2">
                  <div className="orchestra-details-inner">
                    <div className="orchestra-id-parent">
                      <div className="orchestra-id">0006</div>
                      <div className="orchestra-name">
                        Classic Youth Orchestra
                      </div>
                    </div>
                  </div>
                  <button className="join-button1" onClick={onJoinButton5Click}>
                    <div className="button6">Join</div>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ApplyForOrchestra;
