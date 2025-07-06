import React, { useState } from "react";
import {
  Box,
  Button,
  Typography,
  Card,
  CardContent,
  Chip,
  IconButton,
  Skeleton,
} from "@mui/material";
import { ChevronLeft, ChevronRight } from "@mui/icons-material";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  completeWeaningTaskSchema,
  type CompleteWeaningTaskFormFields,
} from "../../schemas/taskSchemas";
import { handleFormError } from "../../utils/formErrorHandler";
import { useAvailableCages } from "../../hooks/useAvailableCages";
import { getCageLabel, getCageChipColor } from "../../utils/typeMappers";

interface WeaningTaskFormProps {
  onSubmit: (data: CompleteWeaningTaskFormFields) => Promise<void>;
  onCancel: () => void;
  taskId: number;
}

const WeaningTaskForm: React.FC<WeaningTaskFormProps> = ({
  onSubmit,
  onCancel,
  taskId,
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
  } = useAvailableCages({ pageSize: cagesPerPage });

  const {
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<CompleteWeaningTaskFormFields>({
    resolver: zodResolver(completeWeaningTaskSchema),
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

  const getBorderColor = (cageId: number): string => {
    if (selectedCageId === cageId) {
      return "primary.main";
    }

    if (errors.newCageId && selectedCageId === null) {
      return "error.main";
    }

    return "divider";
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

  const renderCageSkeletons = () => (
    <Box
      sx={{
        display: "grid",
        gridTemplateColumns: "repeat(2, 1fr)",
        gap: 2,
        mb: 2,
      }}
    >
      {[1, 2].map((_, index) => (
        <Card key={`skeleton-${index}`} sx={{ p: 2 }}>
          <CardContent sx={{ py: 2 }}>
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                textAlign: "center",
              }}
            >
              <Skeleton
                variant="rectangular"
                width={120}
                height={24}
                sx={{ mb: 1 }}
              />
              <Skeleton
                variant="rectangular"
                width={60}
                height={16}
                sx={{ mb: 1 }}
              />
              <Skeleton variant="rounded" width={60} height={24} />
            </Box>
          </CardContent>
        </Card>
      ))}
    </Box>
  );

  const renderCageContent = () => {
    if (loading) {
      return renderCageSkeletons();
    }

    if (cageError) {
      return (
        <Typography color="error">Error loading cages: {cageError}</Typography>
      );
    }

    if (cages.length === 0) {
      return (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            minHeight: 150,
            border: 1,
            borderColor: "divider",
            borderRadius: 1,
            backgroundColor: "grey.50",
          }}
        >
          <Typography variant="body1" color="text.secondary">
            No empty cages available
          </Typography>
        </Box>
      );
    }

    return (
      <>
        <Box
          sx={{
            display: "grid",
            gridTemplateColumns: "repeat(2, 1fr)",
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
          <Typography
            variant="body2"
            color="error.main"
            sx={{ mb: 2, textAlign: "left" }}
          >
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
              mb: 0,
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
    <Box
      component="form"
      onSubmit={handleSubmit(handleFormSubmit)}
      sx={{ width: "100%" }}
    >
      {errors.root && (
        <Typography
          variant="body2"
          color="error.main"
          sx={{ mb: 2, fontWeight: 500 }}
        >
          {errors.root.message}
        </Typography>
      )}

      {renderCageContent()}

      <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
        <Button
          type="submit"
          variant="contained"
          disabled={isSubmitting || cages.length === 0}
        >
          {isSubmitting ? "Completing..." : "Complete Task"}
        </Button>
      </Box>
    </Box>
  );
};

export default WeaningTaskForm;
