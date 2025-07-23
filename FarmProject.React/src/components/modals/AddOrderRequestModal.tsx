import React, { useState, useCallback } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Typography,
  Card,
  CardContent,
  Chip,
  IconButton,
  Alert,
  TextField,
} from "@mui/material";
import {
  ChevronLeft,
  ChevronRight,
  Close,
  Add,
  Remove,
} from "@mui/icons-material";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  addOrderRequestSchema,
  type AddOrderRequestFormFields,
  type OrderRequestItem,
} from "../../schemas/orderSchemas";
import { useCageData } from "../../hooks/useCageData";
import { getCageLabel, getCageChipColor } from "../../utils/typeMappers";

interface AddOrderRequestModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (orderRequest: OrderRequestItem) => void;
  error: string | null;
  existingOrderRequests: OrderRequestItem[];
}

const AddOrderRequestModal: React.FC<AddOrderRequestModalProps> = ({
  open,
  onClose,
  onSubmit,
  error,
  existingOrderRequests,
}) => {
  const [selectedCageId, setSelectedCageId] = useState<number | null>(null);
  const [selectedCage, setSelectedCage] = useState<any>(null);
  const [amount, setAmount] = useState(1);
  const [page, setPage] = useState(0);
  const pageSize = 2;

  const {
    cages,
    loading,
    error: cageError,
    totalCount,
  } = useCageData({
    pageIndex: page,
    pageSize,
    filters: { isSacrificable: true },
    logicalOperator: 0,
    enabled: open,
  });

  const {
    handleSubmit,
    formState: { errors, isSubmitting },
    setValue,
    setError,
  } = useForm<AddOrderRequestFormFields>({
    resolver: zodResolver(addOrderRequestSchema),
  });

  const getReservedAmount = useCallback(
    (cageId: number) => {
      return existingOrderRequests
        .filter((request) => request.cageId === cageId)
        .reduce((total, request) => total + request.amount, 0);
    },
    [existingOrderRequests]
  );

  const getAvailableAmount = useCallback(
    (cage: any) => {
      const backendReserved = cage.reservedOffspringCount;
      const frontendReserved = getReservedAmount(cage.id);
      return Math.max(
        0,
        cage.offspringCount - backendReserved - frontendReserved
      );
    },
    [getReservedAmount]
  );

  const handleCageSelect = (cage: any) => {
    const availableAmount = getAvailableAmount(cage);
    if (availableAmount <= 0) {
      return;
    }

    setSelectedCageId(cage.id);
    setSelectedCage(cage);
    setValue("cageId", cage.id);

    const newAmount = Math.min(amount, availableAmount);
    setAmount(newAmount);
    setValue("amount", newAmount);
  };

  const handleAmountChange = (newAmount: number) => {
    if (!selectedCage) return;

    const availableAmount = getAvailableAmount(selectedCage);
    const validAmount = Math.max(1, Math.min(newAmount, availableAmount));
    setAmount(validAmount);
    setValue("amount", validAmount);
  };

  const handleFormSubmit = async (data: AddOrderRequestFormFields) => {
    if (!selectedCage) return;

    const orderRequest: OrderRequestItem = {
      cageId: data.cageId,
      amount: data.amount,
      cageName: selectedCage.name,
      offspringType: selectedCage.offspringType,
    };

    onSubmit(orderRequest);
  };

  const handleClose = () => {
    setSelectedCageId(null);
    setSelectedCage(null);
    setAmount(1);
    setPage(0);
    onClose();
  };

  React.useEffect(() => {
    if (open) {
      setSelectedCageId(null);
      setSelectedCage(null);
      setAmount(1);
      setPage(0);
    }
  }, [open]);

  React.useEffect(() => {
    setValue("amount", amount);
  }, [amount, setValue]);

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          pb: 1,
        }}
      >
        Add Order Request
        <IconButton onClick={handleClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <DialogContent sx={{ pb: 0, pt: 0 }}>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Typography variant="body1" sx={{ mb: 1 }}>
            Select a Cage
          </Typography>
          {loading ? (
            <Typography>Loading cages...</Typography>
          ) : (
            <>
              {cages.length === 0 ? (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    minHeight: 100,
                    border: 1,
                    borderColor: "divider",
                    borderRadius: 1,
                    backgroundColor: "grey.50",
                    p: 2,
                  }}
                >
                  <Typography variant="body2" color="text.secondary">
                    No sacrificable cages available
                  </Typography>
                </Box>
              ) : (
                <>
                  <Box
                    sx={{
                      display: "flex",
                      flexWrap: "wrap",
                      gap: 2,
                      justifyContent: "center",
                      mb: 2,
                    }}
                  >
                    {cages.map((cage) => {
                      const availableAmount = getAvailableAmount(cage);
                      const isSelectable = availableAmount > 0;

                      return (
                        <Card
                          key={cage.id}
                          sx={{
                            width: 200,
                            cursor: isSelectable ? "pointer" : "not-allowed",
                            border: selectedCageId === cage.id ? 2 : 1,
                            borderColor:
                              selectedCageId === cage.id
                                ? "primary.main"
                                : "divider",
                            opacity: isSelectable ? 1 : 0.5,
                            "&:hover": isSelectable
                              ? {
                                  borderColor: "primary.main",
                                  boxShadow: 1,
                                }
                              : {},
                          }}
                          onClick={() => isSelectable && handleCageSelect(cage)}
                        >
                          <CardContent sx={{ p: 1.5 }}>
                            <Box
                              sx={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "center",
                                textAlign: "center",
                              }}
                            >
                              <Typography
                                variant="body2"
                                fontWeight="medium"
                                sx={{ mb: 0.5 }}
                              >
                                {cage.name}
                              </Typography>
                              <Typography
                                variant="caption"
                                color="text.secondary"
                                sx={{ mb: 0.5 }}
                              >
                                ID: {cage.id}
                              </Typography>
                              <Typography
                                variant="caption"
                                color={
                                  availableAmount > 0
                                    ? "text.secondary"
                                    : "error.main"
                                }
                              >
                                Available: {availableAmount} /{" "}
                                {cage.offspringCount}
                              </Typography>
                            </Box>
                          </CardContent>
                        </Card>
                      );
                    })}
                  </Box>
                  {totalPages > 1 && (
                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        gap: 1,
                        mb: 2,
                      }}
                    >
                      <IconButton
                        onClick={() => setPage(Math.max(0, page - 1))}
                        disabled={page === 0}
                        size="small"
                      >
                        <ChevronLeft />
                      </IconButton>

                      <Typography variant="caption" color="text.secondary">
                        {page + 1} of {totalPages}
                      </Typography>

                      <IconButton
                        onClick={() =>
                          setPage(Math.min(totalPages - 1, page + 1))
                        }
                        disabled={page === totalPages - 1}
                        size="small"
                      >
                        <ChevronRight />
                      </IconButton>
                    </Box>
                  )}
                </>
              )}
            </>
          )}

          <Box sx={{ mt: 1 }}>
            <Typography variant="body2" sx={{ mb: 1, textAlign: "center" }}>
              Amount to order:
            </Typography>
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                gap: 2,
              }}
            >
              <IconButton
                onClick={() => handleAmountChange(amount - 1)}
                disabled={amount <= 1}
                size="small"
              >
                <Remove />
              </IconButton>

              <Typography
                variant="h5"
                sx={{
                  minWidth: 50,
                  textAlign: "center",
                  fontWeight: "bold",
                }}
              >
                {amount}
              </Typography>

              <IconButton
                onClick={() => handleAmountChange(amount + 1)}
                disabled={
                  !selectedCage ||
                  amount >= getAvailableAmount(selectedCage || {})
                }
                size="small"
              >
                <Add />
              </IconButton>
            </Box>
            {selectedCage && (
              <Typography
                variant="caption"
                color="text.secondary"
                sx={{ textAlign: "center", mt: 1, display: "block" }}
              >
                Max: {getAvailableAmount(selectedCage)}
              </Typography>
            )}
          </Box>

          {errors.cageId && (
            <Typography
              variant="caption"
              color="error.main"
              sx={{ mt: 1, display: "block" }}
            >
              {errors.cageId.message}
            </Typography>
          )}
        </DialogContent>
        <DialogActions sx={{ p: 2, pt: 1 }}>
          <Button
            type="submit"
            variant="contained"
            disabled={
              isSubmitting ||
              !selectedCage ||
              amount <= 0 ||
              (selectedCage && getAvailableAmount(selectedCage) <= 0)
            }
            fullWidth
          >
            {isSubmitting ? "Adding..." : "Add Request"}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default AddOrderRequestModal;
