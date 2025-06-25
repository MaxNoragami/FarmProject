import React, { useState, useMemo } from 'react';
import { Box, Button, TextField, Alert, Typography, Card, CardContent, Chip, IconButton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { addRabbitSchema, type AddRabbitFormFields } from '../../schemas/rabbitSchemas';
import { handleFormError } from '../../utils/formErrorHandler';
import { mockCagesData, type CageData } from '../../data/mockCageData';

interface RabbitFormProps {
  onSubmit: (data: AddRabbitFormFields) => Promise<void>;
  onCancel: () => void;
}

const RabbitForm: React.FC<RabbitFormProps> = ({ onSubmit, onCancel }) => {
  const [selectedCageId, setSelectedCageId] = useState<number | null>(null);
  const [cagePage, setCagePage] = useState(1);
  const cagesPerPage = 2;

  const {
    register,
    handleSubmit,
    setError,
    setValue,
    formState: { errors, isSubmitting },
  } = useForm<AddRabbitFormFields>({
    resolver: zodResolver(addRabbitSchema),
  });

  // Filter unoccupied cages
  const unoccupiedCages = useMemo(() => {
    return mockCagesData.filter(cage => cage.rabbitId === null);
  }, []);

  // Pagination for cages
  const paginatedCages = useMemo(() => {
    const startIndex = (cagePage - 1) * cagesPerPage;
    return unoccupiedCages.slice(startIndex, startIndex + cagesPerPage);
  }, [unoccupiedCages, cagePage, cagesPerPage]);

  const totalCagePages = Math.ceil(unoccupiedCages.length / cagesPerPage);

  const handleCageSelect = (cage: CageData) => {
    setSelectedCageId(cage.id);
    setValue('cageId', cage.id);
  };

  const handleFormSubmit = async (data: AddRabbitFormFields) => {
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

      <Box sx={{ mb: 2, minHeight: 'auto' }}>
        {paginatedCages.length > 0 ? (
          <>
            <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mb: 2 }}>
              {paginatedCages.map((cage) => (
                <Card 
                  key={cage.id}
                  sx={{ 
                    cursor: 'pointer',
                    border: selectedCageId === cage.id ? 2 : 1,
                    borderColor: selectedCageId === cage.id 
                      ? 'primary.main' 
                      : errors.cageId && selectedCageId === null
                        ? 'error.main' 
                        : 'divider',
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
                        label="Empty" 
                        color="success" 
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

            {totalCagePages > 1 && (
              <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1 }}>
                <IconButton 
                  onClick={() => setCagePage(prev => Math.max(1, prev - 1))}
                  disabled={cagePage === 1}
                  size="small"
                >
                  <ChevronLeft />
                </IconButton>
                
                <Typography variant="body2" color="text.secondary">
                  {((cagePage - 1) * cagesPerPage) + 1}-{Math.min(cagePage * cagesPerPage, unoccupiedCages.length)} of {unoccupiedCages.length}
                </Typography>
                
                <IconButton 
                  onClick={() => setCagePage(prev => Math.min(totalCagePages, prev + 1))}
                  disabled={cagePage === totalCagePages}
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
          {isSubmitting ? "Adding..." : "Add Rabbit"}
        </Button>
      </Box>
    </Box>
  );
};

export default RabbitForm;