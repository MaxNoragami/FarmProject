import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  Box,
  Typography,
  Divider,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  IconButton,
} from "@mui/material";
import { Close, Clear } from "@mui/icons-material";

interface CustomerFilterDialogProps {
  open: boolean;
  onClose: () => void;
  tempFilters: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNum: string;
  };
  onTempFiltersChange: (filters: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNum: string;
  }) => void;
  onClearFirstName: () => void;
  onClearLastName: () => void;
  onClearEmail: () => void;
  onClearPhoneNum: () => void;
  onApply: (params: {
    filters: {
      firstName: string;
      lastName: string;
      email: string;
      phoneNum: string;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => void;
  sortBy: string;
  sortOrder: "asc" | "desc";
  sortableColumns: Array<{ id: string; label: string }>;
}

const CustomerFilterDialog: React.FC<CustomerFilterDialogProps> = ({
  open,
  onClose,
  tempFilters,
  onTempFiltersChange,
  onClearFirstName,
  onClearLastName,
  onClearEmail,
  onClearPhoneNum,
  onApply,
  sortBy,
  sortOrder,
  sortableColumns,
}) => {
  const [tempSortBy, setTempSortBy] = React.useState(sortBy);
  const [tempSortOrder, setTempSortOrder] = React.useState<"asc" | "desc">(
    sortOrder
  );

  React.useEffect(() => {
    if (open) {
      setTempSortBy(sortBy);
      setTempSortOrder(sortOrder);
    }
  }, [open, sortBy, sortOrder]);

  const handleApply = () => {
    onApply({
      filters: tempFilters,
      sortBy: tempSortBy,
      sortOrder: tempSortOrder,
    });
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        Filter & Sort Options
        <IconButton onClick={onClose} size="small">
          <Close />
        </IconButton>
      </DialogTitle>

      <DialogContent>
        <Typography variant="h6" sx={{ mb: 2 }}>
          Filters
        </Typography>

        <Box sx={{ display: "flex", flexDirection: "column", gap: 2, mb: 3 }}>
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <TextField
              label="First Name"
              value={tempFilters.firstName}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  firstName: e.target.value,
                })
              }
              size="small"
              sx={{ flex: 1 }}
            />
            {tempFilters.firstName && (
              <IconButton onClick={onClearFirstName} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <TextField
              label="Last Name"
              value={tempFilters.lastName}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  lastName: e.target.value,
                })
              }
              size="small"
              sx={{ flex: 1 }}
            />
            {tempFilters.lastName && (
              <IconButton onClick={onClearLastName} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <TextField
              label="Email"
              value={tempFilters.email}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  email: e.target.value,
                })
              }
              size="small"
              sx={{ flex: 1 }}
            />
            {tempFilters.email && (
              <IconButton onClick={onClearEmail} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <TextField
              label="Phone Number"
              value={tempFilters.phoneNum}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  phoneNum: e.target.value,
                })
              }
              size="small"
              sx={{ flex: 1 }}
            />
            {tempFilters.phoneNum && (
              <IconButton onClick={onClearPhoneNum} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>
        </Box>

        <Divider sx={{ my: 2 }} />

        <Typography variant="h6" sx={{ mb: 2 }}>
          Sort
        </Typography>

        <Box sx={{ display: "flex", gap: 2 }}>
          <FormControl size="small" sx={{ flex: 1 }}>
            <InputLabel>Sort By</InputLabel>
            <Select
              value={tempSortBy}
              label="Sort By"
              onChange={(e) => setTempSortBy(e.target.value)}
            >
              {sortableColumns.map((column) => (
                <MenuItem key={column.id} value={column.id}>
                  {column.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <FormControl size="small" sx={{ flex: 1 }}>
            <InputLabel>Order</InputLabel>
            <Select
              value={tempSortOrder}
              label="Order"
              onChange={(e) =>
                setTempSortOrder(e.target.value as "asc" | "desc")
              }
            >
              <MenuItem value="asc">Ascending</MenuItem>
              <MenuItem value="desc">Descending</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </DialogContent>

      <DialogActions>
        <Button onClick={handleApply} variant="contained">
          Apply
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default CustomerFilterDialog;
