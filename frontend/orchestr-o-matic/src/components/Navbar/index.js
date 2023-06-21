import React, { useContext, useState } from "react";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { SidebarData } from "./SidebarData";
import { IconContext } from "react-icons/lib";
import { UserContext } from "../../context/user";
import "./Navbar.css";
import * as FaIcons from "react-icons/fa";
import * as AiIcons from "react-icons/ai";
import * as IoIcons from "react-icons/io5";
import user from "../../images/user.png";

function Navbar() {
  const { user: userProfile, logoutUser } = useContext(UserContext);
  const [sidebar, setSidebar] = useState(false);
  const location = useLocation();

  const showSidebar = () => setSidebar(!sidebar);

  return (
    <>
      <IconContext.Provider value={{ color: "#F0DACD" }}>
        <div className="navbar">
          <Link to="#" className="open-menu-bars">
            <FaIcons.FaBars onClick={showSidebar} />
          </Link>
        </div>
        <nav className={sidebar ? "nav-menu active" : "nav-menu"}>
          <div>
            <div className="navbar-toggle">
              <Link to="#" className="close-menu-bars" onClick={showSidebar}>
                <AiIcons.AiOutlineClose />
              </Link>
            </div>
            <div className="user-profile">
              <img className="user-img" src={user} alt="user avatar" />
              <span className="user-name">{userProfile?.username}</span>
              <span className="user-type">{userProfile?.role}</span>
            </div>
            <ul className="nav-menu-items" onClick={showSidebar}>
              {SidebarData.map((item, index) => {
                return (
                  <li
                    key={index}
                    className={`${item.cName} ${
                      location.pathname === item.path && "nav-text-active"
                    }`}
                  >
                    <Link to={item.path}>
                      {item.icon}
                      <span>{item.title}</span>
                    </Link>
                  </li>
                );
              })}
            </ul>
          </div>
          <div
            className="nav-text"
            onClick={() => {
              logoutUser();
            }}
          >
            <span className="logout">
              <IoIcons.IoLogOut />
              <span>Logout</span>
            </span>
          </div>
        </nav>
      </IconContext.Provider>
    </>
  );
}

export default Navbar;
