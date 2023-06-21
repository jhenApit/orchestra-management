import { instance } from "./axios";

export const fetchOrchestra = async () => {
  try {
    const response = await instance.get("http://localhost:7077/api/orchestras");
    const data = response.data;
    const limitedData = data.slice(0, 3); // Limit the data to the first three sections
    return limitedData;
  } catch (error) {
    console.error("Error fetching orchestra:", error);
    return [];
  }
};

export const fetchPlayer = async (playerId) => {
  try {
    const response = await instance.get(
      `http://localhost:7077/api/players/${playerId}`
    );
    return [response.data]; // Wrap the player data in an array for consistency with the map function in the dashboard component
  } catch (error) {
    console.error("Error fetching player data:", error);
    return [];
  }
};
