import React from "react";
import { Box, Button, TextField } from "@mui/material";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  addCustomerSchema,
  type AddCustomerFormFields,
} from "../../schemas/customerSchemas";
import { handleFormError } from "../../utils/formErrorHandler";

interface CustomerFormProps {
  onSubmit: (data: AddCustomerFormFields) => Promise<void>;
  onCancel: () => void;
  error?: string | null;
}

const CustomerForm: React.FC<CustomerFormProps> = ({
  onSubmit,
  onCancel,
  error,
}) => {
  const {
    register,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useForm<AddCustomerFormFields>({
    resolver: zodResolver(addCustomerSchema),
  });

  const handleFormSubmit = async (data: AddCustomerFormFields) => {
    try {
      await onSubmit(data);
    } catch (formError) {
      handleFormError(formError, setError);
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit(handleFormSubmit)}
      sx={{ width: "100%" }}
    >
      {(errors.root || error) && (
        <Box
          sx={{
            mb: 0,
            mt: 0,
            color: "error.main",
            fontSize: "0.875rem",
            fontWeight: 500,
          }}
        >
          {errors.root?.message || error}
        </Box>
      )}

      <TextField
        {...register("firstName")}
        label="First Name"
        placeholder="Enter first name"
        variant="outlined"
        fullWidth
        error={!!errors.firstName}
        helperText={errors.firstName?.message}
        sx={{ mb: 2, mt: 2 }}
      />

      <TextField
        {...register("lastName")}
        label="Last Name"
        placeholder="Enter last name"
        variant="outlined"
        fullWidth
        error={!!errors.lastName}
        helperText={errors.lastName?.message}
        sx={{ mb: 2 }}
      />

      <TextField
        {...register("email")}
        label="Email"
        placeholder="Enter email address"
        type="email"
        variant="outlined"
        fullWidth
        error={!!errors.email}
        helperText={errors.email?.message}
        sx={{ mb: 2 }}
      />

      <TextField
        {...register("phoneNum")}
        label="Phone Number"
        placeholder="Enter phone number"
        variant="outlined"
        fullWidth
        error={!!errors.phoneNum}
        helperText={errors.phoneNum?.message}
        sx={{ mb: 2 }}
      />

      <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
        <Button type="submit" variant="contained" disabled={isSubmitting}>
          {isSubmitting ? "Adding..." : "Add Customer"}
        </Button>
      </Box>
    </Box>
  );
};

export default CustomerForm;
