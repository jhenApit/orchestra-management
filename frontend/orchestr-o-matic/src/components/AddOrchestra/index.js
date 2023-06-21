import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import * as AiIcons from "react-icons/ai";
import "./AddOrchestra.css";
import { addOrchestra, editOrchestra } from "../../api/orchestra";

function AddOrchestra_Popup(props) {
  const [name, setName] = useState();
  const [description, setDescription] = useState();

  useEffect(() => {
    if (props.row) {
      setName(props.row.name);
      setDescription(props.row.description);
    }
  }, [props.row]);

  const saveAdd = async (e) => {
    e.preventDefault();
    await addOrchestra({
      name: name,
      conductorId: 1,
      description,
    });
    props.getData();
    props.setTrigger(false);
  };

  const saveEdit = async (e) => {
    e.preventDefault();
    await editOrchestra(props.row.id, {
      name: name,
      conductorId: 1,
      description,
    });
    props.getData();
    props.setTrigger(false);
  };

  return props.trigger ? (
    <div className="add-orchestra-popup">
      <div className="add-orchestra-popup-inner">
        <div className="add-orchestra-popup-header">
          <Link
            to="#"
            className="orchestra-close-btn"
            onClick={() => props.setTrigger(false)}
          >
            <AiIcons.AiFillCloseCircle />
          </Link>
          {props.children}
        </div>
        <div className="add-orchestra-popup-infos">
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
            <button
              type="button"
              className="orchestra-cancel-btn"
              onClick={() => props.setTrigger(false)}
            >
              Cancel
            </button>
            <button
              className="orchestra-save-btn"
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

export default AddOrchestra_Popup;
