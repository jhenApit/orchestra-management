import React, { useEffect, useState } from "react";
import Navbar from "../../components/Navbar";
import "../../styles/Conductor/ConductorDashboard.css";
import "../../styles/Searchbar.css";
import { ImMagicWand } from "react-icons/im";
import { GiMusicalNotes } from "react-icons/gi";
import { fetchConcertData } from "../../api/concert";
import { fetchOrchestraData } from "../../api/orchestra";
import moment from "moment";

function Dashboard() {
  const [orchestra, setOrchestra] = useState([]);
  const [concerts, setConcert] = useState([]);

  useEffect(() => {
    async function getData() {
      const concerts = await fetchConcertData();
      setConcert(concerts?.reverse());
      setOrchestra(await fetchOrchestraData());
    }

    getData();
  }, []);

  return (
    <>
      <Navbar />
      <div className="dashboard">
        <div className="dashboard-header">DASHBOARD</div>
        <div className="dashboard-numbers">
          <div className="numbers-container">
            <h2>Total Concerts</h2>
            <div>
              <ImMagicWand id="wand-icon" />
              <span>{concerts ? concerts.length : 0}</span>
            </div>
          </div>
          <div className="vr" />
          <div className="numbers-container">
            <h2>Total Orchestra</h2>
            <div>
              <GiMusicalNotes id="notes-icon" />
              <span>{orchestra ? orchestra.length : 0}</span>
            </div>
          </div>
        </div>
        <div className="dashboard-concerts">
          <div className="dashboard-recent-performances">
            RECENT PERFORMANCES
            {concerts &&
              concerts.length > 0 &&
              concerts.slice(0, 5).map((concert) => (
                <div className="dashboard-performance-list">
                  <div className="dashboard-concert-container">
                    <span className="dashboard-concert-name">
                      {concert.name}
                    </span>
                    <span className="dashboard-concert-date">
                      {moment(concert.date).format("MMMM D, YYYY")}
                    </span>
                  </div>
                  <p className="dashboard-concert-description">
                    {concert.description}
                  </p>
                </div>
              ))}
          </div>
        </div>
      </div>
    </>
  );
}

export default Dashboard;
