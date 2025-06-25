import axios from 'axios';

export const API_BASE_URL = 'http://localhost:5292/api';

// Hardcoded JWT token
const ACCESS_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYXhjcmFmdG1kQGdtYWlsLmNvbSIsImVtYWlsIjoibWF4Y3JhZnRtZEBnbWFpbC5jb20iLCJGaXJzdE5hbWUiOiJNYXhpbSIsIkxhc3ROYW1lIjoiQWxleGVpIiwicm9sZSI6IkxvZ2lzdGljcyIsIm5iZiI6MTc1MDg4MTQyNiwiZXhwIjoxNzUwODg4NjI2LCJpYXQiOjE3NTA4ODE0MjYsImlzcyI6IkF1dGhBUEkiLCJhdWQiOiJTd2FnZ2VyLUNsaWVudCJ9.xTh9y6W0Adg7ps5ELBfUk0J-iC76b9NcItfnRFpzrE0';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${ACCESS_TOKEN}`,
  },
});

// Add response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    return Promise.reject(error);
  }
);
