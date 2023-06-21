import { useState, useCallback } from "react";
import DrawerMenu from "../SideBar/DrawerMenu";
import PortalDrawer from "../SideBar/PortalDrawer";
import "./Navbar(Player Profile).css";
import menuIcon from "../../../images/menu.svg";

const Navbar1 = ({ myConcert, onMyConcertTextClick }) => {
  const [isDrawerMenuOpen, setDrawerMenuOpen] = useState(false);

  const openDrawerMenu = useCallback(() => {
    setDrawerMenuOpen(true);
  }, []);

  const closeDrawerMenu = useCallback(() => {
    setDrawerMenuOpen(false);
  }, []);

  return (
    <>
      <div className="navbar">
        <button className="sidebar-icon" onClick={openDrawerMenu}>
          <img className="menu-icon" alt="" src = {menuIcon} />
        </button>
        <div className="my-concert" onClick={onMyConcertTextClick}>
          {myConcert}
        </div>
      </div>
      {isDrawerMenuOpen && (
        <PortalDrawer placement="Left" onOutsideClick={closeDrawerMenu}>
          <DrawerMenu onClose={closeDrawerMenu} />
        </PortalDrawer>
      )}
    </>
  );
};

export default Navbar1;
