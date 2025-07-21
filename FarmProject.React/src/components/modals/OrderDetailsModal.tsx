import React, { useEffect, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  IconButton,
  Box,
  Typography,
  Grid,
  Divider,
  Alert,
  Skeleton,
  Chip,
} from "@mui/material";
import { Close, ChevronLeft, ChevronRight } from "@mui/icons-material";
import { OrderService } from "../../api/services/orderService";
import { type OrderDetails, type OrderRequest } from "../../api/types/Order";
import {
  getOrderStatusLabel,
  getOrderStatusColor,
} from "../../types/OrderStatus";
import {
  getOrderRequestStatusLabel,
  getOrderRequestStatusColor,
} from "../../types/OrderRequest";

interface OrderDetailsModalProps {
  open: boolean;
  onClose: () => void;
  orderId: number | null;
}

const OrderDetailsModal: React.FC<OrderDetailsModalProps> = ({
  open,
  onClose,
  orderId,
}) => {
  const [order, setOrder] = useState<OrderDetails | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [requestPage, setRequestPage] = useState(0);
  const requestsPerPage = 2;

  useEffect(() => {
    if (open && orderId) {
      fetchOrderDetails();
    }
  }, [open, orderId]);

  const fetchOrderDetails = async () => {
    if (!orderId) return;
    setLoading(true);
    setError(null);
    try {
      const orderData = await OrderService.getOrderById(orderId);
      setOrder(orderData);
      setRequestPage(0);
    } catch (err: any) {
      setError(
        err?.response?.data?.message ||
          err?.message ||
          "Failed to fetch order details"
      );
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    onClose();
    setTimeout(() => {
      setOrder(null);
      setError(null);
      setRequestPage(0);
    }, 300);
  };

  const totalRequestPages = order
    ? Math.ceil(order.orderRequests.length / requestsPerPage)
    : 0;

  const paginatedRequests = order
    ? order.orderRequests.slice(
        requestPage * requestsPerPage,
        (requestPage + 1) * requestsPerPage
      )
    : [];

  const handlePreviousRequestPage = () => {
    setRequestPage((prev) => Math.max(0, prev - 1));
  };

  const handleNextRequestPage = () => {
    setRequestPage((prev) => Math.min(totalRequestPages - 1, prev + 1));
  };

  const renderRequestSkeletons = () => (
    <Box sx={{ display: "flex", justifyContent: "center", gap: 2 }}>
      {Array.from(new Array(requestsPerPage)).map((_, index) => (
        <Box
          key={`skeleton-${index}`}
          sx={{
            p: 2,
            border: 1,
            borderColor: "divider",
            borderRadius: 1,
            minWidth: 180,
            maxWidth: 220,
            backgroundColor: "#fafafa",
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 1,
          }}
        >
          <Skeleton variant="text" width={100} height={28} />
          <Skeleton variant="text" width={80} height={20} sx={{ mb: 1 }} />
          <Skeleton variant="rounded" width={80} height={24} />
          <Skeleton variant="text" width={60} height={16} />
        </Box>
      ))}
    </Box>
  );

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
    <Dialog
      open={open}
      onClose={handleClose}
      maxWidth="xs"
      fullWidth
      TransitionProps={{
        onExited: () => {
          setOrder(null);
          setError(null);
          setRequestPage(0);
        },
      }}
    >
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Order Details
        <IconButton onClick={handleClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>

      <DialogContent>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        {(() => {
          if (loading) {
            return (
              <Box>
                <Skeleton
                  variant="text"
                  width={120}
                  height={32}
                  sx={{ mb: 2 }}
                />
                <Skeleton
                  variant="text"
                  width={80}
                  height={24}
                  sx={{ mb: 2 }}
                />
                <Divider sx={{ my: 2 }} />
                {renderRequestSkeletons()}
              </Box>
            );
          }

          if (order) {
            return (
              <Box>
                {/* Order Info */}
                <Box sx={{ mb: 3 }}>
                  <Typography variant="h5" sx={{ mb: 2 }}>
                    Order #{order.id}
                  </Typography>
                  <Box
                    sx={{
                      display: "flex",
                      gap: 2,
                      alignItems: "center",
                      mb: 1,
                    }}
                  >
                    <Typography variant="body2" color="text.secondary">
                      Status:
                    </Typography>
                    <Chip
                      label={getOrderStatusLabel(order.orderStatus)}
                      color={getOrderStatusColor(order.orderStatus)}
                      size="small"
                    />
                  </Box>
                  <Box sx={{ display: "flex", gap: 2, mb: 1 }}>
                    <Typography variant="body2" color="text.secondary">
                      Customer ID:
                    </Typography>
                    <Typography variant="body1" fontWeight="medium">
                      {order.customerId}
                    </Typography>
                  </Box>
                  <Box sx={{ display: "flex", gap: 2 }}>
                    <Typography variant="body2" color="text.secondary">
                      Order Date:
                    </Typography>
                    <Typography variant="body1" fontWeight="medium">
                      {new Date(order.orderDate).toLocaleString()}
                    </Typography>
                  </Box>
                </Box>

                <Divider sx={{ my: 2 }} />

                {/* Order Requests Section */}
                <Box>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                      mb: 2,
                    }}
                  >
                    <Typography variant="h6">
                      Order Requests ({order.orderRequests.length})
                    </Typography>
                    {totalRequestPages > 1 && (
                      <Box
                        sx={{ display: "flex", alignItems: "center", gap: 1 }}
                      >
                        <IconButton
                          onClick={handlePreviousRequestPage}
                          disabled={requestPage === 0}
                          size="small"
                        >
                          <ChevronLeft />
                        </IconButton>
                        <Typography variant="body2" color="text.secondary">
                          {requestPage + 1} of {totalRequestPages}
                        </Typography>
                        <IconButton
                          onClick={handleNextRequestPage}
                          disabled={requestPage === totalRequestPages - 1}
                          size="small"
                        >
                          <ChevronRight />
                        </IconButton>
                      </Box>
                    )}
                  </Box>
                  {order.orderRequests.length === 0 ? (
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
                      }}
                    >
                      <Typography variant="body1" color="text.secondary">
                        No order requests found for this order
                      </Typography>
                    </Box>
                  ) : (
                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "center",
                        gap: 2,
                        mb: 2,
                      }}
                    >
                      {paginatedRequests.map((request) => (
                        <Box
                          key={request.id}
                          sx={{
                            border: 1,
                            borderColor: "divider",
                            borderRadius: 1,
                            p: 2,
                            minWidth: 180,
                            maxWidth: 220,
                            backgroundColor: "#fafafa",
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                            gap: 1,
                          }}
                        >
                          <Box
                            sx={{
                              display: "flex",
                              gap: 1,
                              alignItems: "center",
                            }}
                          >
                            <Typography variant="body2" fontWeight="medium">
                              Id: {request.id}
                            </Typography>
                            <Chip
                              label={getOrderRequestStatusLabel(
                                request.orderRequestStatus
                              )}
                              color={getOrderRequestStatusColor(
                                request.orderRequestStatus
                              )}
                              size="small"
                            />
                          </Box>
                          <Typography variant="body2" color="text.secondary">
                            Cage Id: {request.cageId}
                          </Typography>
                          <Typography variant="body2" color="text.secondary">
                            {getOffspringTypeLabel(request.offspringType)}
                          </Typography>
                          <Typography variant="body2" color="text.secondary">
                            Completeness: {request.sacrificedAmount} /{" "}
                            {request.amount}
                          </Typography>
                        </Box>
                      ))}
                    </Box>
                  )}
                </Box>
              </Box>
            );
          }

          return null;
        })()}
      </DialogContent>
    </Dialog>
  );
};

export default OrderDetailsModal;
