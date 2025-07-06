import React, { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Typography,
  Card,
  CardContent,
  Chip,
  IconButton,
  Alert,
} from "@mui/material";
import { ChevronLeft, ChevronRight, Close } from "@mui/icons-material";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  completeWeaningTaskSchema,
  type CompleteWeaningTaskFormFields,
} from "../../schemas/taskSchemas";
import { type TaskData } from "../../utils/taskMappers";
import { useAvailableCages } from "../../hooks/useAvailableCages";
import { getCageLabel, getCageChipColor } from "../../utils/typeMappers";
import { handleFormError } from "../../utils/formErrorHandler";

interface CompleteWeaningTaskModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CompleteWeaningTaskFormFields) => Promise<void>;
  error: string | null;
  task: TaskData;
}

const CompleteWeaningTaskModal: React.FC<CompleteWeaningTaskModalProps> = ({
  open,
  onClose,
  onSubmit,
  error,
  task,
}) => {
  const [selectedCageId, setSelectedCageId] = useState<number | null>(null);
  const cagesPerPage = 2;

  
  const {
    cages,
    loading,
    error: cageError,
    totalCount,
    totalPages,
    pageIndex,
    setPageIndex,
  } = useAvailableCages({
    pageSize: cagesPerPage,
    enabled: open, 
  });

  const {
    control,
    handleSubmit,
    setValue,
    setError,
    formState: { errors, isSubmitting },
  } = useForm<CompleteWeaningTaskFormFields>({
    resolver: zodResolver(completeWeaningTaskSchema),
    defaultValues: {
      newCageId: undefined,
    },
  });

  const handleCageSelect = (cageId: number) => {
    setSelectedCageId(cageId);
    setValue("newCageId", cageId);
  };

  const handleFormSubmit = async (data: CompleteWeaningTaskFormFields) => {
    try {
      await onSubmit(data);
    } catch (formError) {
      handleFormError(formError, setError);
    }
  };

  const handlePreviousPage = () => {
    if (pageIndex > 1) {
      setPageIndex(pageIndex - 1);
    }
  };

  const handleNextPage = () => {
    if (pageIndex < totalPages) {
      setPageIndex(pageIndex + 1);
    }
  };

  const getBorderColor = (cageId: number): string => {
    if (selectedCageId === cageId) {
      return "primary.main";
    }

    if (errors.newCageId && selectedCageId === null) {
      return "error.main";
    }

    return "divider";
  };

  const renderCageSelection = () => {
    if (loading) {
      return <Typography>Loading available cages...</Typography>;
    }

    if (cages.length === 0) {
      return (
        <Alert severity="warning">
          No empty cages available. Please add a new cage first.
        </Alert>
      );
    }

    return (
      <>
        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: 2,
            mb: 2,
          }}
        >
          {cages.map((cage) => (
            <Card
              key={cage.id}
              sx={{
                cursor: "pointer",
                border: selectedCageId === cage.id ? 2 : 1,
                borderColor: getBorderColor(cage.id),
                "&:hover": {
                  borderColor: "primary.main",
                  boxShadow: 2,
                },
              }}
              onClick={() => handleCageSelect(cage.id)}
            >
              <CardContent sx={{ py: 2 }}>
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    textAlign: "center",
                  }}
                >
                  <Typography
                    variant="body1"
                    fontWeight="medium"
                    sx={{ mb: 0.5 }}
                  >
                    {cage.name}
                  </Typography>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ mb: 1 }}
                  >
                    ID: {cage.id}
                  </Typography>
                  <Chip
                    label={getCageLabel(cage)}
                    color={getCageChipColor(cage)}
                    size="small"
                  />
                </Box>
              </CardContent>
            </Card>
          ))}
        </Box>

        {errors.newCageId && (
          <Typography variant="body2" color="error.main" sx={{ mb: 2 }}>
            {errors.newCageId.message}
          </Typography>
        )}

        {totalPages > 1 && (
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: 1,
              mt: 2,
            }}
          >
            <IconButton
              onClick={handlePreviousPage}
              disabled={pageIndex === 1}
              size="small"
            >
              <ChevronLeft />
            </IconButton>

            <Typography variant="body2" color="text.secondary">
              {(pageIndex - 1) * cagesPerPage + 1}-
              {Math.min(pageIndex * cagesPerPage, totalCount)} of {totalCount}
            </Typography>

            <IconButton
              onClick={handleNextPage}
              disabled={pageIndex === totalPages}
              size="small"
            >
              <ChevronRight />
            </IconButton>
          </Box>
        )}
      </>
    );
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Complete Weaning Task
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <DialogContent>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Typography variant="h6" sx={{ mb: 2 }}>
            Select a Cage for the Offspring
          </Typography>

          <Box sx={{ mb: 3 }}>{renderCageSelection()}</Box>

          {task.cageId && (
            <Typography variant="body2" color="text.secondary">
              Original cage ID: {task.cageId}
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained" disabled={isSubmitting}>
            {isSubmitting ? "Completing..." : "Complete Task"}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default CompleteWeaningTaskModal;
