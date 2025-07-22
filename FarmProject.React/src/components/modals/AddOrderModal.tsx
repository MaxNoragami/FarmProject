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
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  Divider,
  Card,
  CardContent,
} from "@mui/material";
import {
  Close,
  Add as AddIcon,
  Delete,
  Person,
  Edit,
} from "@mui/icons-material";
import { type CustomerData } from "../../utils/customerMappers";
import { type OrderRequestItem } from "../../schemas/orderSchemas";
import AddOrderRequestModal from "./AddOrderRequestModal";
import CustomerSelectionModal from "./CustomerSelectionModal";

const offspringTypeLabels: Record<number, string> = {
  0: "None",
  1: "Mixed",
  2: "Male",
  3: "Female",
};
function getOffspringTypeLabel(type: number): string {
  return offspringTypeLabels[type] ?? "Unknown";
}

interface AddOrderModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: {
    customerId: number;
    orderRequests: Array<{ cageId: number; amount: number }>;
  }) => Promise<void>;
  error: string | null;
}

const AddOrderModal: React.FC<AddOrderModalProps> = ({
  open,
  onClose,
  onSubmit,
  error,
}) => {
  const [selectedCustomer, setSelectedCustomer] = useState<CustomerData | null>(
    null
  );
  const [orderRequests, setOrderRequests] = useState<OrderRequestItem[]>([]);
  const [addRequestModalOpen, setAddRequestModalOpen] = useState(false);
  const [customerSelectionModalOpen, setCustomerSelectionModalOpen] =
    useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleAddOrderRequest = (orderRequest: OrderRequestItem) => {
    setOrderRequests((prev) => [...prev, orderRequest]);
    setAddRequestModalOpen(false);
  };

  const handleRemoveOrderRequest = (index: number) => {
    setOrderRequests((prev) => prev.filter((_, i) => i !== index));
  };

  React.useEffect(() => {
    if (open) {
      setSelectedCustomer(null);
      setOrderRequests([]);
    }
  }, [open]);

  const handleSubmit = async () => {
    if (!selectedCustomer || orderRequests.length === 0) return;

    setIsSubmitting(true);
    try {
      await onSubmit({
        customerId: selectedCustomer.id,
        orderRequests: orderRequests.map((req) => ({
          cageId: req.cageId,
          amount: req.amount,
        })),
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setSelectedCustomer(null);
    setOrderRequests([]);
    onClose();
  };

  const handleCustomerSelect = (customer: CustomerData) => {
    setSelectedCustomer(customer);
  };

  return (
    <>
      <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
        <DialogTitle
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          Create New Order
          <IconButton onClick={handleClose} size="small">
            <Close />
          </IconButton>
        </DialogTitle>
        <DialogContent sx={{ pb: 1 }}>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          {/* Customer Selection */}
          <Box sx={{ mb: 2 }}>
            <Typography variant="body1" sx={{ mb: 1 }}>
              Customer
            </Typography>
            {selectedCustomer ? (
              <Card variant="outlined">
                <CardContent sx={{ p: 2, "&:last-child": { pb: 2 } }}>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                    }}
                  >
                    <Box>
                      <Typography variant="body1" fontWeight="medium">
                        {selectedCustomer.firstName} {selectedCustomer.lastName}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        ID: {selectedCustomer.id}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {selectedCustomer.email}
                      </Typography>
                    </Box>
                    <IconButton
                      onClick={() => setCustomerSelectionModalOpen(true)}
                      size="small"
                    >
                      <Edit />
                    </IconButton>
                  </Box>
                </CardContent>
              </Card>
            ) : (
              <Button
                variant="outlined"
                startIcon={<Person />}
                onClick={() => setCustomerSelectionModalOpen(true)}
                fullWidth
                sx={{ py: 2 }}
              >
                Select Customer
              </Button>
            )}
          </Box>

          {/* Order Requests */}
          <Box>
            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                mb: 1,
              }}
            >
              <Typography variant="body1">
                Order Requests ({orderRequests.length})
              </Typography>
              <Button
                variant="outlined"
                startIcon={<AddIcon />}
                onClick={() => setAddRequestModalOpen(true)}
                size="small"
              >
                Add Request
              </Button>
            </Box>

            {orderRequests.length === 0 ? (
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  height: 150,
                  border: 1,
                  borderColor: "divider",
                  borderRadius: 1,
                  backgroundColor: "grey.50",
                  p: 2,
                }}
              >
                <Typography variant="body2" color="text.secondary">
                  No order requests added yet
                </Typography>
              </Box>
            ) : (
              <Box
                sx={{
                  border: 1,
                  borderColor: "divider",
                  borderRadius: 1,
                  height: 180,
                  overflow: "auto",
                  backgroundColor: "grey.50",
                  p: 1,
                }}
              >
                {orderRequests.map((request, index) => (
                  <Card
                    key={index}
                    sx={{
                      mb: 1,
                      boxShadow: 1,
                      border: 1,
                      borderColor: "divider",
                      "&:last-child": { mb: 0 },
                    }}
                  >
                    <CardContent sx={{ p: 2, "&:last-child": { pb: 2 } }}>
                      <Box
                        sx={{
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                        }}
                      >
                        <Box sx={{ flex: 1 }}>
                          <Box
                            sx={{
                              display: "flex",
                              justifyContent: "space-between",
                              mb: 0.5,
                            }}
                          >
                            <Typography variant="body2" fontWeight="medium">
                              Cage Id: {request.cageId}
                            </Typography>
                            <Typography variant="body2" fontWeight="medium">
                              Cage:{" "}
                              {request.cageName || `Cage ${request.cageId}`}
                            </Typography>
                          </Box>
                          <Box
                            sx={{
                              display: "flex",
                              justifyContent: "space-between",
                            }}
                          >
                            <Typography variant="body2" color="text.secondary">
                              {getOffspringTypeLabel(
                                request.offspringType ?? 0
                              )}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                              Amount: {request.amount}
                            </Typography>
                          </Box>
                        </Box>
                        <IconButton
                          onClick={() => handleRemoveOrderRequest(index)}
                          color="error"
                          size="small"
                          sx={{ ml: 2 }}
                        >
                          <Delete />
                        </IconButton>
                      </Box>
                    </CardContent>
                  </Card>
                ))}
              </Box>
            )}
          </Box>
        </DialogContent>
        <DialogActions>
          <Button
            variant="contained"
            onClick={handleSubmit}
            disabled={
              isSubmitting || !selectedCustomer || orderRequests.length === 0
            }
            sx={{ mr: 2, mb: 2 }}
          >
            {isSubmitting ? "Creating..." : "Create Order"}
          </Button>
        </DialogActions>
      </Dialog>

      <CustomerSelectionModal
        open={customerSelectionModalOpen}
        onClose={() => setCustomerSelectionModalOpen(false)}
        onCustomerSelect={handleCustomerSelect}
        selectedCustomer={selectedCustomer}
      />

      <AddOrderRequestModal
        open={addRequestModalOpen}
        onClose={() => setAddRequestModalOpen(false)}
        onSubmit={handleAddOrderRequest}
        error={null}
        existingOrderRequests={orderRequests}
      />
    </>
  );
};

export default AddOrderModal;
