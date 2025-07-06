import React, { useState } from 'react';
import { Box, Button, TextField, Typography, Card, CardContent, Chip, IconButton, Skeleton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { addRabbitSchema, type AddRabbitFormFields } from '../../schemas/rabbitSchemas';
import { handleFormError } from '../../utils/formErrorHandler';
import { useAvailableCages } from '../../hooks/useAvailableCages';
import { type CageData } from '../../utils/cageMappers';
import { getCageLabel, getCageChipColor } from '../../utils/typeMappers';

interface RabbitFormProps {
  onSubmit: (data: AddRabbitFormFields) => Promise<void>;
  onCancel: () => void;
  error?: string | null;
}

const RabbitForm: React.FC<RabbitFormProps> = ({ onSubmit, onCancel, error }) => {
  const [selectedCageId, setSelectedCageId] = useState<number | null>(null);
  const cagesPerPage = 2;

  const {
    cages,
    loading,
    error: cageError,
    totalCount,
    totalPages,
    pageIndex,
    setPageIndex
  } = useAvailableCages({ pageSize: cagesPerPage });

  const {
    register,
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<AddRabbitFormFields>({
    resolver: zodResolver(addRabbitSchema),
  });

  const handleCageSelect = (cage: CageData) => {
    setSelectedCageId(cage.id);
    setValue('cageId', cage.id);
  };

  const handleFormSubmit = async (data: AddRabbitFormFields) => {
    try {
      await onSubmit(data);
    } catch (formError) {
      handleFormError(formError, setError);
    }
  };

  
  const renderCageSkeletons = () => (
    <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
      {[1, 2].map((_, index) => (
        <Card key={`skeleton-${index}`} sx={{ p: 2 }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <Skeleton variant="rectangular" width={120} height={24} sx={{ mb: 1 }} />
            <Skeleton variant="rectangular" width={60} height={16} sx={{ mb: 2 }} />
            <Skeleton variant="rounded" width={60} height={24} />
          </Box>
        </Card>
      ))}
    </Box>
  );

  const getBorderColor = (cageId: number): string => {
    if (selectedCageId === cageId) {
      return 'primary.main';
    }
    
    if (errors.cageId && selectedCageId === null) {
      return 'error.main';
    }
    
    return 'divider';
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

  return (
    <Box component="form" onSubmit={handleSubmit(handleFormSubmit)} sx={{ width: '100%' }}>
      {/* Display server or root errors with proper spacing */}
      {(errors.root || error) && (
        <Typography 
          variant="body2" 
          color="error.main" 
          sx={{ 
            mb: 2, 
            mt: 1, 
            fontWeight: 500,
            display: 'block'
          }}
        >
          {errors.root?.message || error}
        </Typography>
      )}

      <TextField
        {...register("name")}
        label="Rabbit Name"
        placeholder="Enter rabbit name"
        variant="outlined"
        fullWidth
        error={!!errors.name}
        helperText={errors.name?.message}
        sx={{ mb: 3, mt: 2 }}
      />

      <Typography variant="h6" sx={{ mb: 2 }}>
        Select a Cage
      </Typography>

      <Box sx={{ minHeight: 'auto' }}>
        {cages.length > 0 ? (
          <>
            <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
              {cages.map((cage) => (
                <Card 
                  key={cage.id}
                  sx={{ 
                    cursor: 'pointer',
                    border: selectedCageId === cage.id ? 2 : 1,
                    borderColor: getBorderColor(cage.id),
                    '&:hover': {
                      borderColor: 'primary.main',
                      boxShadow: 2
                    }
                  }}
                  onClick={() => handleCageSelect(cage)}
                >
                  <CardContent sx={{ py: 2 }}>
                    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
                      <Typography variant="body1" fontWeight="medium" sx={{ mb: 0.5 }}>
                        {cage.name}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
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

            {errors.cageId && selectedCageId === null && (
              <Typography 
                variant="body2" 
                color="error.main" 
                sx={{ mb: 2, textAlign: 'left' }}
              >
                Please select a cage for the rabbit
              </Typography>
            )}

            {totalPages > 1 && (
              <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1, mt: 2, mb: 0 }}>
                <IconButton 
                  onClick={handlePreviousPage}
                  disabled={pageIndex === 1}
                  size="small"
                >
                  <ChevronLeft />
                </IconButton>
                
                <Typography variant="body2" color="text.secondary">
                  {((pageIndex - 1) * cagesPerPage) + 1}-{Math.min(pageIndex * cagesPerPage, totalCount)} of {totalCount}
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
        ) : (
          <Box sx={{ 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: 'center', 
            minHeight: 150,
            border: 1,
            borderColor: 'divider',
            borderRadius: 1,
            backgroundColor: 'grey.50'
          }}>
            <Typography variant="body1" color="text.secondary">
              No empty cages available
            </Typography>
          </Box>
        )}
      </Box>

      <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          type="submit"
          variant="contained"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Adding..." : "Add Rabbit"}
        </Button>
      </Box>
    </Box>
  );
};

export default RabbitForm;