import React from 'react';
import { Box, Button, TextField, Alert } from '@mui/material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { addCageSchema, type AddCageFormFields } from '../../schemas/cageSchemas';
import { handleFormError } from '../../utils/formErrorHandler';

interface CageFormProps {
  onSubmit: (data: AddCageFormFields) => Promise<void>;
  onCancel: () => void;
}

const CageForm: React.FC<CageFormProps> = ({ onSubmit, onCancel }) => {
  const {
    register,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useForm<AddCageFormFields>({
    resolver: zodResolver(addCageSchema),
  });

  const handleFormSubmit = async (data: AddCageFormFields) => {
    try {
      await onSubmit(data);
    } catch (error) {
      handleFormError(error, setError);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit(handleFormSubmit)} sx={{ width: '100%' }}>
      <TextField
        {...register("name")}
        label="Cage Name"
        placeholder="Enter cage name"
        variant="outlined"
        fullWidth
        error={!!errors.name}
        helperText={errors.name?.message}
        sx={{ mb: 2, mt: 2 }}
      />

      {errors.root && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {errors.root.message}
        </Alert>
      )}

      <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
        <Button
          type="submit"
          variant="contained"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Adding..." : "Add Cage"}
        </Button>
      </Box>
    </Box>
  );
};

export default CageForm;