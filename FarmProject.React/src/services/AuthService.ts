import { apiClient } from '../api/config';

export const AuthService = {
  async login(email: string, password: string) {
    const res = await apiClient.post('/users/login', { email, password });
    return res.data;
  },
  async register(data: {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    role: string | number;
  }) {
    // API expects role as number
    const roleNum = typeof data.role === 'number' ? data.role : (data.role === 'Logistics' ? 0 : 1);
    const payload = { ...data, role: roleNum };
    const res = await apiClient.post('/users/register', payload);
    return res.data;
  }
};
