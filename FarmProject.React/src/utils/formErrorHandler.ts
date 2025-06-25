import type { UseFormSetError } from 'react-hook-form';

export const handleFormError = <T extends Record<string, any>>(
  error: any,
  setError: UseFormSetError<T>
) => {
  if (!error.response && !error.status) {
    setError("root" as any, {
      message: "Network error. Please check your connection and try again.",
    });
    return;
  }

  const status = error.response?.status || error.status;
  const errorData = error.response?.data || error;
  
  switch (status) {
    case 400:
      setError("root" as any, {
        message: errorData?.message || "Please check your input and try again.",
      });
      break;
      
    case 401:
      setError("root" as any, {
        message: "You are not authorized to perform this action. Please log in again.",
      });
      break;
      
    case 403:
      setError("root" as any, {
        message: "You don't have permission to perform this action.",
      });
      break;
      
    case 404:
      setError("root" as any, {
        message: "The requested resource was not found.",
      });
      break;
      
    case 500:
      setError("root" as any, {
        message: "Server error. Please try again later.",
      });
      break;
      
    default:
      setError("root" as any, {
        message: errorData?.message || "An unexpected error occurred. Please try again.",
      });
      break;
  }
};