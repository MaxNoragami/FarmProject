import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Box, Typography, IconButton } from '@mui/material';
import { Close, Add, Remove } from '@mui/icons-material';
import { type RabbitData } from '../../utils/rabbitMappers';

interface BirthModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (offspringCount: number) => Promise<void>;
  rabbit: RabbitData | null;
  error: string | null;
}

const BirthModal: React.FC<BirthModalProps> = ({
  open,
  onClose,
  onSubmit,
  rabbit,
  error
}) => {
  const [offspringCount, setOffspringCount] = useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleIncrease = () => {
    setOffspringCount(prev => prev + 1);
  };

  const handleDecrease = () => {
    setOffspringCount(prev => Math.max(0, prev - 1));
  };

  const handleConfirm = async () => {
    if (!rabbit) return;
    
    setIsSubmitting(true);
    try {
      await onSubmit(offspringCount);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Register Birth
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pb: 0 }}> 
        {error && (
          <Typography 
            variant="body2" 
            color="error.main" 
            sx={{ 
              mb: 0, 
              mt: 1,
              fontWeight: 500,
              display: 'block'
            }}
          >
            {error}
          </Typography>
        )}

        <Box sx={{ mb: 0 }}>
          <Typography variant="body1" sx={{ mb: 2 }}>
            Num of offspring born by {rabbit?.name || 'this rabbit'}:
          </Typography>
          
          <Box sx={{ 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: 'center', 
            gap: 2,
            pb: 0 
          }}>
            <IconButton onClick={handleDecrease} disabled={offspringCount <= 0 || isSubmitting}>
              <Remove />
            </IconButton>
            
            <Typography variant="h4" sx={{ minWidth: '60px', textAlign: 'center' }}>
              {offspringCount}
            </Typography>
            
            <IconButton onClick={handleIncrease} disabled={isSubmitting}>
              <Add />
            </IconButton>
          </Box>
        </Box>
      </DialogContent>
      <DialogActions sx={{ p: 2, pb: 2 }}>
        <Button 
          variant="contained" 
          onClick={handleConfirm}
          disabled={isSubmitting}
          sx={{
            '&.Mui-disabled': {
              color: 'white', 
              opacity: 0.7
            }
          }}
        >
          {isSubmitting ? 'Registering...' : 'Confirm'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default BirthModal;
