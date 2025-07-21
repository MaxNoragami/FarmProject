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
  currentFilters: {
    firstName?: string;
    lastName?: string;
    email?: string;
    phoneNum?: string;
  };
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
  currentFilters,
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

  const hasChanges = React.useMemo(() => {
    const filtersChanged =
      tempFilters.firstName !== (currentFilters.firstName || "") ||
      tempFilters.lastName !== (currentFilters.lastName || "") ||
      tempFilters.email !== (currentFilters.email || "") ||
      tempFilters.phoneNum !== (currentFilters.phoneNum || "");

    const sortChanged = tempSortBy !== sortBy || tempSortOrder !== sortOrder;

    return filtersChanged || sortChanged;
  }, [
    tempFilters,
    tempSortBy,
    tempSortOrder,
    sortBy,
    sortOrder,
    currentFilters,
  ]);

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
              variant="outlined"
              fullWidth
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
              variant="outlined"
              fullWidth
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
              variant="outlined"
              fullWidth
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
              variant="outlined"
              fullWidth
              sx={{ flex: 1 }}
            />
            {tempFilters.phoneNum && (
              <IconButton onClick={onClearPhoneNum} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>
        </Box>
        <Box sx={{ display: "flex", gap: 2 }}>
          <FormControl fullWidth sx={{ flex: 1 }}>
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

          <FormControl fullWidth sx={{ flex: 1 }}>
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

        <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 3 }}>
          <Button
            onClick={handleApply}
            variant="contained"
            disabled={!hasChanges}
          >
            Apply
          </Button>
        </Box>
      </DialogContent>
    </Dialog>
  );
};

export default CustomerFilterDialog;
