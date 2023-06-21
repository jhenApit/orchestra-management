import React, { useEffect, useState } from "react";
import { fetchLeaderboardData, fetchSections } from "../../api/ranking";
import "../../styles/Player/PlayerRanking.scss";
import { FaArrowAltCircleLeft } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

const Ranking = () => {
  const [leaderboardData, setLeaderboardData] = useState([]);
  const [sections, setSections] = useState([]);
  const [selectedSectionId, setSelectedSectionId] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRankingData = async () => {
      try {
        const data = await fetchLeaderboardData(selectedSectionId);
        const sortedData = data.sort((a, b) => b.score - a.score);
        const rankedData = sortedData.map((player, index) => ({
          ...player,
          rank: index + 1,
        }));
        setLeaderboardData(rankedData);
      } catch (error) {
        console.error("Error fetching leaderboard data:", error);
        setLeaderboardData([]);
      }
    };

    fetchRankingData();
  }, [selectedSectionId]);

  useEffect(() => {
    const fetchSectionsData = async () => {
      try {
        const data = await fetchSections();
        setSections(data);
      } catch (error) {
        console.error("Error fetching sections:", error);
        setSections([]);
      }
    };

    fetchSectionsData();
  }, []);

  const navigateToDashboard = () => {
    navigate("/dashboard");
  };

  const handleSectionChange = (event) => {
    setSelectedSectionId(event.target.value);
  };

  return (
    <div className="ranking">
      <div className="back-icon" onClick={navigateToDashboard}>
        <FaArrowAltCircleLeft />
      </div>
      <div className="form-group">
        <h1>Leaderboard</h1>
        <div className="section-dropdown">
          <label htmlFor="section">Section:</label>
          <select
            name="section"
            id="section"
            value={selectedSectionId}
            onChange={handleSectionChange}
          >
            <option value="">All Sections</option>
            {sections.map((section) => (
              <option key={section.id} value={section.id}>
                {section.name}
              </option>
            ))}
          </select>
        </div>
        <div className="player-cards">
          {leaderboardData.map((player) => (
            <div key={player.id} className="player-card">
              <h2>{player.name}</h2>
              <p>Score: {player.score}</p>
              <p>Rank: {player.rank}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Ranking;
