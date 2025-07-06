import React from "react";
import {
  Box,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
} from "@mui/material";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  updatePairStatusSchema,
  type UpdatePairStatusFormFields,
} from "../../schemas/pairSchemas";
import { handleFormError } from "../../utils/formErrorHandler";
import { pairingStatusStringToEnum } from "../../types/PairingStatus";

interface UpdatePairStatusFormProps {
  onSubmit: (data: UpdatePairStatusFormFields) => Promise<void>;
  error?: string | null;
}

const UpdatePairStatusForm: React.FC<UpdatePairStatusFormProps> = ({
  onSubmit,
  error,
}) => {
  const {
    control,
    handleSubmit,
    setError,
    formState: { errors, isSubmitting },
  } = useForm<UpdatePairStatusFormFields>({
    resolver: zodResolver(updatePairStatusSchema),
    defaultValues: {
      pairingStatus: undefined,
    },
  });

  const handleFormSubmit = async (data: UpdatePairStatusFormFields) => {
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
      {/* Display server or root errors */}
      {(errors.root || error) && (
        <Typography
          variant="body2"
          color="error.main"
          sx={{
            mb: 2,
            mt: 1,
            fontWeight: 500,
            display: "block",
          }}
        >
          {errors.root?.message || error}
        </Typography>
      )}

      <Controller
        name="pairingStatus"
        control={control}
        render={({ field }) => (
          <FormControl
            fullWidth
            error={!!errors.pairingStatus}
            sx={{ mb: 3, mt: 2 }}
          >
            <InputLabel>Update Status</InputLabel>
            <Select {...field} label="Update Status" value={field.value || ""}>
              <MenuItem value={pairingStatusStringToEnum.Successful}>
                Successful
              </MenuItem>
              <MenuItem value={pairingStatusStringToEnum.Failed}>
                Failed
              </MenuItem>
            </Select>
            {errors.pairingStatus && (
              <Typography variant="caption" color="error" sx={{ mt: 0.5 }}>
                {errors.pairingStatus.message}
              </Typography>
            )}
          </FormControl>
        )}
      />

      <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
        <Button type="submit" variant="contained" disabled={isSubmitting}>
          {isSubmitting ? "Updating..." : "Confirm"}
        </Button>
      </Box>
    </Box>
  );
};

export default UpdatePairStatusForm;
