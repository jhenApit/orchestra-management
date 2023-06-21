import { instance } from "./axios";

export const fetchLeaderboardData = async (sectionId) => {
  try {
    const response = await instance.get(
      `http://localhost:7077/api/sections/${sectionId}/leaderboards`
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching leaderboard data:", error);
    return [];
  }
};

export const fetchSections = async () => {
  try {
    const response = await instance.get("http://localhost:7077/api/sections");
    return response.data.map((section) => ({
      id: section.id,
      name: section.name,
    }));
  } catch (error) {
    console.error("Error fetching sections:", error);
    return [];
  }
};
