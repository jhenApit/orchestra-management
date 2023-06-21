import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../../components/PlayerScreen/NavigationBars/Navbar(Player Profile)";
import { fetchOrchestra, fetchPlayer } from "../../api/dashboard";
import "../../styles/Player/PlayerDashboard.scss";
import orchestraImage from "../../images/orchestra-img1@2x.png";

const PlayerDashboard = () => {
  const navigate = useNavigate();
  const [orchestras, setOrchestras] = useState([]);
  const [players, setPlayers] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const orchestraData = await fetchOrchestra();
        const playerData = await fetchPlayer(1);
        setOrchestras(orchestraData);
        setPlayers(playerData);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const onMyDashboardTextClick = useCallback(() => {
    navigate("/dashboard");
  }, [navigate]);

  const handleSeeAll = () => {
    navigate("/my-orchestra");
  };

  const handleViewRanking = () => {
    navigate("/ranking");
  };

  return (
    <div className="player-dashboard">
      <div className="content">
        <Navbar1
          myConcert="Dashboard"
          onMyConcertTextClick={onMyDashboardTextClick}
        />
        <div className="info-card">
          <div className="info-header">
            <h3>My Score</h3>
            <button onClick={handleViewRanking}>View Ranking</button>
          </div>
          <div className="info">
            {players.map((player) => (
              <div key={player.id} className="card">
                <p>Name: {player.name}</p>
                <p>Section: {player.section}</p>
                <p>Score: {player.score}</p>
              </div>
            ))}
          </div>
        </div>
        <div className="orchestra-card">
          <div className="orchestra-header">
            <h3>Available Orchestras</h3>
            <button onClick={handleSeeAll}>View More</button>
          </div>
          <div className="orchestras">
            {orchestras.map((orchestra) => (
              <div key={orchestra.id} className="orchestra">
                <div className="orchestra-image">
                  <img src={orchestraImage} alt={orchestra.name} />
                </div>
                <div className="orchestra-info">
                  <h1>{orchestra.name}</h1>
                  <h5>{new Date(orchestra.date).toLocaleString()}</h5>
                  <h3>Conductor: {orchestra.conductor}</h3>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default PlayerDashboard;
