import { useContext, useEffect } from "react";
import { UserContext } from "../context/user";
import { Navigate } from "react-router-dom";

const Logout = () => {
  const { logoutUser } = useContext(UserContext);

  useEffect(() => {
    logoutUser();
  }, [logoutUser]);

  return <Navigate to="/signin" />;
};

export default Logout;
