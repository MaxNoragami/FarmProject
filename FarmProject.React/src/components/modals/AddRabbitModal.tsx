import React from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton } from '@mui/material';
import { Close } from '@mui/icons-material';
import RabbitForm from '../forms/RabbitForm';
import type { AddRabbitFormFields } from '../../schemas/rabbitSchemas';

interface AddRabbitModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddRabbitFormFields) => Promise<void>;
}

const AddRabbitModal: React.FC<AddRabbitModalProps> = ({ open, onClose, onSubmit }) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Add New Rabbit
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        <RabbitForm onSubmit={onSubmit} onCancel={onClose} />
      </DialogContent>
    </Dialog>
  );
};

export default AddRabbitModal;
