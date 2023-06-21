import { instance } from "./axios";

export const fetchOrchestraData = async () => {
  try {
    const response = await instance.get("/orchestras");

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const addOrchestra = async (data) => {
  try {
    const response = await instance.post("/orchestras", data);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const editOrchestra = async (id, data) => {
  try {
    const response = await instance.put(`/orchestras/${id}`, data);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};

export const deleteOrchestra = async (id) => {
  try {
    const response = await instance.delete(`/orchestras/${id}`);

    return response.data;
  } catch (error) {
    console.error(error);
  }
};
