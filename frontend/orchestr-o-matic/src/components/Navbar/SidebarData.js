import React from "react";
import * as FaIcons from "react-icons/fa";
import * as MdIcons from "react-icons/md";
import * as BsIcons from "react-icons/bs";
import * as HiIcons from "react-icons/hi";

export const SidebarData = [
  {
    title: "Dashboard",
    path: "/dashboard",
    icon: <MdIcons.MdSpaceDashboard />,
    cName: "nav-text",
  },
  {
    title: "Orchestra",
    path: "/orchestra",
    icon: <BsIcons.BsFileEarmarkMusicFill />,
    cName: "nav-text",
  },
  {
    title: "Concert",
    path: "/concert",
    icon: <HiIcons.HiTicket />,
    cName: "nav-text",
  },
  {
    title: "Profile",
    path: "/profile",
    icon: <FaIcons.FaUserCircle />,
    cName: "nav-text",
  },
];
