import { TextField, InputAdornment, Icon } from "@mui/material";
import "./ContentSection.css";
import orchestraCardImage1 from "../../../images/orchestra-img@2x.png";

const ContentSection = () => {
  return (
    <div className="content-section1">
      <TextField
        className="search-bar1"
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
        label="Search a Orchestra"
        placeholder="Eg. Classic Youth Concert"
        size="medium"
        margin="normal"
      />
      <div className="cardwrapper3">
        <div className="orchestra1">
          <div className="rec-card-13">
            <img
              className="orchestra-img-icon4"
              alt=""
              src= {orchestraCardImage1}
            />
            <div className="orchestra-details8">
              <div className="orchestra-details-inner6">
                <div className="orchestra-id-parent4">
                  <div className="orchestra-id6">0001</div>
                  <div className="orchestra-name8">Classic Youth Orchestra</div>
                </div>
              </div>
            </div>
          </div>
          </div>
        </div>
      </div>
  );
};

export default ContentSection;
