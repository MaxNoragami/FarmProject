import { apiClient } from '../api/config';

export const CageService = {
  async addCage(name: string) {
    try {
      const res = await apiClient.post('/cages', { name });
      return res.data;
    } catch (err) {
      return Promise.reject(err);
    }
  }
};
