import React, { useState, useEffect } from 'react';
import { Box, Button, TextField, Typography, Card, CardContent, Chip, IconButton, Skeleton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { completeOffspringSeparationTaskSchema, type CompleteOffspringSeparationTaskFormFields } from '../../schemas/taskSchemas';
import { handleFormError } from '../../utils/formErrorHandler';
import { useAvailableCages } from '../../hooks/useAvailableCages';
import { type TaskData } from '../../utils/taskMappers';
import { getCageLabel, getCageChipColor } from '../../utils/typeMappers';

interface OffspringSeparationTaskFormProps {
  onSubmit: (data: CompleteOffspringSeparationTaskFormFields) => Promise<void>;
  onCancel: () => void;
  error?: string | null;
  task: TaskData;
}

const OffspringSeparationTaskForm: React.FC<OffspringSeparationTaskFormProps> = ({ 
  onSubmit, 
  onCancel, 
  error,
  task
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
    setPageIndex
  } = useAvailableCages({ pageSize: cagesPerPage });

  const {
    register,
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<CompleteOffspringSeparationTaskFormFields>({
    resolver: zodResolver(completeOffspringSeparationTaskSchema),
    defaultValues: {
      femaleOffspringCount: 0
    }
  });

  
  useEffect(() => {
    if (error) {
      setError('root', { 
        type: 'manual', 
        message: error 
      });
    }
  }, [error, setError]);

  
  const handleCageSelect = (cage: any) => {
    setSelectedCageId(cage.id);
    setValue('otherCageId', cage.id);
  };

  const handleFormSubmit = async (data: CompleteOffspringSeparationTaskFormFields) => {
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
    
    if (errors.otherCageId && selectedCageId === null) {
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

  
  const renderCageSection = () => {
    if (loading) {
      return renderCageSkeletons();
    }
    
    if (cages.length > 0) {
      return (
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

          {errors.otherCageId && selectedCageId === null && (
            <Typography 
              variant="body2" 
              color="error.main" 
              sx={{ mb: 2, textAlign: 'left' }}
            >
              Please select a cage for offspring separation
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
      );
    }
    
    
    return (
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
          {cageError || "No empty cages available"}
        </Typography>
      </Box>
    );
  };

  return (
    <Box component="form" onSubmit={handleSubmit(handleFormSubmit)} sx={{ width: '100%' }}>
      {/* Display server or root errors as standard text error */}
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
      
      <Box sx={{ mt: 1 }}>
        <TextField
          {...register("femaleOffspringCount", { 
            valueAsNumber: true 
          })}
          label="Female Offspring Count"
          placeholder="Enter number of female offspring"
          type="number"
          
          slotProps={{
            input: {
              inputProps: {
                min: 0,
                step: 1
              }
            }
          }}
          variant="outlined"
          fullWidth
          error={!!errors.femaleOffspringCount}
          helperText={errors.femaleOffspringCount?.message}
          sx={{ 
            mb: 2,
            
            '& input[type=number]::-webkit-inner-spin-button, & input[type=number]::-webkit-outer-spin-button': {
              '-webkit-appearance': 'none',
              margin: 0,
            },
            '& input[type=number]': {
              '-moz-appearance': 'textfield',
            },
          }}
        />
      </Box>

      <Typography variant="h6" sx={{ mb: 2 }}>
        Select Target Cage
      </Typography>

      <Box sx={{ minHeight: 'auto', mb: 3 }}>
        {renderCageSection()}
      </Box>

      <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          type="submit"
          variant="contained"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Completing..." : "Complete Task"}
        </Button>
      </Box>
    </Box>
  );
};

export default OffspringSeparationTaskForm;
