import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { addConcert, editConcert } from "../../api/concert";
import { fetchOrchestraData } from "../../api/orchestra";
import * as AiIcons from "react-icons/ai";
import "./AddConcert.css";

function AddConcert_Popup(props) {
  const [name, setName] = useState();
  const [orchestra, setOrchestra] = useState([]);
  const [description, setDescription] = useState();
  const [selectedOrchestra, setSelectedOrchestra] = useState(-1);

  useEffect(() => {
    async function getData() {
      const orchestraData = await fetchOrchestraData();
      setOrchestra(orchestraData);
    }

    getData();
  }, []);

  useEffect(() => {
    setName("");
    setDescription("");
    setSelectedOrchestra(-1);
    if (props.row) {
      setName(props.row.name);
      setDescription(props.row.description);
      setSelectedOrchestra(orchestra[0].id);
    }
  }, [props.row, orchestra]);

  const saveAdd = async (e) => {
    e.preventDefault();
    await addConcert({
      name,
      description,
      performanceDate: new Date().toISOString(),
      orchestraId: selectedOrchestra,
    });
    props.getData();
    props.setTrigger(false);
  };

  const saveEdit = async (e) => {
    e.preventDefault();
    await editConcert(props.row.id, {
      name,
      description,
      performanceDate: new Date().toISOString(),
      orchestraId: selectedOrchestra,
    });
    props.getData();
    props.setTrigger(false);
  };

  return props.trigger ? (
    <div className="add-concert-popup">
      <div className="add-concert-popup-inner">
        <div className="add-concert-popup-header">
          <Link
            to="#"
            className="concert-close-btn"
            onClick={() => props.setTrigger(false)}
          >
            <AiIcons.AiFillCloseCircle />
          </Link>
          {props.children}
        </div>
        <div className="add-concert-popup-infos">
          <form>
            <label>Name</label>
            <br />
            <input
              type="text"
              required
              className="name-input-wrapper"
              value={name}
              onChange={(e) => setName(e.target.value)}
            ></input>
            <br />
            <label>Description</label>
            <br />
            <textarea
              required
              rows="4"
              className="description-input-wrapper"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            ></textarea>
            <br />
            <button
              type="button"
              className="concert-cancel-btn"
              onClick={() => props.setTrigger(false)}
            >
              Cancel
            </button>
            <button
              className="concert-save-btn"
              onClick={props.row ? saveEdit : saveAdd}
              disabled={!name || !description}
            >
              Save
            </button>
          </form>
        </div>
      </div>
    </div>
  ) : (
    ""
  );
}

export default AddConcert_Popup;
