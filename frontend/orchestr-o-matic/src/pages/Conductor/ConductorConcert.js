import React, { useCallback, useEffect, useMemo, useState } from "react";
import "../../styles/Conductor/ConductorConcert.css";
import "../../styles/Searchbar.css";
import Popup from "../../components/AddConcert";
import * as MdIcons from "react-icons/md";
import { deleteConcert, fetchConcertData } from "../../api/concert";
import moment from "moment/moment";
import Navbar from "../../components/Navbar";

const ITEMS_PER_PAGE = 5;

function Concert() {
  const [buttonPopup, setButtonPopup] = useState(false);
  const [data, setData] = useState([]);
  const [page, setPage] = useState(1);
  const [editRow, setEditRow] = useState(-1);

  const numberOfPages = useMemo(() => {
    let result = 0;
    if (data && data.length > 0) {
      return Math.ceil(data?.length / ITEMS_PER_PAGE);
    }
    return result;
  }, [data]);

  const getPaginatedData = useCallback(() => {
    const startIdx = (page - 1) * ITEMS_PER_PAGE;
    const endIdx = page * ITEMS_PER_PAGE;
    return data.slice(startIdx, endIdx);
  }, [page, data]);

  useEffect(() => {
    getData();
  }, []);

  const getData = async () => {
    setData(await fetchConcertData());
  };

  useEffect(() => {
    if (buttonPopup === false) {
      setEditRow(-1);
    }
  }, [buttonPopup]);

  const deleteItem = async (e, id) => {
    try {
      await deleteConcert(id);
      setData((d) => d.filter((o) => o.id !== id));
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <>
      <Navbar />
      <div className="concert">
        <div className="concert-header">
          <div className="concert-input-wrapper" id="concert">
            <button
              className="add-concert-btn"
              onClick={() => setButtonPopup(true)}
            >
              Add concert
            </button>
            <Popup
              trigger={buttonPopup}
              setTrigger={setButtonPopup}
              row={data.filter((fd) => fd.id === editRow)[0]}
              getData={getData}
            >
              <h3>{editRow === -1 ? "Add new concert" : "Edit concert"}</h3>
            </Popup>
          </div>
          <div id="concert-title">CONCERTS</div>
        </div>
        <div className="concert-list">
          <table className="concert-table">
            <thead className="concert-table-head">
              <tr>
                <th width="200">Concert ID</th>
                <th width="400">Name</th>
                <th>Description</th>
                <th width="300">Date</th>
                <th></th>
              </tr>
            </thead>
            <tbody className="concert-table-body">
              {data &&
                data?.length > 0 &&
                getPaginatedData().map((concert) => (
                  <tr key={concert.id}>
                    <td className="concert-id">
                      <div>{concert.id}</div>
                    </td>
                    <td className="concert-name">
                      <div>{concert.name}</div>
                    </td>
                    <td className="concert-description">
                      <div>{concert.description}</div>
                    </td>
                    <td className="concert-date">
                      <div>{moment(concert.date).format("MMMM D, YYYY")}</div>
                    </td>
                    <td className="concert-actions">
                      <div className="actions">
                        <MdIcons.MdEdit
                          id="edit-icon"
                          onClick={() => {
                            setEditRow(concert.id);
                            setButtonPopup(true);
                          }}
                        />
                        <MdIcons.MdDelete
                          id="delete-icon"
                          onClick={(e) => deleteItem(e, concert.id)}
                        />
                      </div>
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
          <div className="pagination">
            {numberOfPages > 1 &&
              [...Array(numberOfPages)].map((_, i) => (
                <span
                  className={page === i + 1 && "page-active"}
                  onClick={() => setPage(i + 1)}
                >
                  {i + 1}
                </span>
              ))}
          </div>
        </div>
      </div>
    </>
  );
}

export default Concert;
