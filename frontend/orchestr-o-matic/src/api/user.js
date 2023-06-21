import { instance } from "./axios";

export const getUser = async (id) => {
  try {
    const response = await instance.get(`/users/${id}`);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const editUser = async (id, data) => {
  try {
    const response = await instance.put(`/users/${id}`, data);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};
