import React, { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Typography,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  IconButton,
  Alert,
  Skeleton,
  Card,
  CardContent,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import { Close, Search, ChevronLeft, ChevronRight } from "@mui/icons-material";
import {
  useCustomerSearch,
  type SearchField,
} from "../../hooks/useCustomerSearch";
import { type CustomerData } from "../../utils/customerMappers";
import CustomerCard from "../orders/CustomerCard";

interface CustomerSelectionModalProps {
  open: boolean;
  onClose: () => void;
  onCustomerSelect: (customer: CustomerData) => void;
  selectedCustomer: CustomerData | null;
}

const searchFieldOptions: Array<{ value: SearchField; label: string }> = [
  { value: "email", label: "Email" },
  { value: "firstName", label: "First Name" },
  { value: "lastName", label: "Last Name" },
  { value: "phoneNum", label: "Phone Number" },
];

const CustomerSelectionModal: React.FC<CustomerSelectionModalProps> = ({
  open,
  onClose,
  onCustomerSelect,
  selectedCustomer,
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const [searchQuery, setSearchQuery] = useState("");
  const [searchField, setSearchField] = useState<SearchField>("email");
  const [customerPage, setCustomerPage] = useState(1);

  const {
    customers,
    loading,
    error: searchError,
    totalPages,
    search,
    loadPage,
    hasSearched,
  } = useCustomerSearch({ pageSize: 2 });

  React.useEffect(() => {
    if (open && !hasSearched) {
      setCustomerPage(1);
      search("", "email", 1);
    }
  }, [open, hasSearched, search]);

  const handleSearch = async () => {
    setCustomerPage(1);
    await search(searchQuery, searchField, 1);
  };

  const handleCustomerPageChange = async (newPage: number) => {
    setCustomerPage(newPage);
    await loadPage(newPage);
  };

  const handleCustomerSelect = (customer: CustomerData) => {
    onCustomerSelect(customer);
    onClose();
  };

  const handleClose = () => {
    setSearchQuery("");
    setCustomerPage(1);
    onClose();
  };

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
        Select Customer
        <IconButton onClick={handleClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>
      <DialogContent sx={{ pt: 2, px: 3, pb: 3 }}>
        {searchError && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {searchError}
          </Alert>
        )}

        <Box sx={{ display: "flex", gap: 1, mb: 2, mt: 1 }}>
          <TextField
            label="Search"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyPress={(e) => e.key === "Enter" && handleSearch()}
            size="small"
            sx={{ flex: 1 }}
          />
          <FormControl size="small" sx={{ minWidth: 100 }}>
            <InputLabel>Field</InputLabel>
            <Select
              value={searchField}
              label="Field"
              onChange={(e) => setSearchField(e.target.value as SearchField)}
            >
              {searchFieldOptions.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>

        {loading ? (
          <Box
            sx={{
              display: "grid",
              gridTemplateColumns: "1fr",
              gap: 2,
              mx: 1,
            }}
          >
            {Array.from(new Array(2)).map((_, index) => (
              <Box
                key={index}
                sx={{
                  p: 2,
                  border: 1,
                  borderColor: "divider",
                  borderRadius: 1,
                }}
              >
                <Skeleton variant="text" width="100%" height={24} />
                <Skeleton
                  variant="text"
                  width="60%"
                  height={20}
                  sx={{ mt: 1 }}
                />
                <Skeleton
                  variant="text"
                  width="80%"
                  height={20}
                  sx={{ mt: 1 }}
                />
              </Box>
            ))}
          </Box>
        ) : (
          <>
            {customers.length === 0 ? (
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  minHeight: 120,
                  border: 1,
                  borderColor: "divider",
                  borderRadius: 1,
                  backgroundColor: "grey.50",
                  p: 2,
                  mx: 1,
                }}
              >
                <Typography variant="body2" color="text.secondary">
                  No customers found
                </Typography>
              </Box>
            ) : (
              <>
                <Box
                  sx={{
                    display: "grid",
                    gridTemplateColumns: "1fr",
                    gap: 1,
                    mb: 1,
                  }}
                >
                  {customers.map((customer) => (
                    <Card
                      key={customer.id}
                      sx={{
                        cursor: "pointer",
                        border: selectedCustomer?.id === customer.id ? 2 : 1,
                        borderColor:
                          selectedCustomer?.id === customer.id
                            ? "primary.main"
                            : "divider",
                        "&:hover": {
                          borderColor: "primary.main",
                          boxShadow: 1,
                        },
                      }}
                      onClick={() => handleCustomerSelect(customer)}
                    >
                      <CardContent sx={{ p: 1, "&:last-child": { pb: 1 } }}>
                        <Box
                          sx={{
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                            textAlign: "center",
                          }}
                        >
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                              gap: 1,
                              mb: 0,
                            }}
                          >
                            <Typography variant="body2" fontWeight="medium">
                              {customer.firstName} {customer.lastName}
                            </Typography>
                            <Typography
                              variant="caption"
                              color="text.secondary"
                            >
                              (ID: {customer.id})
                            </Typography>
                          </Box>
                          <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{
                              mb: 0,
                              wordBreak: "break-all",
                            }}
                          >
                            {customer.email}
                          </Typography>
                          <Typography variant="caption" color="text.secondary">
                            {customer.phoneNum}
                          </Typography>
                        </Box>
                      </CardContent>
                    </Card>
                  ))}
                </Box>

                {totalPages > 1 && (
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      gap: 1,
                    }}
                  >
                    <IconButton
                      onClick={() => handleCustomerPageChange(customerPage - 1)}
                      disabled={customerPage === 1}
                      size="small"
                    >
                      <ChevronLeft />
                    </IconButton>

                    <Typography variant="caption" color="text.secondary">
                      {customerPage} of {totalPages}
                    </Typography>

                    <IconButton
                      onClick={() => handleCustomerPageChange(customerPage + 1)}
                      disabled={customerPage === totalPages}
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
      </DialogContent>
    </Dialog>
  );
};

export default CustomerSelectionModal;
