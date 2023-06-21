import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import Logo from "../images/logo.png";
import AuthBg from "../images/auth-bg.png";
import "../styles/Signup.scss";
import { addUser, createPlayer, createConductor } from "../api/signup";

const Signup = () => {
  const [role, setRole] = useState("");
  const [name, setFullName] = useState("");
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [formError, setFormError] = useState("");

  const handleSubmitSignup = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    try {
      const user = {
        role,
        username,
        email,
        password,
      };

      const createdUser = await addUser(user);

      if (role === "1") {
        await createConductor(name, createdUser.id);
      } else if (role === "2") {
        await createPlayer(name, createdUser.id);
      }

      setRole("");
      setFullName("");
      setUsername("");
      setEmail("");
      setPassword("");
      setFormError("");

      alert("Signup successful!");
      navigate("/signin");
    } catch (error) {
      console.error("Error signing up:", error);
      setFormError("An error occurred during sign-up. Please try again later.");
    }
  };

  const validateForm = () => {
    if (
      name.trim() === "" ||
      username.trim() === "" ||
      email.trim() === "" ||
      password.trim() === ""
    ) {
      setFormError("Please fill in all fields.");
      return false;
    }

    if (!emailIsValid(email)) {
      setFormError("Please enter a valid email address.");
      return false;
    }

    if (password.length < 8) {
      setFormError("Password must be at least 8 characters long.");
      return false;
    }

    setFormError("");
    return true;
  };

  const emailIsValid = (email) => {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  };

  const navigate = useNavigate();

  const handleSignInClick = () => {
    navigate("/signin");
  };

  return (
    <div className="signup-container">
      <div
        className="left-container"
        style={{ backgroundImage: `url(${AuthBg})` }}
      >
        <div className="content">
          <div className="logo-container">
            <Link to="/">
              <img src={Logo} alt="Logo" className="logo" />
            </Link>
            <h1>Orchestr-o-matic</h1>
          </div>
          <div className="content-container">
            <h2>Hi, friend!</h2>
            <p>
              Join our vibrant community and be part of something great! By
              joining us, you'll gain access to a wealth of resources,
              information, and like-minded individuals who share your passions
              and interests. Sign up today and let's embark on an exciting
              journey together!
            </p>
            <h4>Already have an account?</h4>
            <button onClick={handleSignInClick}>Sign In</button>
          </div>
        </div>
      </div>

      <div className="right-container">
        <h1>Sign Up</h1>
        <form onSubmit={handleSubmitSignup}>
          <div className="form-group">
            <label htmlFor="role">Account Type</label>
            <select
              id="role"
              value={role}
              onChange={(e) => setRole(e.target.value)}
              required
            >
              <option value="">Select an option</option>
              <option value="1">Conductor</option>
              <option value="2">Player</option>
            </select>
          </div>

          <div className="form-group">
            <div className="form-row">
              <div className="form-col">
                <label htmlFor="name">Full Name</label>
                <input
                  type="text"
                  id="name"
                  value={name}
                  onChange={(e) => setFullName(e.target.value)}
                  required
                />
              </div>
              <div className="form-col">
                <label htmlFor="username">Username</label>
                <input
                  type="text"
                  id="username"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
                />
              </div>
            </div>
          </div>

          <div className="form-group1">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="form-group1">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          {formError && <p className="error-message">{formError}</p>}

          <button type="submit">Sign Up</button>
        </form>
      </div>
    </div>
  );
};

export default Signup;
