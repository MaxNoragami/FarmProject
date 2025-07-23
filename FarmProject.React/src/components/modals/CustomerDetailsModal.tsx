import React, { useState, useEffect } from "react";
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
} from "@mui/material";
import { Close, ChevronLeft, ChevronRight } from "@mui/icons-material";
import { CustomerService } from "../../api/services/customerService";
import { type CustomerWithOrders } from "../../types/Customer";
import OrderCard from "../customers/OrderCard";

interface CustomerDetailsModalProps {
  open: boolean;
  onClose: () => void;
  customerId: number | null;
}

const CustomerDetailsModal: React.FC<CustomerDetailsModalProps> = ({
  open,
  onClose,
  customerId,
}) => {
  const [customer, setCustomer] = useState<CustomerWithOrders | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [orderPage, setOrderPage] = useState(0);
  const ordersPerPage = 2;

  useEffect(() => {
    if (open && customerId) {
      fetchCustomerDetails();
    }
  }, [open, customerId]);

  const fetchCustomerDetails = async () => {
    if (!customerId) return;

    setLoading(true);
    setError(null);
    try {
      const customerData = await CustomerService.getCustomerById(customerId);
      setCustomer(customerData);
      setOrderPage(0);
    } catch (err: any) {
      setError(
        err?.response?.data?.message ||
          err?.message ||
          "Failed to fetch customer details"
      );
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    onClose();

    setTimeout(() => {
      setCustomer(null);
      setError(null);
      setOrderPage(0);
    }, 300);
  };

  const totalOrderPages = customer
    ? Math.ceil(customer.orders.length / ordersPerPage)
    : 0;

  const paginatedOrders = customer
    ? customer.orders.slice(
        orderPage * ordersPerPage,
        (orderPage + 1) * ordersPerPage
      )
    : [];

  const handlePreviousOrderPage = () => {
    setOrderPage((prev) => Math.max(0, prev - 1));
  };

  const handleNextOrderPage = () => {
    setOrderPage((prev) => Math.min(totalOrderPages - 1, prev + 1));
  };

  const renderOrderSkeletons = () => (
    <Grid container spacing={2}>
      {Array.from(new Array(ordersPerPage)).map((_, index) => (
        <Grid size={{ xs: 6, md: 6 }} key={`skeleton-${index}`}>
          <Box
            sx={{ p: 2, border: 1, borderColor: "divider", borderRadius: 1 }}
          >
            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "flex-start",
                mb: 2,
              }}
            >
              <Skeleton variant="text" width={100} height={28} />
              <Skeleton variant="rounded" width={80} height={24} />
            </Box>
            <Skeleton variant="text" width={60} height={16} />
            <Skeleton variant="text" width={120} height={20} />
          </Box>
        </Grid>
      ))}
    </Grid>
  );

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      maxWidth="sm"
      fullWidth
      TransitionProps={{
        onExited: () => {
          setCustomer(null);
          setError(null);
          setOrderPage(0);
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
        Customer Details
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
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: 2,
                    mb: 3,
                  }}
                >
                  <Box>
                    <Skeleton variant="text" width={80} height={20} />
                    <Skeleton variant="text" width={120} height={28} />
                  </Box>
                  <Box>
                    <Skeleton variant="text" width={80} height={20} />
                    <Skeleton variant="text" width={150} height={28} />
                  </Box>
                </Box>
                <Divider sx={{ my: 2 }} />
                <Skeleton
                  variant="text"
                  width={100}
                  height={24}
                  sx={{ mb: 2 }}
                />
                {renderOrderSkeletons()}
              </Box>
            );
          }

          if (customer) {
            return (
              <Box>
                {/* Customer Info */}
                <Box sx={{ mb: 3 }}>
                  <Typography variant="h5" sx={{ mb: 2 }}>
                    {customer.firstName} {customer.lastName}
                  </Typography>

                  <Box
                    sx={{
                      display: "grid",
                      gridTemplateColumns: { xs: "1fr", sm: "1fr 1fr" },
                      gap: 2,
                    }}
                  >
                    <Box>
                      <Typography variant="body2" color="text.secondary">
                        EMAIL
                      </Typography>
                      <Typography variant="body1" fontWeight="medium">
                        {customer.email}
                      </Typography>
                    </Box>
                    <Box>
                      <Typography variant="body2" color="text.secondary">
                        PHONE
                      </Typography>
                      <Typography variant="body1" fontWeight="medium">
                        {customer.phoneNum}
                      </Typography>
                    </Box>
                  </Box>
                </Box>

                <Divider sx={{ my: 2 }} />

                {/* Orders Section */}
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
                      Orders ({customer.orders.length})
                    </Typography>

                    {totalOrderPages > 1 && (
                      <Box
                        sx={{ display: "flex", alignItems: "center", gap: 1 }}
                      >
                        <IconButton
                          onClick={handlePreviousOrderPage}
                          disabled={orderPage === 0}
                          size="small"
                        >
                          <ChevronLeft />
                        </IconButton>

                        <Typography variant="body2" color="text.secondary">
                          {orderPage + 1} of {totalOrderPages}
                        </Typography>

                        <IconButton
                          onClick={handleNextOrderPage}
                          disabled={orderPage === totalOrderPages - 1}
                          size="small"
                        >
                          <ChevronRight />
                        </IconButton>
                      </Box>
                    )}
                  </Box>

                  {customer.orders.length === 0 ? (
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
                        No orders found for this customer
                      </Typography>
                    </Box>
                  ) : (
                    <Grid container spacing={2}>
                      {paginatedOrders.map((order) => (
                        <Grid size={{ xs: 6, md: 6 }} key={order.id}>
                          <OrderCard order={order} />
                        </Grid>
                      ))}
                    </Grid>
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

export default CustomerDetailsModal;
