import "./App.css";
import {
  Route,
  Routes,
  useNavigationType,
  useLocation,
  Navigate,
} from "react-router-dom";
import SignUp from "./pages/Signup";
import SignIn from "./pages/Signin";
import Logout from "./pages/Logout";
import Ranking from "./pages/Player/PlayerRanking";
import LandingPage from "./pages/Landing/LandingPage";
import ContactUs from "./pages/Landing/ContactUs";
import AboutUs from "./pages/Landing/AboutUs";
import ApplyForOrchestraForm from "./components/ApplyForOrchestraForm";
import ApplyForOrchestra from "./components/ApplyForOrchestra";
import EditProfile from "./components/PlayerEditProfile";
import PlayerDashboard from "./pages/Player/PlayerDashboard";
import PlayerConcert from "./pages/Player/PlayerConcert";
import PlayerOrchestra from "./pages/Player/PlayerOrchestra";
import PlayerProfile from "./pages/Player/PlayerProfile";
import ConductorDashboard from "./pages/Conductor/ConductorDashboard";
import ConductorOrchestra from "./pages/Conductor/ConductorOrchestra";
import ConductorConcert from "./pages/Conductor/ConductorConcert";
import ConductorProfile from "./pages/Conductor/ConductorProfile";
import { useContext, useEffect } from "react";
import { UserContext } from "./context/user";

function App() {
  const { user } = useContext(UserContext);
  const action = useNavigationType();
  const location = useLocation();
  const pathname = location.pathname;

  useEffect(() => {
    if (action !== "POP") {
      window.scrollTo(0, 0);
    }
  }, [action, pathname]);

  useEffect(() => {
    let title = "";
    let metaDescription = "";

    switch (pathname) {
      case "/":
        title = "";
        metaDescription = "";
        break;
      case "/contact-us":
        title = "";
        metaDescription = "";
        break;
      case "/about-us":
        title = "";
        metaDescription = "";
        break;
      case "/my-concerts":
        title = "";
        metaDescription = "";
        break;
      case "/my-orchestra":
        title = "";
        metaDescription = "";
        break;
      case "/apply-for-orchestra-form":
        title = "";
        metaDescription = "";
        break;
      case "/apply-for-orchestra":
        title = "";
        metaDescription = "";
        break;
      case "/edit-profile":
        title = "";
        metaDescription = "";
        break;
      case "/my-profile":
        title = "";
        metaDescription = "";
        break;
      default:
        break;
    }

    if (title) {
      document.title = title;
    }

    if (metaDescription) {
      const metaDescriptionTag = document.querySelector(
        'head > meta[name="description"]'
      );
      if (metaDescriptionTag) {
        metaDescriptionTag.content = metaDescription;
      }
    }
  }, [pathname]);

  return (
    <div className="App">
      <Routes>
        {/* Routes for Common Page */}
        {!user ? (
          <>
            <Route path="/" element={<LandingPage />} />
            <Route path="/signup" element={<SignUp />} />
            <Route path="/signin" element={<SignIn />} />
            <Route path="/contact-us" element={<ContactUs />} />
            <Route path="/about-us" element={<AboutUs />} />
          </>
        ) : (
          <>
            <Route path="/" element={<Navigate to="/dashboard" />} />
            <Route path="/signup" element={<Navigate to="/dashboard" />} />
            <Route path="/signin" element={<Navigate to="/dashboard" />} />
          </>
        )}
        {/* Routes for Player Screen */}
        {user && user?.role === "Player" && (
          <>
            <Route path="/edit-profile" element={<EditProfile />} />
            <Route path="/my-profile" element={<PlayerProfile />} />
            <Route path="/dashboard" element={<PlayerDashboard />} />
            <Route path="/my-concerts" element={<PlayerConcert />} />
            <Route path="/my-orchestra" element={<PlayerOrchestra />} />
            <Route
              path="/apply-for-orchestra-form"
              element={<ApplyForOrchestraForm />}
            />
            <Route
              path="/apply-for-orchestra"
              element={<ApplyForOrchestra />}
            />

            <Route path="/ranking" element={<Ranking />} />
          </>
        )}
        {/* Routes for Conductor Screen */}
        {user && user?.role === "Conductor" && (
          <>
            <Route path="/dashboard" Component={ConductorDashboard}></Route>
            <Route path="/orchestra" Component={ConductorOrchestra}></Route>
            <Route path="/concert" Component={ConductorConcert}></Route>
            <Route path="/profile" Component={ConductorProfile}></Route>
          </>
        )}
        {user && <Route path="/logout" Component={Logout} />}
      </Routes>
    </div>
  );
}

export default App;
