import {
  Typography,
  Box,
  Button,
  Divider,
  useMediaQuery,
  useTheme,
  Grid,
  Paper,
  TablePagination,
  Chip,
} from "@mui/material";
import { Add, FilterList } from "@mui/icons-material";
import * as React from "react";
import { Helmet } from "react-helmet-async";
import CageCard from "../components/cages/CageCard";
import CageCardSkeleton from "../components/common/CageCardSkeleton";
import AddCageModal from "../components/modals/AddCageModal";
import SacrificeModal from "../components/modals/SacrificeModal";
import type { AddCageFormFields } from "../schemas/cageSchemas";
import ErrorAlert from "../components/common/ErrorAlert";
import { CageService } from "../services/CageService";
import CageFilterDialog from "../components/cages/CageFilterDialog";
import { useCageData } from "../hooks/useCageData";
import { getSortableCageColumns } from "../constants/cageColumns";
import { offspringTypeStringToEnum } from "../types/OffspringType";
import { type CageData } from "../utils/cageMappers";

const CagesPage = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(12);

  const [addModalOpen, setAddModalOpen] = React.useState(false);
  const [addCageError, setAddCageError] = React.useState<string | null>(null);

  const [sacrificeModalOpen, setSacrificeModalOpen] = React.useState(false);
  const [selectedCage, setSelectedCage] = React.useState<CageData | null>(null);
  const [sacrificeError, setSacrificeError] = React.useState<string | null>(
    null
  );

  const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
  const [filters, setFilters] = React.useState<{
    name?: string;
    offspringType?: number;
    isOccupied?: boolean;
    isSacrificable?: boolean;
  }>({});
  const [logicalOperator, setLogicalOperator] = React.useState<"AND" | "OR">(
    "AND"
  );

  const [tempFilters, setTempFilters] = React.useState<{
    name: string;
    offspringType: string;
    isOccupied: boolean | null;
    isSacrificable: boolean | null;
  }>({ name: "", offspringType: "", isOccupied: null, isSacrificable: null });
  const [tempLogicalOperator, setTempLogicalOperator] = React.useState<
    "AND" | "OR"
  >("AND");

  const [sortBy, setSortBy] = React.useState<string>("cageId");
  const [sortOrder, setSortOrder] = React.useState<"asc" | "desc">("asc");

  const { cages, loading, error, totalCount, refetch } = useCageData({
    pageIndex: page,
    pageSize: rowsPerPage,
    filters,
    logicalOperator: logicalOperator === "AND" ? 0 : 1,
    sort: sortBy ? `${sortBy}:${sortOrder}` : undefined,
  });

  const sortableColumns = React.useMemo(
    () =>
      getSortableCageColumns().filter(
        (col) => col.id === "cageId" || col.id === "name"
      ),
    []
  );

  const handleAddCage = () => {
    setAddCageError(null);
    setAddModalOpen(true);
  };

  const handleModalClose = () => {
    setAddModalOpen(false);
    setAddCageError(null);
  };

  const handleSubmitNewCage = async (data: AddCageFormFields) => {
    setAddCageError(null);
    try {
      await CageService.addCage(data.name);
      setAddModalOpen(false);
      await refetch();
    } catch (err: any) {
      setAddCageError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred."
      );
    }
  };

  const handleCageClick = (cage: CageData) => {
    if (cage.isSacrificable && cage.offspringCount > 0) {
      setSelectedCage(cage);
      setSacrificeError(null);
      setSacrificeModalOpen(true);
    }
  };

  const handleSacrificeModalClose = () => {
    setSacrificeModalOpen(false);
    setSacrificeError(null);
  };

  const handleSacrifice = async (cageId: number, count: number) => {
    setSacrificeError(null);
    try {
      await CageService.sacrificeOffspring(cageId, count);
      setSacrificeModalOpen(false);
      await refetch();
    } catch (err: any) {
      setSacrificeError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred while sacrificing offspring."
      );
      throw err;
    }
  };

  const isCageClickable = (cage: CageData) => {
    return cage.isSacrificable && cage.offspringCount > 0;
  };

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
    let offspringTypeString = "";
    if (filters.offspringType !== undefined && filters.offspringType !== null) {
      const entry = Object.entries(offspringTypeStringToEnum).find(
        ([, num]) => num === filters.offspringType
      );
      offspringTypeString = entry ? entry[0] : "";
    }
    setTempFilters({
      name: filters.name || "",
      offspringType: offspringTypeString,
      isOccupied: filters.isOccupied ?? null,
      isSacrificable: filters.isSacrificable ?? null,
    });
    setTempLogicalOperator(logicalOperator);
    setSortBy(sortBy);
    setSortOrder(sortOrder);
    setFilterDialogOpen(true);
  };

  const handleApplyFilters = ({
    filters: modalFilters,
    sortBy: modalSortBy,
    sortOrder: modalSortOrder,
  }: {
    filters: {
      name: string;
      offspringType: string;
      isOccupied: boolean | null;
      isSacrificable: boolean | null;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => {
    const apiFilters: any = {};
    if (modalFilters.name.trim()) apiFilters.name = modalFilters.name.trim();
    if (
      modalFilters.offspringType !== "" &&
      Object.prototype.hasOwnProperty.call(
        offspringTypeStringToEnum,
        modalFilters.offspringType
      )
    ) {
      apiFilters.offspringType =
        offspringTypeStringToEnum[modalFilters.offspringType];
    }
    if (modalFilters.isOccupied !== null)
      apiFilters.isOccupied = modalFilters.isOccupied;
    if (modalFilters.isSacrificable !== null)
      apiFilters.isSacrificable = modalFilters.isSacrificable;

    setFilters(apiFilters);
    setSortBy(modalSortBy || "name");
    setSortOrder(modalSortOrder || "asc");
    setLogicalOperator(tempLogicalOperator);
    setPage(0);
    setFilterDialogOpen(false);
  };

  const handleClearNameFilter = () => {
    setFilters((prev) => ({ ...prev, name: undefined }));
    setPage(0);
  };

  const handleClearOffspringTypeFilter = () => {
    setFilters((prev) => ({ ...prev, offspringType: undefined }));
    setPage(0);
  };

  const handleClearOccupiedFilter = () => {
    setFilters((prev) => ({ ...prev, isOccupied: undefined }));
    setPage(0);
  };

  const handleClearSacrificableFilter = () => {
    setFilters((prev) => ({ ...prev, isSacrificable: undefined }));
    setPage(0);
  };

  const handleClearAllFilters = () => {
    setFilters({});
    setSortBy("name");
    setSortOrder("asc");
    setLogicalOperator("AND");
    setPage(0);
  };

  const getOffspringTypeString = (enumValue: number) => {
    const entry = Object.entries(offspringTypeStringToEnum).find(
      ([, num]) => num === enumValue
    );
    return entry ? entry[0] : "";
  };

  const FilterChips = () => {
    const hasFilters =
      filters.name ||
      filters.offspringType !== undefined ||
      filters.isOccupied !== undefined ||
      filters.isSacrificable !== undefined;

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
          Filters ({logicalOperator}):
        </Typography>

        {filters.name && (
          <Chip
            label={`NAME contains "${filters.name}"`}
            onDelete={handleClearNameFilter}
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

        {filters.offspringType !== undefined && (
          <Chip
            label={`OFFSPRING TYPE is "${getOffspringTypeString(
              filters.offspringType
            )}"`}
            onDelete={handleClearOffspringTypeFilter}
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

        {filters.isOccupied !== undefined && (
          <Chip
            label={`OCCUPIED is "${filters.isOccupied ? "Yes" : "No"}"`}
            onDelete={handleClearOccupiedFilter}
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

        {filters.isSacrificable !== undefined && (
          <Chip
            label={`SACRIFICABLE is "${filters.isSacrificable ? "Yes" : "No"}"`}
            onDelete={handleClearSacrificableFilter}
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

  const skeletonCards = Array.from(new Array(rowsPerPage)).map((_, index) => (
    <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={`skeleton-${index}`}>
      <CageCardSkeleton />
    </Grid>
  ));

  const mobileSkeleton = Array.from(new Array(rowsPerPage)).map((_, index) => (
    <Grid size={{ xs: 12, sm: 6 }} key={`mobile-skeleton-${index}`}>
      <CageCardSkeleton />
    </Grid>
  ));

  return (
    <>
      <Helmet>
        <title>Cages Management - Farm Project</title>
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
                <Typography variant="h5">Cages</Typography>

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
                    onClick={handleAddCage}
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
                  ? mobileSkeleton
                  : cages.map((cage) => (
                      <Grid size={{ xs: 12, sm: 6 }} key={cage.id}>
                        <CageCard
                          cage={cage}
                          onCageClick={handleCageClick}
                          isCageClickable={isCageClickable}
                        />
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
          {/* Header */}
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 2,
            }}
          >
            <Typography variant="h5">Cages</Typography>

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
                onClick={handleAddCage}
              >
                Add
              </Button>
            </Box>
          </Box>

          <FilterChips />
          <Divider sx={{ mb: 3 }} />

          {/* Cards Grid Container */}
          <Paper
            sx={{
              width: "100%",
              overflow: "hidden",
              display: "flex",
              flexDirection: "column",
              height: "calc(100vh - 240px)",
            }}
          >
            {/* Cards Grid */}
            <Box
              sx={{
                flex: 1,
                overflow: "auto",
                p: 2,
              }}
            >
              {error && <ErrorAlert message={error} onRetry={refetch} />}

              <Grid
                container
                rowSpacing={2}
                columnSpacing={{ xs: 1, sm: 2, md: 2 }}
              >
                {loading
                  ? skeletonCards
                  : cages.map((cage) => (
                      <Grid
                        size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                        key={cage.id}
                      >
                        <CageCard
                          cage={cage}
                          onCageClick={handleCageClick}
                          isCageClickable={isCageClickable}
                        />
                      </Grid>
                    ))}
              </Grid>
            </Box>

            {/* Desktop Pagination */}
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

      {/* Filter Dialog */}
      <CageFilterDialog
        open={filterDialogOpen}
        onClose={() => setFilterDialogOpen(false)}
        tempFilters={tempFilters}
        onTempFiltersChange={setTempFilters}
        onClearName={() => setTempFilters((f) => ({ ...f, name: "" }))}
        onClearOffspringType={() =>
          setTempFilters((f) => ({ ...f, offspringType: "" }))
        }
        onClearOccupied={() =>
          setTempFilters((f) => ({ ...f, isOccupied: null }))
        }
        onClearSacrificable={() =>
          setTempFilters((f) => ({ ...f, isSacrificable: null }))
        }
        logicalOperator={tempLogicalOperator}
        onLogicalOperatorChange={setTempLogicalOperator}
        onApply={handleApplyFilters}
        sortBy={sortBy}
        onSortByChange={setSortBy}
        sortOrder={sortOrder}
        onSortOrderChange={setSortOrder}
        sortableColumns={sortableColumns}
      />

      <AddCageModal
        open={addModalOpen}
        onClose={handleModalClose}
        onSubmit={handleSubmitNewCage}
        error={addCageError}
      />

      <SacrificeModal
        open={sacrificeModalOpen}
        onClose={handleSacrificeModalClose}
        onSubmit={handleSacrifice}
        cage={selectedCage}
        error={sacrificeError}
      />
    </>
  );
};

export default CagesPage;
