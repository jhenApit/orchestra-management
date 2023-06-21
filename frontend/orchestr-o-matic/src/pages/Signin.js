import React, { useContext, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import logo from "../images/logo.png";
import authBg from "../images/auth-bg.png";
import "../styles/Signin.scss";
import { getUserSignIn } from "../api/signin";
import { UserContext } from "../context/user";

const SignIn = () => {
  const { loginUser } = useContext(UserContext);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleSignIn = async (e) => {
    e.preventDefault();

    try {
      const response = await getUserSignIn(username, password);
      // Handle successful sign-in
      const user = response;
      loginUser(user);
      if (user.role === "Player") {
        navigate("/dashboard");
      } else if (user.role === "Conductor") {
        navigate("/dashboard");
      }
    } catch (error) {
      setError(error.message);
    }
  };

  const handleResetPassword = (e) => {
    e.preventDefault();
    setUsername("");
    setPassword("");
  };

  return (
    <div className="sign-in-container">
      <div className="left-container">
        <h1>Sign In</h1>
        <form onSubmit={handleSignIn}>
          <label>Username</label>
          <input
            type="text"
            placeholder="Enter your username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <label>Password</label>
          <input
            type="password"
            placeholder="Enter your password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <a href="/reset" onClick={handleResetPassword}>
            Forgot Password?
          </a>
          {error && <p>{error}</p>}
          <button type="submit">Sign In</button>
        </form>
      </div>
      <div
        className="right-container"
        style={{ backgroundImage: `url(${authBg})` }}
      >
        <div className="content">
          <div className="logo-container">
            <Link to="/">
              <img src={logo} alt="Logo" className="logo" />
            </Link>
            <h1>Orchestr-o-matic</h1>
          </div>
          <div className="content-container">
            <h2>Welcome back, friend!</h2>
            <p>
              Sign in now to catch up with your community, discover new
              opportunities, and stay connected with like-minded individuals who
              share your interests. Let's pick up where we left off and continue
              our journey together. See you on the other side!
            </p>
            <h4>Don’t have an account?</h4>
            <button onClick={() => navigate("/signup")}>Sign Up</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SignIn;
