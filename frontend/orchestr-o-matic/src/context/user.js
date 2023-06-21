import React, { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getUser } from "../api/user";

export const UserContext = createContext();

const UserProvider = ({ children }) => {
  let navigate = useNavigate();
  const [user, setUser] = useState(null);

  useEffect(() => {
    setUser(JSON.parse(localStorage.getItem("user")));
  }, []);

  const loginUser = (user) => {
    setUser(user);
    localStorage.setItem("user", JSON.stringify(user));
  };

  const logoutUser = () => {
    setUser({});
    localStorage.removeItem("user");
    setTimeout(() => {
      navigate("/signin");
      window.location.reload();
    }, 1000);
  };

  const updateUser = async () => {
    const newUser = await getUser(user.id);
    setUser(newUser);
    localStorage.setItem("user", JSON.stringify(newUser));
  };

  return (
    <UserContext.Provider
      value={{ user, setUser, loginUser, logoutUser, updateUser }}
    >
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
