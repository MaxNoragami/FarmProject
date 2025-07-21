import React from "react";
import { Dialog, DialogTitle, DialogContent, IconButton } from "@mui/material";
import { Close } from "@mui/icons-material";
import CustomerForm from "../forms/CustomerForm";
import type { AddCustomerFormFields } from "../../schemas/customerSchemas";

interface AddCustomerModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: AddCustomerFormFields) => Promise<void>;
  error?: string | null;
}

const AddCustomerModal: React.FC<AddCustomerModalProps> = ({
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
        Add New Customer
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        <CustomerForm onSubmit={onSubmit} onCancel={onClose} error={error} />
      </DialogContent>
    </Dialog>
  );
};

export default AddCustomerModal;
