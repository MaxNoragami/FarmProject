import React, { useState, useEffect } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Typography,
  IconButton,
  CircularProgress,
  Chip,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";
import {
  Close,
  Add,
  Remove,
  ChevronLeft,
  ChevronRight,
} from "@mui/icons-material";
import { type CageData } from "../../utils/cageMappers";
import { type OrderRequest } from "../../types/OrderRequest";
import { apiClient } from "../../api/config";
import {
  getOrderRequestStatusLabel,
  getOrderRequestStatusColor,
} from "../../types/OrderRequest";

const sacrificationReasons = [
  { value: 0, label: "Other" },
  { value: 1, label: "Health" },
  { value: 2, label: "Order" },
  { value: 3, label: "Age" },
];

interface SacrificeModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (payload: {
    cageId: number;
    amount: number;
    sacrificationReason: number;
    orderRequestId?: number;
  }) => Promise<void>;
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
  const [sacrificationReason, setSacrificationReason] = useState<number>(0);
  const [orderRequests, setOrderRequests] = useState<OrderRequest[]>([]);
  const [orderRequestId, setOrderRequestId] = useState<number | undefined>(
    undefined
  );
  const [orderRequestsLoading, setOrderRequestsLoading] = useState(false);
  const [orderRequestPage, setOrderRequestPage] = useState(1);
  const [orderRequestTotalPages, setOrderRequestTotalPages] = useState(1);
  const [selectedOrderRequestId, setSelectedOrderRequestId] = useState<
    number | undefined
  >(undefined);
  const [serverError, setServerError] = useState<string | null>(null);

  const getMaxCount = () => {
    if (
      sacrificationReason === 2 &&
      selectedOrderRequestId &&
      orderRequests.length > 0
    ) {
      const selectedRequest = orderRequests.find(
        (r) => r.id === selectedOrderRequestId
      );
      if (selectedRequest) {
        return selectedRequest.amount - selectedRequest.sacrificedAmount;
      }
    }

    if (cage?.isSacrificable) {
      const available = cage.offspringCount - cage.reservedOffspringCount;
      return available > 0 ? available : 0;
    }
    return cage?.offspringCount || 0;
  };

  const maxCount = getMaxCount();

  useEffect(() => {
    if (open && cage) {
      setSacrificeCount(1);
      setSacrificationReason(0);
      setSelectedOrderRequestId(undefined);
      setOrderRequests([]);
      setOrderRequestPage(1);
      setOrderRequestTotalPages(1);
    }
  }, [open, cage]);

  useEffect(() => {
    if (open && cage && sacrificationReason === 2) {
      fetchOrderRequests(cage.id, orderRequestPage);
    }
  }, [open, cage, sacrificationReason, orderRequestPage]);

  useEffect(() => {
    if (sacrificationReason === 2 && selectedOrderRequestId) {
      const selectedRequest = orderRequests.find(
        (r) => r.id === selectedOrderRequestId
      );
      if (selectedRequest) {
        const allowed =
          selectedRequest.amount - selectedRequest.sacrificedAmount;
        if (sacrificeCount > allowed) {
          setSacrificeCount(allowed > 0 ? allowed : 1);
        }
      }
    }
  }, [selectedOrderRequestId, sacrificationReason, orderRequests]);

  const fetchOrderRequests = async (cageId: number, pageIndex: number) => {
    setOrderRequestsLoading(true);
    try {
      const response = await apiClient.get(
        `/order-requests?pageIndex=${pageIndex}&pageSize=2&CageId=${cageId}&OrderRequestStatus=0`
      );
      setOrderRequests(response.data.items);
      setOrderRequestTotalPages(response.data.totalPages);
      if (response.data.items.length > 0) {
        setSelectedOrderRequestId(response.data.items[0].id);
      } else {
        setSelectedOrderRequestId(undefined);
      }
    } catch {
      setOrderRequests([]);
      setSelectedOrderRequestId(undefined);
    } finally {
      setOrderRequestsLoading(false);
    }
  };

  const handleIncrease = () => {
    setSacrificeCount((prev) => Math.min(maxCount, prev + 1));
  };

  const handleDecrease = () => {
    setSacrificeCount((prev) => Math.max(1, prev - 1));
  };

  const handleConfirm = async () => {
    const cageId =
      cage && typeof cage.id === "number" ? cage.id : Number(cage?.id);
    if (!cageId) return;
    setIsSubmitting(true);
    setServerError(null);
    try {
      await apiClient.post("/sacrifications", {
        cageId,
        amount: sacrificeCount,
        sacrificationReason,
        ...(sacrificationReason === 2 && selectedOrderRequestId
          ? { orderRequestId: selectedOrderRequestId }
          : {}),
      });
      setServerError(null);
      setIsSubmitting(false);

      if (onSubmit) {
        await onSubmit({
          cageId,
          amount: sacrificeCount,
          sacrificationReason,
          ...(sacrificationReason === 2 && selectedOrderRequestId
            ? { orderRequestId: selectedOrderRequestId }
            : {}),
        });
      }
      onClose();
    } catch (err: any) {
      const msg =
        err?.response?.data?.message ||
        err?.message ||
        "An unexpected error occurred.";
      setServerError(msg);
      setIsSubmitting(false);
    }
  };

  const offspringTypeLabels: Record<number, string> = {
    0: "None",
    1: "Mixed",
    2: "Male",
    3: "Female",
  };
  function getOffspringTypeLabel(type: number): string {
    return offspringTypeLabels[type] ?? "Unknown";
  }

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
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
        {serverError && (
          <Typography
            variant="body2"
            sx={{
              color: "#d32f2f",
              mb: 2,
              fontWeight: 500,
              textAlign: "left",
            }}
          >
            {serverError}
          </Typography>
        )}

        <Box sx={{ mb: 2 }}>
          <Typography variant="body1" sx={{ mb: 2 }}>
            Num of offspring to sacrifice from {cage?.name || "this cage"}:
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

        <FormControl fullWidth sx={{ mb: 2 }}>
          <InputLabel>Sacrification Reason</InputLabel>
          <Select
            value={sacrificationReason}
            label="Sacrification Reason"
            onChange={(e) => setSacrificationReason(Number(e.target.value))}
          >
            {sacrificationReasons.map((reason) => (
              <MenuItem key={reason.value} value={reason.value}>
                {reason.label}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {sacrificationReason === 2 && (
          <Box sx={{ mb: 2 }}>
            <Typography variant="body2" sx={{ mb: 1 }}>
              Select Order Request:
            </Typography>

            {(() => {
              if (orderRequestsLoading) {
                return (
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      minHeight: 40,
                    }}
                  >
                    <CircularProgress size={24} />
                  </Box>
                );
              }
              if (orderRequests.length === 0) {
                return (
                  <Typography variant="body2" color="text.secondary">
                    No order requests found for this cage.
                  </Typography>
                );
              }
              return (
                <>
                  <Box
                    sx={{ display: "flex", justifyContent: "center", gap: 2 }}
                  >
                    {orderRequests.map((req) => (
                      <Box
                        key={req.id}
                        sx={{
                          border: 1,
                          borderColor:
                            selectedOrderRequestId === req.id
                              ? "primary.main"
                              : "divider",
                          borderRadius: 1,
                          p: 2,
                          minWidth: 180,
                          maxWidth: 220,
                          backgroundColor:
                            selectedOrderRequestId === req.id
                              ? "#e3f2fd"
                              : "#fafafa",
                          display: "flex",
                          flexDirection: "column",
                          alignItems: "center",
                          gap: 1,
                          cursor: "pointer",
                          boxShadow: selectedOrderRequestId === req.id ? 2 : 0,
                          transition: "box-shadow 0.2s, border-color 0.2s",
                        }}
                        onClick={() => setSelectedOrderRequestId(req.id)}
                      >
                        <Box
                          sx={{ display: "flex", gap: 1, alignItems: "center" }}
                        >
                          <Typography variant="body2" fontWeight="medium">
                            Id: {req.id}
                          </Typography>
                          <Chip
                            label={getOrderRequestStatusLabel(
                              req.orderRequestStatus
                            )}
                            color={getOrderRequestStatusColor(
                              req.orderRequestStatus
                            )}
                            size="small"
                          />
                        </Box>
                        <Typography variant="body2" color="text.secondary">
                          OrderId: {req.orderId}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {getOffspringTypeLabel(req.offspringType)}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          Completeness: {req.sacrificedAmount} / {req.amount}
                        </Typography>
                      </Box>
                    ))}
                  </Box>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      gap: 1,
                      mt: 1,
                    }}
                  >
                    <IconButton
                      onClick={() =>
                        setOrderRequestPage((p) => Math.max(1, p - 1))
                      }
                      disabled={orderRequestPage === 1}
                      size="small"
                    >
                      <ChevronLeft />
                    </IconButton>
                    <Typography variant="caption" color="text.secondary">
                      {orderRequestPage} of {orderRequestTotalPages}
                    </Typography>
                    <IconButton
                      onClick={() =>
                        setOrderRequestPage((p) =>
                          Math.min(orderRequestTotalPages, p + 1)
                        )
                      }
                      disabled={orderRequestPage === orderRequestTotalPages}
                      size="small"
                    >
                      <ChevronRight />
                    </IconButton>
                  </Box>
                </>
              );
            })()}
          </Box>
        )}
      </DialogContent>
      <DialogActions sx={{ p: 2, pb: 2 }}>
        <Button
          variant="contained"
          onClick={handleConfirm}
          disabled={
            isSubmitting ||
            sacrificeCount < 1 ||
            (sacrificationReason === 2 && !selectedOrderRequestId)
          }
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
