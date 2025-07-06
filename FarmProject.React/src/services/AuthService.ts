import { apiClient } from "../api/config";

export const AuthService = {
  async login(email: string, password: string) {
    try {
      const res = await apiClient.post("/users/login", { email, password });
      return res.data;
    } catch (err) {
      return Promise.reject(err);
    }
  },
  async register(data: {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    role: string | number;
  }) {
    const roleNum =
      typeof data.role === "number"
        ? data.role
        : data.role === "Logistics"
        ? 0
        : 1;
    const payload = { ...data, role: roleNum };
    try {
      const res = await apiClient.post("/users/register", payload);
      return res.data;
    } catch (err) {
      return Promise.reject(err);
    }
  },
};
