import { instance } from "./axios";

export const getUserSignIn = async (username, password) => {
  try {
    const response = await instance.get(
      `http://localhost:7077/users/username-and-password?username=${username}&password=${password}`
    );
    return response.data;
  } catch (error) {
    console.error("Error during sign-in:", error.response.data);
    throw new Error("Invalid credentials");
  }
};

// Function to check if the email exists
export const checkEmailExists = async (email) => {
  try {
    const response = await instance.get(`/check-email-exists?email=${email}`);
    return response.data.exists;
  } catch (error) {
    console.log("Error checking email exists:", error);
    throw error;
  }
};

// Function to reset the password
export const resetPassword = async (email, password) => {
  try {
    const response = await instance.post("/reset-password", {
      email: email,
      password: password,
    });
    return response.data;
  } catch (error) {
    console.log("Error resetting password:", error);
    throw error;
  }
};
