import React from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton } from '@mui/material';
import { Close } from '@mui/icons-material';
import CageForm from '../forms/CageForm';
import type { AddCageFormFields } from '../../schemas/cageSchemas';

interface AddCageModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddCageFormFields) => Promise<void>;
}

const AddCageModal: React.FC<AddCageModalProps> = ({ open, onClose, onSubmit }) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Add New Cage
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent>
        <CageForm onSubmit={onSubmit} onCancel={onClose} />
      </DialogContent>
    </Dialog>
  );
};

export default AddCageModal;
