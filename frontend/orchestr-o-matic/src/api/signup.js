import { instance } from "./axios";

export const addUser = async (user) => {
  try {
    const response = await instance.post(
      "http://localhost:7077/api/users",
      user
    );

    return response.data;
  } catch (error) {
    console.error("Error during sign-up:", error.response.data);
    throw new Error("Invalid credentials");
  }
};

export const createConductor = async (name, userId) => {
  try {
    const response = await instance.post(
      "http://localhost:7077/api/conductors",
      { name, userId }
    );

    return response.data;
  } catch (error) {
    throw new Error("An error occurred while creating the conductor.");
  }
};

export const createPlayer = async (name, userId) => {
  try {
    const response = await instance.post("http://localhost:7077/api/players", {
      name,
      userId,
    });

    return response.data;
  } catch (error) {
    throw new Error("An error occurred while creating the player.");
  }
};
