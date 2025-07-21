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
import CustomerTable from "../components/customers/CustomerTable";
import CustomerCard from "../components/customers/CustomerCard";
import ErrorAlert from "../components/common/ErrorAlert";
import CustomerFilterDialog from "../components/customers/CustomerFilterDialog";
import AddCustomerModal from "../components/modals/AddCustomerModal";
import { useCustomerData } from "../hooks/useCustomerData";
import { CustomerService } from "../api/services/customerService";
import { type CustomerData } from "../utils/customerMappers";
import { type AddCustomerFormFields } from "../schemas/customerSchemas";

const CustomersPage = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);

  const [addModalOpen, setAddModalOpen] = React.useState(false);
  const [addCustomerError, setAddCustomerError] = React.useState<string | null>(
    null
  );

  const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
  const [filters, setFilters] = React.useState<{
    firstName?: string;
    lastName?: string;
    email?: string;
    phoneNum?: string;
  }>({});

  const [tempFilters, setTempFilters] = React.useState<{
    firstName: string;
    lastName: string;
    email: string;
    phoneNum: string;
  }>({ firstName: "", lastName: "", email: "", phoneNum: "" });

  const [sortBy, setSortBy] = React.useState<string>("id");
  const [sortOrder, setSortOrder] = React.useState<"asc" | "desc">("asc");

  const apiFilters = React.useMemo(() => {
    const converted: any = {};
    if (filters.firstName) converted.firstName = filters.firstName;
    if (filters.lastName) converted.lastName = filters.lastName;
    if (filters.email) converted.email = filters.email;
    if (filters.phoneNum) converted.phoneNum = filters.phoneNum;
    return converted;
  }, [filters]);

  const getApiSortField = (uiSortField: string) => {
    return uiSortField;
  };

  const { customers, loading, error, totalCount, refetch } = useCustomerData({
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
      firstName: filters.firstName || "",
      lastName: filters.lastName || "",
      email: filters.email || "",
      phoneNum: filters.phoneNum || "",
    });
    setFilterDialogOpen(true);
  };

  const handleApplyFilters = ({
    filters: modalFilters,
    sortBy: modalSortBy,
    sortOrder: modalSortOrder,
  }: {
    filters: {
      firstName: string;
      lastName: string;
      email: string;
      phoneNum: string;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => {
    const newFilters: any = {};
    if (modalFilters.firstName.trim())
      newFilters.firstName = modalFilters.firstName.trim();
    if (modalFilters.lastName.trim())
      newFilters.lastName = modalFilters.lastName.trim();
    if (modalFilters.email.trim()) newFilters.email = modalFilters.email.trim();
    if (modalFilters.phoneNum.trim())
      newFilters.phoneNum = modalFilters.phoneNum.trim();

    setFilters(newFilters);
    setSortBy(modalSortBy || "id");
    setSortOrder(modalSortOrder || "asc");
    setPage(0);
    setFilterDialogOpen(false);
  };

  const handleClearFirstNameFilter = () => {
    setFilters((prev) => ({ ...prev, firstName: undefined }));
    setPage(0);
  };

  const handleClearLastNameFilter = () => {
    setFilters((prev) => ({ ...prev, lastName: undefined }));
    setPage(0);
  };

  const handleClearEmailFilter = () => {
    setFilters((prev) => ({ ...prev, email: undefined }));
    setPage(0);
  };

  const handleClearPhoneNumFilter = () => {
    setFilters((prev) => ({ ...prev, phoneNum: undefined }));
    setPage(0);
  };

  const handleOpenAddModal = () => {
    setAddCustomerError(null);
    setAddModalOpen(true);
  };

  const handleCloseAddModal = () => {
    setAddModalOpen(false);
    setAddCustomerError(null);
  };

  const handleAddCustomer = async (data: AddCustomerFormFields) => {
    setAddCustomerError(null);
    try {
      await CustomerService.addCustomer(data);
      setAddModalOpen(false);
      await refetch();
    } catch (err: any) {
      setAddCustomerError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred while adding the customer."
      );
      throw err;
    }
  };

  const FilterChips = () => {
    const hasFilters =
      filters.firstName ||
      filters.lastName ||
      filters.email ||
      filters.phoneNum;

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

        {filters.firstName && (
          <Chip
            label={`FIRST NAME contains "${filters.firstName}"`}
            onDelete={handleClearFirstNameFilter}
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

        {filters.lastName && (
          <Chip
            label={`LAST NAME contains "${filters.lastName}"`}
            onDelete={handleClearLastNameFilter}
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

        {filters.email && (
          <Chip
            label={`EMAIL contains "${filters.email}"`}
            onDelete={handleClearEmailFilter}
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

        {filters.phoneNum && (
          <Chip
            label={`PHONE contains "${filters.phoneNum}"`}
            onDelete={handleClearPhoneNumFilter}
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

  return (
    <>
      <Helmet>
        <title>Customers Management - Farm Project</title>
      </Helmet>

      {isMobile ? (
        <>
          {/* Sticky Header */}
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
                <Typography variant="h5">Customers</Typography>

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

          {/* Content Area */}
          <Box
            sx={{
              flex: 1,
              overflow: "auto",
              px: 2,
            }}
          >
            <Box sx={{ py: 2 }}>
              {error && <ErrorAlert message={error} onRetry={refetch} />}

              <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2 }}>
                {loading
                  ? Array.from(new Array(rowsPerPage)).map((_, index) => (
                      <Grid size={{ xs: 12, sm: 6 }} key={`skeleton-${index}`}>
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
                          </Box>

                          <Box
                            sx={{
                              display: "grid",
                              gridTemplateColumns: "1fr 1fr",
                              gap: 2,
                            }}
                          >
                            <Box>
                              <Skeleton variant="text" width={60} height={16} />
                              <Skeleton variant="text" width={40} height={20} />
                            </Box>
                            <Box>
                              <Skeleton variant="text" width={60} height={16} />
                              <Skeleton variant="text" width={40} height={20} />
                            </Box>
                          </Box>
                        </Paper>
                      </Grid>
                    ))
                  : customers.map((customer) => (
                      <Grid size={{ xs: 12, sm: 6 }} key={customer.id}>
                        <CustomerCard customer={customer} />
                      </Grid>
                    ))}
              </Grid>
            </Box>
          </Box>

          {/* Pagination */}
          <Box
            sx={{
              flexShrink: 0,
              px: 2,
              pb: 1,
              backgroundColor: "#f5f5f5",
            }}
          >
            <Paper sx={{ borderRadius: 1 }}>
              <TablePagination
                rowsPerPageOptions={[10, 25, 100]}
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
          {/* Header */}
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 2,
            }}
          >
            <Typography variant="h5">Customers</Typography>

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

          {/* Table Container with fixed height*/}
          {!error && (
            <Paper
              sx={{
                width: "100%",
                overflow: "hidden",
                display: "flex",
                flexDirection: "column",
                height: "calc(100vh - 240px)",
              }}
            >
              {/* Scrollable Table */}
              <Box
                sx={{
                  flex: 1,
                  overflow: "auto",
                }}
              >
                <CustomerTable
                  customers={customers}
                  loading={loading}
                  sortBy={sortBy}
                  sortOrder={sortOrder}
                  onSort={(field: string) => {
                    if (sortBy === field) {
                      setSortOrder(sortOrder === "asc" ? "desc" : "asc");
                    } else {
                      setSortBy(field);
                      setSortOrder("asc");
                    }
                    setPage(0);
                  }}
                />
              </Box>

              {/* Fixed Pagination */}
              <TablePagination
                rowsPerPageOptions={[10, 25, 100]}
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
          )}
        </>
      )}

      <CustomerFilterDialog
        open={filterDialogOpen}
        onClose={() => setFilterDialogOpen(false)}
        tempFilters={tempFilters}
        onTempFiltersChange={setTempFilters}
        onClearFirstName={() =>
          setTempFilters((f) => ({ ...f, firstName: "" }))
        }
        onClearLastName={() => setTempFilters((f) => ({ ...f, lastName: "" }))}
        onClearEmail={() => setTempFilters((f) => ({ ...f, email: "" }))}
        onClearPhoneNum={() => setTempFilters((f) => ({ ...f, phoneNum: "" }))}
        onApply={handleApplyFilters}
        sortBy={sortBy}
        sortOrder={sortOrder}
        sortableColumns={[
          { id: "id", label: "Customer ID" },
          { id: "firstName", label: "First Name" },
          { id: "lastName", label: "Last Name" },
          { id: "email", label: "Email" },
        ]}
      />

      <AddCustomerModal
        open={addModalOpen}
        onClose={handleCloseAddModal}
        onSubmit={handleAddCustomer}
        error={addCustomerError}
      />
    </>
  );
};

export default CustomersPage;
