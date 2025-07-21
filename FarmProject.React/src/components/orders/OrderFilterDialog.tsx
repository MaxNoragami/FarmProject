import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
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
import { orderStatusOptions } from "../../types/OrderStatus";

interface OrderFilterDialogProps {
  open: boolean;
  onClose: () => void;
  tempFilters: {
    customerId: string;
    orderStatus: string;
    orderDate: string;
  };
  onTempFiltersChange: (filters: {
    customerId: string;
    orderStatus: string;
    orderDate: string;
  }) => void;
  onClearCustomerId: () => void;
  onClearOrderStatus: () => void;
  onClearOrderDate: () => void;
  onApply: (params: {
    filters: {
      customerId: string;
      orderStatus: string;
      orderDate: string;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => void;
  sortBy: string;
  sortOrder: "asc" | "desc";
  sortableColumns: Array<{ id: string; label: string }>;
  currentFilters: {
    customerId?: string;
    orderStatus?: string;
    orderDate?: string;
  };
}

const OrderFilterDialog: React.FC<OrderFilterDialogProps> = ({
  open,
  onClose,
  tempFilters,
  onTempFiltersChange,
  onClearCustomerId,
  onClearOrderStatus,
  onClearOrderDate,
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
      tempFilters.customerId !== (currentFilters.customerId || "") ||
      tempFilters.orderStatus !== (currentFilters.orderStatus || "") ||
      tempFilters.orderDate !== (currentFilters.orderDate || "");

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
              label="Customer ID"
              value={tempFilters.customerId}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  customerId: e.target.value,
                })
              }
              variant="outlined"
              fullWidth
              sx={{ flex: 1 }}
            />
            {tempFilters.customerId && (
              <IconButton onClick={onClearCustomerId} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <FormControl fullWidth sx={{ flex: 1 }}>
              <InputLabel>Order Status</InputLabel>
              <Select
                value={tempFilters.orderStatus}
                label="Order Status"
                onChange={(e) =>
                  onTempFiltersChange({
                    ...tempFilters,
                    orderStatus: e.target.value,
                  })
                }
              >
                {orderStatusOptions.map((status) => (
                  <MenuItem key={status.value} value={status.value}>
                    {status.label}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            {tempFilters.orderStatus && (
              <IconButton onClick={onClearOrderStatus} size="small">
                <Clear />
              </IconButton>
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <TextField
              label="Order Date"
              type="date"
              value={tempFilters.orderDate}
              onChange={(e) =>
                onTempFiltersChange({
                  ...tempFilters,
                  orderDate: e.target.value,
                })
              }
              variant="outlined"
              fullWidth
              sx={{ flex: 1 }}
              InputLabelProps={{
                shrink: true,
              }}
            />
            {tempFilters.orderDate && (
              <IconButton onClick={onClearOrderDate} size="small">
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

export default OrderFilterDialog;
