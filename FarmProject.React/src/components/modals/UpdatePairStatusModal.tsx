import React from "react";
import { Dialog, DialogTitle, DialogContent, IconButton } from "@mui/material";
import { Close } from "@mui/icons-material";
import UpdatePairStatusForm from "../forms/UpdatePairStatusForm";
import type { UpdatePairStatusFormFields } from "../../schemas/pairSchemas";
import type { PairData } from "../../utils/pairMappers";

interface UpdatePairStatusModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: UpdatePairStatusFormFields) => Promise<void>;
  pair: PairData | null;
  error?: string | null;
}

const UpdatePairStatusModal: React.FC<UpdatePairStatusModalProps> = ({
  open,
  onClose,
  onSubmit,
  pair,
  error,
}) => {
  if (!pair) return null;

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Update Pair Status - Pair ID: {pair.pairId}
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 3 }}>
        <UpdatePairStatusForm onSubmit={onSubmit} error={error} />
      </DialogContent>
    </Dialog>
  );
};

export default UpdatePairStatusModal;
