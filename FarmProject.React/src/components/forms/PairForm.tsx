import React, { useState, useMemo } from 'react';
import { Box, Button, TextField, Alert, Typography, Card, CardContent, Chip, IconButton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { addPairSchema, type AddPairFormFields } from '../../schemas/pairSchemas';
import { handleFormError } from '../../utils/formErrorHandler';
import { mockRabbitsData, type RabbitData } from '../../data/mockData';
import { BreedingStatus } from '../../types/BreedingStatus';

interface PairFormProps {
  onSubmit: (data: AddPairFormFields) => Promise<void>;
  onCancel: () => void;
}

const PairForm: React.FC<PairFormProps> = ({ onSubmit, onCancel }) => {
  const [selectedFemaleId, setSelectedFemaleId] = useState<number | null>(null);
  const [femalePage, setFemalePage] = useState(1);
  const femalesPerPage = 2;

  const {
    register,
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<AddPairFormFields>({
    resolver: zodResolver(addPairSchema),
  });

  // Filter available female rabbits
  const availableFemales = useMemo(() => {
    return mockRabbitsData.filter(rabbit => rabbit.status === BreedingStatus.Available);
  }, []);

  // Pagination for females
  const paginatedFemales = useMemo(() => {
    const startIndex = (femalePage - 1) * femalesPerPage;
    return availableFemales.slice(startIndex, startIndex + femalesPerPage);
  }, [availableFemales, femalePage, femalesPerPage]);

  const totalFemalePages = Math.ceil(availableFemales.length / femalesPerPage);

  const handleFemaleSelect = (rabbit: RabbitData) => {
    setSelectedFemaleId(rabbit.rabbitId);
    setValue('femaleRabbitId', rabbit.rabbitId);
  };

  const handleFormSubmit = async (data: AddPairFormFields) => {
    try {
      await onSubmit(data);
    } catch (error) {
      handleFormError(error, setError);
    }
  };

  const handleMaleRabbitIdBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    const value = e.target.value;
    // If the value is not a valid number, clear the field
    if (value && (isNaN(Number(value)) || Number(value) <= 0 || !Number.isInteger(Number(value)))) {
      setValue('maleRabbitId', '' as any);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit(handleFormSubmit)} sx={{ width: '100%' }}>
      <TextField
        {...register("maleRabbitId", { valueAsNumber: true })}
        label="Male Rabbit ID"
        placeholder="Enter male rabbit ID"
        type="number"
        variant="outlined"
        fullWidth
        error={!!errors.maleRabbitId}
        helperText={errors.maleRabbitId?.message}
        onBlur={handleMaleRabbitIdBlur}
        sx={{ 
          mb: 3, 
          mt: 2,
          '& input[type=number]': {
            MozAppearance: 'textfield'
          },
          '& input[type=number]::-webkit-outer-spin-button': {
            WebkitAppearance: 'none',
            margin: 0
          },
          '& input[type=number]::-webkit-inner-spin-button': {
            WebkitAppearance: 'none',
            margin: 0
          }
        }}
      />

      <Typography variant="h6" sx={{ mb: 2 }}>
        Select an Available Female Rabbit
      </Typography>

      <Box sx={{ mb: 2, minHeight: 'auto' }}>
        {paginatedFemales.length > 0 ? (
          <>
            <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
              {paginatedFemales.map((rabbit) => (
                <Card 
                  key={rabbit.rabbitId}
                  sx={{ 
                    cursor: 'pointer',
                    border: selectedFemaleId === rabbit.rabbitId ? 2 : 1,
                    borderColor: selectedFemaleId === rabbit.rabbitId 
                      ? 'primary.main' 
                      : errors.femaleRabbitId && selectedFemaleId === null
                        ? 'error.main' 
                        : 'divider',
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
                      <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                        ID: {rabbit.rabbitId}
                      </Typography>
                      <Chip 
                        label={rabbit.status} 
                        color="success" 
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
                Please select a female rabbit for the pair
              </Typography>
            )}

            {totalFemalePages > 1 && (
              <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1 }}>
                <IconButton 
                  onClick={() => setFemalePage(prev => Math.max(1, prev - 1))}
                  disabled={femalePage === 1}
                  size="small"
                >
                  <ChevronLeft />
                </IconButton>
                
                <Typography variant="body2" color="text.secondary">
                  {((femalePage - 1) * femalesPerPage) + 1}-{Math.min(femalePage * femalesPerPage, availableFemales.length)} of {availableFemales.length}
                </Typography>
                
                <IconButton 
                  onClick={() => setFemalePage(prev => Math.min(totalFemalePages, prev + 1))}
                  disabled={femalePage === totalFemalePages}
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
              No available female rabbits
            </Typography>
          </Box>
        )}
      </Box>

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
          {isSubmitting ? "Creating..." : "Create Pair"}
        </Button>
      </Box>
    </Box>
  );
};

export default PairForm;
