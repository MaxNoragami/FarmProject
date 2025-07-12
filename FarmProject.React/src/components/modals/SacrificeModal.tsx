import React, { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Typography,
  IconButton,
  Alert,
} from "@mui/material";
import { Close, Add, Remove } from "@mui/icons-material";
import { type CageData } from "../../utils/cageMappers";

interface SacrificeModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (cageId: number, count: number) => Promise<void>;
  cage: CageData | null;
  error: string | null;
}

const SacrificeModal: React.FC<SacrificeModalProps> = ({
  open,
  onClose,
  onSubmit,
  cage,
  error,
}) => {
  const [sacrificeCount, setSacrificeCount] = useState(1);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const maxCount = cage?.offspringCount || 0;

  const handleIncrease = () => {
    setSacrificeCount((prev) => Math.min(maxCount, prev + 1));
  };

  const handleDecrease = () => {
    setSacrificeCount((prev) => Math.max(1, prev - 1));
  };

  const handleConfirm = async () => {
    if (!cage) return;

    setIsSubmitting(true);
    try {
      await onSubmit(cage.id, sacrificeCount);
    } finally {
      setIsSubmitting(false);
    }
  };

  React.useEffect(() => {
    if (open && cage) {
      setSacrificeCount(1);
    }
  }, [open, cage]);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Sacrifice Offspring
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pb: 0 }}>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Box sx={{ mb: 0 }}>
          <Typography variant="body1" sx={{ mb: 2 }}>
            Num of offspring to sacrifice from {cage?.name || "this cage"}:
          </Typography>

          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              gap: 2,
              pb: 0,
            }}
          >
            <IconButton
              onClick={handleDecrease}
              disabled={sacrificeCount <= 1 || isSubmitting}
            >
              <Remove />
            </IconButton>

            <Typography
              variant="h4"
              sx={{ minWidth: "60px", textAlign: "center" }}
            >
              {sacrificeCount}
            </Typography>

            <IconButton
              onClick={handleIncrease}
              disabled={sacrificeCount >= maxCount || isSubmitting}
            >
              <Add />
            </IconButton>
          </Box>

          <Typography
            variant="body2"
            color="text.secondary"
            sx={{ textAlign: "center", mt: 1 }}
          >
            Max: {maxCount}
          </Typography>
        </Box>
      </DialogContent>
      <DialogActions sx={{ p: 2, pb: 2 }}>
        <Button
          variant="contained"
          onClick={handleConfirm}
          disabled={isSubmitting || sacrificeCount < 1}
          color="error"
          sx={{
            "&.Mui-disabled": {
              color: "white",
              opacity: 0.7,
            },
          }}
        >
          {isSubmitting ? "Processing..." : "Sacrifice"}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default SacrificeModal;
