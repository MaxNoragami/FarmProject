import React from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton } from '@mui/material';
import { Close } from '@mui/icons-material';
import OffspringSeparationTaskForm from '../forms/OffspringSeparationTaskForm';
import type { TaskData } from '../../utils/taskMappers';
import type { CompleteOffspringSeparationTaskFormFields } from '../../schemas/taskSchemas';

interface CompleteOffspringSeparationTaskModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CompleteOffspringSeparationTaskFormFields) => Promise<void>;
  error?: string | null;
  task: TaskData;
}

const CompleteOffspringSeparationTaskModal: React.FC<CompleteOffspringSeparationTaskModalProps> = ({ 
  open, 
  onClose, 
  onSubmit, 
  error, 
  task 
}) => {
  
  const handleClose = (event: any, reason: string) => {
    
    if (error && (reason === 'backdropClick' || reason === 'escapeKeyDown')) {
      return;
    }
    onClose();
  };

  return (
    <Dialog 
      open={open} 
      onClose={handleClose}
      maxWidth="sm" 
      fullWidth
      
      disableEscapeKeyDown={!!error}
    >
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Complete Offspring Separation
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        <OffspringSeparationTaskForm 
          onSubmit={onSubmit} 
          onCancel={onClose} 
          error={error}
          task={task}
        />
      </DialogContent>
    </Dialog>
  );
};

export default CompleteOffspringSeparationTaskModal;
