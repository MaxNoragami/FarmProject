import { type UseFormSetError } from "react-hook-form";

export const handleFormError = (error: any, setError: UseFormSetError<any>) => {
  console.error("Form submission error:", error);

  if (error?.response?.data?.errors) {
    const apiErrors = error.response.data.errors;

    Object.keys(apiErrors).forEach((fieldName) => {
      const field = fieldName.charAt(0).toLowerCase() + fieldName.slice(1);
      setError(field, {
        type: "manual",
        message: Array.isArray(apiErrors[fieldName])
          ? apiErrors[fieldName][0]
          : apiErrors[fieldName],
      });
    });
  } else {
    setError("root", {
      type: "manual",
      message:
        error?.response?.data?.message ||
        error?.message ||
        "An unexpected error occurred. Please try again.",
    });
  }
};
