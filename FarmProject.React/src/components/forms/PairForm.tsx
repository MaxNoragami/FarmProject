import React, { useState } from 'react';
import { Box, Button, TextField, Typography, Card, CardContent, Chip, IconButton, Skeleton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { addPairSchema, type AddPairFormFields } from '../../schemas/pairSchemas';
import { handleFormError } from '../../utils/formErrorHandler';
import { useAvailableFemaleRabbits } from '../../hooks/useAvailableFemaleRabbits';
import { getBreedingStatusColor } from '../../types/BreedingStatus';
import { type RabbitData } from '../../utils/rabbitMappers';

interface PairFormProps {
  onSubmit: (data: AddPairFormFields) => Promise<void>;
  onCancel: () => void;
  error?: string | null;
}

const PairForm: React.FC<PairFormProps> = ({ onSubmit, onCancel, error }) => {
  const [selectedFemaleId, setSelectedFemaleId] = useState<number | null>(null);
  const rabbitsPerPage = 2;

  const {
    rabbits,
    loading,
    error: rabbitError,
    totalCount,
    totalPages,
    pageIndex,
    setPageIndex
  } = useAvailableFemaleRabbits({ pageSize: rabbitsPerPage });

  const {
    register,
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<AddPairFormFields>({
    resolver: zodResolver(addPairSchema),
  });

  const handleFemaleSelect = (rabbit: RabbitData) => {
    setSelectedFemaleId(rabbit.rabbitId);
    setValue('femaleRabbitId', rabbit.rabbitId);
  };

  const handleFormSubmit = async (data: AddPairFormFields) => {
    try {
      await onSubmit(data);
    } catch (formError) {
      handleFormError(formError, setError);
    }
  };

  
  const renderRabbitSkeletons = () => (
    <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
      {[1, 2].map((_, index) => (
        <Card key={`skeleton-${index}`} sx={{ p: 2 }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <Skeleton variant="rectangular" width={120} height={24} sx={{ mb: 1 }} />
            <Skeleton variant="rectangular" width={60} height={16} sx={{ mb: 1 }} />
            <Skeleton variant="rectangular" width={60} height={16} sx={{ mb: 2 }} />
            <Skeleton variant="rounded" width={80} height={24} />
          </Box>
        </Card>
      ))}
    </Box>
  );

  const getBorderColor = (rabbitId: number): string => {
    if (selectedFemaleId === rabbitId) {
      return 'primary.main';
    }
    
    if (errors.femaleRabbitId && selectedFemaleId === null) {
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
      {/* Display server or root errors */}
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
        {...register("maleRabbitId", { valueAsNumber: true })}
        label="Male Rabbit ID"
        placeholder="Enter male rabbit ID"
        variant="outlined"
        fullWidth
        error={!!errors.maleRabbitId}
        helperText={errors.maleRabbitId?.message}
        sx={{ mb: 3, mt: 2 }}
      />

      <Typography variant="h6" sx={{ mb: 2 }}>
        Select a Female Rabbit
      </Typography>

      <Box sx={{ mb: 2, minHeight: 'auto' }}>
        {(() => {
          if (loading) {
            return renderRabbitSkeletons();
          }
          
          if (rabbits.length === 0) {
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
                  {rabbitError || "No available female rabbits found"}
                </Typography>
              </Box>
            );
          }

          return (
            <>
              <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
                {rabbits.map((rabbit) => (
                  <Card 
                    key={rabbit.rabbitId}
                    sx={{ 
                      cursor: 'pointer',
                      border: selectedFemaleId === rabbit.rabbitId ? 2 : 1,
                      borderColor: getBorderColor(rabbit.rabbitId),
                      '&:hover': {
                        borderColor: 'primary.main',
                        boxShadow: 2
                      }
                    }}
                    onClick={() => handleFemaleSelect(rabbit)}
                  >
                    <CardContent sx={{ py: 2 }}>
                      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
                        <Typography variant="body1" fontWeight="medium" sx={{ mb: 0.5 }}>
                          {rabbit.name}
                        </Typography>
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                          ID: {rabbit.rabbitId}
                        </Typography>
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                          Cage: {rabbit.cageId}
                        </Typography>
                        <Chip 
                          label={rabbit.status}
                          color={getBreedingStatusColor(rabbit.status)}
                          size="small" 
                        />
                      </Box>
                    </CardContent>
                  </Card>
                ))}
              </Box>

              {errors.femaleRabbitId && selectedFemaleId === null && (
                <Typography 
                  variant="body2" 
                  color="error.main" 
                  sx={{ mb: 2, textAlign: 'left' }}
                >
                  Please select a female rabbit for pairing
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
                    {((pageIndex - 1) * rabbitsPerPage) + 1}-{Math.min(pageIndex * rabbitsPerPage, totalCount)} of {totalCount}
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
        })()}
      </Box>

      <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Button
          type="submit"
          variant="contained"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Creating..." : "Create Pair"}
        </Button>
      </Box>
    </Box>
  );
};

export default PairForm;
