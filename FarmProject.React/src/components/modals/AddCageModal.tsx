import React from "react";
import { Dialog, DialogTitle, DialogContent, IconButton } from "@mui/material";
import { Close } from "@mui/icons-material";
import AddCageForm from "../forms/CageForm";
import type { AddCageFormFields } from "../../schemas/cageSchemas";

interface AddCageModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddCageFormFields) => Promise<void>;
  error?: string | null;
}

const AddCageModal: React.FC<AddCageModalProps> = ({
  open,
  onClose,
  onSubmit,
  error,
}) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Add New Cage
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        {error && (
          <div style={{ marginBottom: 16 }}>
            <span style={{ color: "#d32f2f", fontSize: 14 }}>{error}</span>
          </div>
        )}
        <AddCageForm onSubmit={onSubmit} onCancel={onClose} />
      </DialogContent>
    </Dialog>
  );
};

export default AddCageModal;
