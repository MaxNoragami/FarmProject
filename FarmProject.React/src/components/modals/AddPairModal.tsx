import React from 'react';
import { Dialog, DialogTitle, DialogContent, IconButton } from '@mui/material';
import { Close } from '@mui/icons-material';
import PairForm from '../forms/PairForm';
import type { AddPairFormFields } from '../../schemas/pairSchemas';

interface AddPairModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddPairFormFields) => Promise<void>;
  error?: string | null;
}

const AddPairModal: React.FC<AddPairModalProps> = ({ open, onClose, onSubmit, error }) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Create New Breeding Pair
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        <PairForm onSubmit={onSubmit} onCancel={onClose} error={error} />
      </DialogContent>
    </Dialog>
  );
};

export default AddPairModal;
