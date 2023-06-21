import { instance } from "./axios";

export const fetchConcertData = async () => {
  try {
    const response = await instance.get("/concerts");

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const addConcert = async (data) => {
  try {
    const response = await instance.post("/concerts", data);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const editConcert = async (id, data) => {
  try {
    const response = await instance.put(`/concerts/${id}`, data);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const deleteConcert = async (id) => {
  try {
    const response = await instance.delete(`/concerts/${id}`);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};
