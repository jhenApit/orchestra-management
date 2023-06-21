import React, { useEffect, useState } from "react";
import "../../styles/Conductor/ConductorOrchestra.css";
import "../../styles/Searchbar.css";
import Popup from "../../components/AddOrchestra";
import { FaSearch } from "react-icons/fa";
import * as MdIcons from "react-icons/md";
import { fetchOrchestraData } from "../../api/orchestra";
import Navbar from "../../components/Navbar";
import { deleteOrchestra } from "../../api/orchestra";
import moment from "moment";

function Orchestra() {
  const [buttonPopup, setButtonPopup] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [data, setData] = useState([]);
  const [filteredData, setFilterData] = useState([]);
  const [editRow, setEditRow] = useState(-1);

  useEffect(() => {
    getData();
  }, []);

  const getData = async () => {
    const orchestraData = await fetchOrchestraData();
    setData(orchestraData);
    setFilterData(orchestraData);
  };

  useEffect(() => {
    if (data && data?.length > 0 && searchText !== "") {
      setFilterData(
        data.filter((o) =>
          o.name.toLowerCase().includes(searchText.toLowerCase())
        )
      );
    } else {
      setFilterData(data);
    }
  }, [searchText, data]);

  useEffect(() => {
    if (buttonPopup === false) {
      setEditRow(-1);
    }
  }, [buttonPopup]);

  const deleteItem = async (e, id) => {
    try {
      await deleteOrchestra(id);
      setData((d) => d.filter((o) => o.id !== id));
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <>
      <Navbar />
      <div className="orchestra">
        <div className="orchestra-header">
          <span className="header-title">ORCHESTRAS</span>
          <div className="orchestra-input-wrapper" id="orchestra">
            <FaSearch id="search-icon" />
            <input
              placeholder="Search orchestra"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
          </div>
          <button
            className="add-orchestra-btn"
            onClick={() => setButtonPopup(true)}
          >
            Add orchestra
          </button>
          <Popup
            trigger={buttonPopup}
            setTrigger={setButtonPopup}
            row={filteredData.filter((fd) => fd.id === editRow)[0]}
            getData={getData}
          >
            <h3>{editRow === -1 ? "Add new orchestra" : "Edit orchestra"}</h3>
          </Popup>
        </div>
        <div className="orchestra-contents">
          {filteredData &&
            filteredData?.length > 0 &&
            filteredData.map((orchestra) => (
              <div className="orchestra-content" key={orchestra.id}>
                <b>{orchestra.name}</b>
                <div className="actions">
                  <MdIcons.MdEdit
                    id="edit-icon"
                    onClick={() => {
                      setEditRow(orchestra.id);
                      setButtonPopup(true);
                    }}
                  />
                  <MdIcons.MdDelete
                    id="delete-icon"
                    onClick={(e) => deleteItem(e, orchestra.id)}
                  />
                </div>
                <br />
                EST. {moment(orchestra.date).format("MMMM D, YYYY")}
                <br />
                <br />
                <div className="content-description">
                  {orchestra.description}
                </div>
              </div>
            ))}
        </div>
      </div>
    </>
  );
}

export default Orchestra;
