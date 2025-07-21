import {
  Typography,
  Box,
  Button,
  Divider,
  useMediaQuery,
  useTheme,
  TablePagination,
  Chip,
  Grid,
  Paper,
  Skeleton,
} from "@mui/material";
import { FilterList, Add } from "@mui/icons-material";
import * as React from "react";
import { Helmet } from "react-helmet-async";
import OrderCard from "../components/orders/OrderCard";
import ErrorAlert from "../components/common/ErrorAlert";
import OrderFilterDialog from "../components/orders/OrderFilterDialog";
import { useOrderData } from "../hooks/useOrderData";
import { getOrderStatusLabel } from "../types/OrderStatus";
import AddOrderModal from "../components/modals/AddOrderModal";
import { OrderService } from "../api/services/orderService";

const OrdersPage = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(12);

  const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
  const [filters, setFilters] = React.useState<{
    customerId?: string;
    orderStatus?: string;
    orderDate?: string;
  }>({});

  const [tempFilters, setTempFilters] = React.useState<{
    customerId: string;
    orderStatus: string;
    orderDate: string;
  }>({ customerId: "", orderStatus: "", orderDate: "" });

  const [sortBy, setSortBy] = React.useState<string>("id");
  const [sortOrder, setSortOrder] = React.useState<"asc" | "desc">("desc");

  const [addModalOpen, setAddModalOpen] = React.useState(false);
  const [addOrderError, setAddOrderError] = React.useState<string | null>(null);

  const apiFilters = React.useMemo(() => {
    const converted: any = {};
    if (filters.customerId && !isNaN(Number(filters.customerId))) {
      converted.customerId = Number(filters.customerId);
    }
    if (filters.orderStatus && !isNaN(Number(filters.orderStatus))) {
      converted.orderStatus = Number(filters.orderStatus);
    }
    if (filters.orderDate) {
      converted.orderDate = filters.orderDate;
    }
    return converted;
  }, [filters]);

  const getApiSortField = (uiSortField: string) => {
    return uiSortField;
  };

  const { orders, loading, error, totalCount, refetch } = useOrderData({
    pageIndex: page,
    pageSize: rowsPerPage,
    filters: apiFilters,
    logicalOperator: 0,
    sort: sortBy ? `${getApiSortField(sortBy)}:${sortOrder}` : undefined,
  });

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  const handleOpenFilterDialog = () => {
    setTempFilters({
      customerId: filters.customerId || "",
      orderStatus: filters.orderStatus || "",
      orderDate: filters.orderDate || "",
    });
    setFilterDialogOpen(true);
  };

  const handleApplyFilters = ({
    filters: modalFilters,
    sortBy: modalSortBy,
    sortOrder: modalSortOrder,
  }: {
    filters: {
      customerId: string;
      orderStatus: string;
      orderDate: string;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => {
    const newFilters: any = {};
    if (modalFilters.customerId.trim())
      newFilters.customerId = modalFilters.customerId.trim();
    if (modalFilters.orderStatus.trim())
      newFilters.orderStatus = modalFilters.orderStatus.trim();
    if (modalFilters.orderDate.trim())
      newFilters.orderDate = modalFilters.orderDate.trim();

    setFilters(newFilters);
    setSortBy(modalSortBy || "id");
    setSortOrder(modalSortOrder || "desc");
    setPage(0);
    setFilterDialogOpen(false);
  };

  const handleClearCustomerIdFilter = () => {
    setFilters((prev) => ({ ...prev, customerId: undefined }));
    setPage(0);
  };

  const handleClearOrderStatusFilter = () => {
    setFilters((prev) => ({ ...prev, orderStatus: undefined }));
    setPage(0);
  };

  const handleClearOrderDateFilter = () => {
    setFilters((prev) => ({ ...prev, orderDate: undefined }));
    setPage(0);
  };

  const FilterChips = () => {
    const hasFilters =
      filters.customerId || filters.orderStatus || filters.orderDate;

    if (!hasFilters) return null;

    return (
      <Box
        sx={{
          display: "flex",
          flexWrap: "wrap",
          gap: 1,
          mb: 2,
          alignItems: "center",
        }}
      >
        <Typography variant="body2" color="text.secondary" sx={{ mr: 1 }}>
          Filters:
        </Typography>

        {filters.customerId && (
          <Chip
            label={`CUSTOMER ID is "${filters.customerId}"`}
            onDelete={handleClearCustomerIdFilter}
            size="small"
            variant="filled"
            sx={{
              backgroundColor: "#e0e0e0",
              color: "#424242",
              "& .MuiChip-deleteIcon": {
                color: "#757575",
                fontSize: "16px",
                "&:hover": {
                  color: "#424242",
                },
              },
            }}
          />
        )}

        {filters.orderStatus && (
          <Chip
            label={`STATUS is "${getOrderStatusLabel(
              Number(filters.orderStatus)
            )}"`}
            onDelete={handleClearOrderStatusFilter}
            size="small"
            variant="filled"
            sx={{
              backgroundColor: "#e0e0e0",
              color: "#424242",
              "& .MuiChip-deleteIcon": {
                color: "#757575",
                fontSize: "16px",
                "&:hover": {
                  color: "#424242",
                },
              },
            }}
          />
        )}

        {filters.orderDate && (
          <Chip
            label={`ORDER DATE is "${filters.orderDate}"`}
            onDelete={handleClearOrderDateFilter}
            size="small"
            variant="filled"
            sx={{
              backgroundColor: "#e0e0e0",
              color: "#424242",
              "& .MuiChip-deleteIcon": {
                color: "#757575",
                fontSize: "16px",
                "&:hover": {
                  color: "#424242",
                },
              },
            }}
          />
        )}
      </Box>
    );
  };

  const hasFilters =
    filters.customerId || filters.orderStatus || filters.orderDate;

  const skeletonCards = Array.from(new Array(rowsPerPage)).map((_, index) => (
    <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={`skeleton-${index}`}>
      <Paper sx={{ p: 2, height: "100%" }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            mb: 2,
          }}
        >
          <Skeleton variant="text" width={120} height={32} />
          <Skeleton variant="rounded" width={80} height={24} />
        </Box>
        <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
          <Box>
            <Skeleton variant="text" width={80} height={16} />
            <Skeleton variant="text" width={40} height={20} />
          </Box>
          <Box>
            <Skeleton variant="text" width={80} height={16} />
            <Skeleton variant="text" width={100} height={20} />
          </Box>
        </Box>
      </Paper>
    </Grid>
  ));

  const mobileSkeleton = Array.from(new Array(rowsPerPage)).map((_, index) => (
    <Grid size={{ xs: 12, sm: 6 }} key={`mobile-skeleton-${index}`}>
      <Paper sx={{ p: 2, height: "100%" }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start",
            mb: 2,
          }}
        >
          <Skeleton variant="text" width={120} height={32} />
          <Skeleton variant="rounded" width={80} height={24} />
        </Box>
        <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
          <Box>
            <Skeleton variant="text" width={80} height={16} />
            <Skeleton variant="text" width={40} height={20} />
          </Box>
          <Box>
            <Skeleton variant="text" width={80} height={16} />
            <Skeleton variant="text" width={100} height={20} />
          </Box>
        </Box>
      </Paper>
    </Grid>
  ));

  const handleOpenAddModal = () => {
    setAddOrderError(null);
    setAddModalOpen(true);
  };

  const handleCloseAddModal = () => {
    setAddModalOpen(false);
    setAddOrderError(null);
  };

  const handleAddOrder = async (data: {
    customerId: number;
    orderRequests: Array<{ cageId: number; amount: number }>;
  }) => {
    setAddOrderError(null);
    try {
      await OrderService.addOrder(data);
      setAddModalOpen(false);
      await refetch();
    } catch (err: any) {
      setAddOrderError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred while creating the order."
      );
      throw err;
    }
  };

  return (
    <>
      <Helmet>
        <title>Orders Management - Farm Project</title>
      </Helmet>

      {isMobile ? (
        <>
          {/* Mobile layout */}
          <Box
            sx={{
              position: "sticky",
              top: 0,
              backgroundColor: "#f5f5f5",
              zIndex: 1,
              px: 2,
              pt: 2,
              pb: 2,
            }}
          >
            <Box
              sx={{
                backgroundColor: "white",
                borderRadius: 1,
                p: 2,
                boxShadow: 1,
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  mb: 2,
                }}
              >
                <Typography variant="h5">Orders</Typography>
                <Box sx={{ display: "flex", gap: 1 }}>
                  <Button
                    variant="outlined"
                    startIcon={<FilterList />}
                    onClick={handleOpenFilterDialog}
                    size="small"
                  >
                    Filter
                  </Button>
                  <Button
                    variant="contained"
                    startIcon={<Add />}
                    onClick={handleOpenAddModal}
                    size="small"
                  >
                    Add
                  </Button>
                </Box>
              </Box>
              <FilterChips />
              <Divider />
            </Box>
          </Box>

          <Box sx={{ flex: 1, overflow: "auto", px: 2 }}>
            <Box sx={{ py: 2 }}>
              {error && <ErrorAlert message={error} onRetry={refetch} />}
              <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2 }}>
                {loading
                  ? mobileSkeleton
                  : orders.map((order) => (
                      <Grid size={{ xs: 12, sm: 6 }} key={order.id}>
                        <OrderCard order={order} />
                      </Grid>
                    ))}
              </Grid>
            </Box>
          </Box>

          <Box sx={{ flexShrink: 0, px: 2, pb: 1, backgroundColor: "#f5f5f5" }}>
            <Paper sx={{ borderRadius: 1 }}>
              <TablePagination
                rowsPerPageOptions={[12, 24, 48]}
                component="div"
                count={totalCount}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                labelRowsPerPage="Rows:"
              />
            </Paper>
          </Box>
        </>
      ) : (
        <>
          {/* Desktop layout */}
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 2,
            }}
          >
            <Typography variant="h5">Orders</Typography>
            <Box sx={{ display: "flex", gap: 1 }}>
              <Button
                variant="outlined"
                startIcon={<FilterList />}
                onClick={handleOpenFilterDialog}
              >
                Filter
              </Button>
              <Button
                variant="contained"
                startIcon={<Add />}
                onClick={handleOpenAddModal}
              >
                Add
              </Button>
            </Box>
          </Box>

          <FilterChips />
          <Divider sx={{ mb: 3 }} />

          {error && <ErrorAlert message={error} onRetry={refetch} />}

          <Paper
            sx={{
              width: "100%",
              overflow: "hidden",
              display: "flex",
              flexDirection: "column",
              height: hasFilters
                ? "calc(100vh - 280px)"
                : "calc(100vh - 240px)",
            }}
          >
            <Box sx={{ flex: 1, overflow: "auto", p: 2 }}>
              <Grid
                container
                rowSpacing={2}
                columnSpacing={{ xs: 1, sm: 2, md: 2 }}
              >
                {loading
                  ? skeletonCards
                  : orders.map((order) => (
                      <Grid
                        size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                        key={order.id}
                      >
                        <OrderCard order={order} />
                      </Grid>
                    ))}
              </Grid>
            </Box>

            <TablePagination
              rowsPerPageOptions={[12, 24, 48]}
              component="div"
              count={totalCount}
              rowsPerPage={rowsPerPage}
              page={page}
              onPageChange={handleChangePage}
              onRowsPerPageChange={handleChangeRowsPerPage}
              labelRowsPerPage="Rows:"
              sx={{
                borderTop: 1,
                borderColor: "divider",
                flexShrink: 0,
              }}
            />
          </Paper>
        </>
      )}

      <OrderFilterDialog
        open={filterDialogOpen}
        onClose={() => setFilterDialogOpen(false)}
        tempFilters={tempFilters}
        onTempFiltersChange={setTempFilters}
        onClearCustomerId={() =>
          setTempFilters((f) => ({ ...f, customerId: "" }))
        }
        onClearOrderStatus={() =>
          setTempFilters((f) => ({ ...f, orderStatus: "" }))
        }
        onClearOrderDate={() =>
          setTempFilters((f) => ({ ...f, orderDate: "" }))
        }
        onApply={handleApplyFilters}
        sortBy={sortBy}
        sortOrder={sortOrder}
        currentFilters={filters}
        sortableColumns={[
          { id: "id", label: "Order ID" },
          { id: "customerId", label: "Customer ID" },
          { id: "orderDate", label: "Order Date" },
        ]}
      />

      <AddOrderModal
        open={addModalOpen}
        onClose={handleCloseAddModal}
        onSubmit={handleAddOrder}
        error={addOrderError}
      />
    </>
  );
};

export default OrdersPage;
