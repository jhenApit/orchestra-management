import { useCallback } from "react";
import { TextField, InputAdornment, Icon } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Navbar1 from "../../components/PlayerScreen/NavigationBars/Navbar(Player Profile)";
import CardWrapper from "../../components/PlayerScreen/MyConcert/CardWrapper";
import "../../styles/Player/PlayerConcert.css";
const MyConcerts = () => {
  const navigate = useNavigate();

  const onMyConcertTextClick = useCallback(() => {
    navigate("/my-profile");
  }, [navigate]);

  return (
    <div className="my-concerts">
      <Navbar1
        myConcert="My Concert"
        onMyConcertTextClick={onMyConcertTextClick}
      />
      <div className="list-of-courses">
        <div className="content-section">
          <TextField
            className="search-bar"
            sx={{ width: 382 }}
            color="primary"
            variant="outlined"
            type="text"
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <Icon>search_sharp</Icon>
                </InputAdornment>
              ),
            }}
            label="Search a Concert"
            placeholder="Eg. Spring Concert"
            size="medium"
            margin="normal"
          />
          <CardWrapper />
        </div>
      </div>
    </div>
  );
};

export default MyConcerts;
