import {
  Dialog,
  DialogTitle,
  DialogContent,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Box,
  Button,
  IconButton,
  Checkbox,
  FormControlLabel,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import { Close, Clear } from "@mui/icons-material";
import * as React from "react";
import { offspringTypeOptions } from "../../types/OffspringType";

interface CageFilterDialogProps {
  open: boolean;
  onClose: () => void;
  tempFilters: {
    name: string;
    offspringType: string;
    isOccupied: boolean | null;
    isSacrificable: boolean | null;
  };
  onTempFiltersChange: (filters: {
    name: string;
    offspringType: string;
    isOccupied: boolean | null;
    isSacrificable: boolean | null;
  }) => void;
  onClearName: () => void;
  onClearOffspringType: () => void;
  onClearOccupied: () => void;
  onClearSacrificable: () => void;
  logicalOperator: "AND" | "OR";
  onLogicalOperatorChange: (operator: "AND" | "OR") => void;
  onApply: (params: {
    filters: {
      name: string;
      offspringType: string;
      isOccupied: boolean | null;
      isSacrificable: boolean | null;
    };
    sortBy: string;
    sortOrder: "asc" | "desc";
  }) => void;
  sortBy?: string;
  onSortByChange?: (sortBy: string) => void;
  sortOrder?: "asc" | "desc";
  onSortOrderChange?: (order: "asc" | "desc") => void;
  sortableColumns?: Array<{ id: string; label: string }>;
}

const CageFilterDialog: React.FC<CageFilterDialogProps> = ({
  open,
  onClose,
  tempFilters,
  logicalOperator,
  onApply,
  sortBy,
  sortOrder,
  sortableColumns = [],
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const [name, setName] = React.useState("");
  const [offspringType, setOffspringType] = React.useState("");
  const [isOccupied, setIsOccupied] = React.useState<boolean | null>(null);
  const [isSacrificable, setIsSacrificable] = React.useState<boolean | null>(
    null
  );
  const [localSortBy, setLocalSortBy] = React.useState("");
  const [localSortOrder, setLocalSortOrder] = React.useState<"asc" | "desc">(
    "asc"
  );
  const [localLogicalOperator, setLocalLogicalOperator] = React.useState<
    "AND" | "OR"
  >("AND");

  React.useEffect(() => {
    if (open) {
      setName(tempFilters.name);
      setOffspringType(tempFilters.offspringType);
      setIsOccupied(tempFilters.isOccupied);
      setIsSacrificable(tempFilters.isSacrificable);
      setLocalSortBy(sortBy || "");
      setLocalSortOrder(sortOrder || "asc");
      setLocalLogicalOperator(logicalOperator);
    }
  }, [open, tempFilters, sortBy, sortOrder, logicalOperator]);

  const handleApply = () => {
    onApply({
      filters: { name, offspringType, isOccupied, isSacrificable },
      sortBy: localSortBy,
      sortOrder: localSortOrder,
    });
  };

  const handleIsOccupiedChange = () => {
    if (isOccupied === null) {
      setIsOccupied(true);
    } else if (isOccupied === true) {
      setIsOccupied(false);
    } else {
      setIsOccupied(null);
    }
  };

  const handleIsSacrificableChange = () => {
    if (isSacrificable === null) {
      setIsSacrificable(true);
    } else if (isSacrificable === true) {
      setIsSacrificable(false);
    } else {
      setIsSacrificable(null);
    }
  };

  const hasChanges = React.useMemo(() => {
    const filtersChanged =
      name !== tempFilters.name ||
      offspringType !== tempFilters.offspringType ||
      isOccupied !== tempFilters.isOccupied ||
      isSacrificable !== tempFilters.isSacrificable;
    const sortChanged =
      localSortBy !== (sortBy || "") || localSortOrder !== (sortOrder || "asc");
    const operatorChanged = localLogicalOperator !== logicalOperator;

    return filtersChanged || sortChanged || operatorChanged;
  }, [
    name,
    offspringType,
    isOccupied,
    isSacrificable,
    localSortBy,
    localSortOrder,
    localLogicalOperator,
    tempFilters,
    sortBy,
    sortOrder,
    logicalOperator,
  ]);

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
        <Box sx={{ display: "flex", flexDirection: "column", gap: 3, mt: 2 }}>
          <TextField
            label="Name"
            variant="outlined"
            fullWidth
            value={name}
            onChange={(e) => setName(e.target.value)}
            slotProps={{
              input: {
                endAdornment: name && (
                  <IconButton onClick={() => setName("")} size="small">
                    <Clear />
                  </IconButton>
                ),
              },
            }}
          />

          <FormControl fullWidth>
            <InputLabel>Offspring Type</InputLabel>
            <Select
              value={offspringType}
              onChange={(e) => setOffspringType(e.target.value)}
              label="Offspring Type"
              endAdornment={
                offspringType && (
                  <IconButton
                    onClick={() => setOffspringType("")}
                    size="small"
                    sx={{ mr: 2 }}
                  >
                    <Clear />
                  </IconButton>
                )
              }
            >
              {offspringTypeOptions.map((type) => (
                <MenuItem key={type} value={type}>
                  {type}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {sortableColumns.length > 0 && (
            <>
              <FormControl fullWidth>
                <InputLabel>Sort By</InputLabel>
                <Select
                  value={localSortBy}
                  onChange={(e) => setLocalSortBy(e.target.value)}
                  label="Sort By"
                >
                  {sortableColumns.map((column) => (
                    <MenuItem key={column.id} value={column.id}>
                      {column.label}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl fullWidth>
                <InputLabel>Sort Order</InputLabel>
                <Select
                  value={localSortOrder}
                  onChange={(e) =>
                    setLocalSortOrder(e.target.value as "asc" | "desc")
                  }
                  label="Sort Order"
                >
                  <MenuItem value="asc">Ascending</MenuItem>
                  <MenuItem value="desc">Descending</MenuItem>
                </Select>
              </FormControl>
            </>
          )}

          {/* Checkbox container - responsive layout */}
          <Box
            sx={{
              display: "flex",
              flexDirection: isMobile ? "column" : "row",
              gap: isMobile ? 0 : 4,
            }}
          >
            {/* IsOccupied checkbox */}
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
              }}
            >
              <FormControlLabel
                control={
                  <Checkbox
                    checked={isOccupied === true}
                    indeterminate={isOccupied === null}
                    onChange={handleIsOccupiedChange}
                    size={isMobile ? "small" : "medium"}
                  />
                }
                label="Is Occupied?"
                sx={{
                  "& .MuiFormControlLabel-label": {
                    fontSize: "inherit",
                  },
                }}
              />
              {isOccupied !== null && (
                <IconButton onClick={() => setIsOccupied(null)} size="small">
                  <Clear />
                </IconButton>
              )}
            </Box>

            {/* IsSacrificable checkbox */}
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
              }}
            >
              <FormControlLabel
                control={
                  <Checkbox
                    checked={isSacrificable === true}
                    indeterminate={isSacrificable === null}
                    onChange={handleIsSacrificableChange}
                    size={isMobile ? "small" : "medium"}
                  />
                }
                label="Is Sacrificable?"
                sx={{
                  "& .MuiFormControlLabel-label": {
                    fontSize: "inherit",
                  },
                }}
              />
              {isSacrificable !== null && (
                <IconButton
                  onClick={() => setIsSacrificable(null)}
                  size="small"
                >
                  <Clear />
                </IconButton>
              )}
            </Box>
          </Box>

          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              gap: 2,
            }}
          >
            <FormControl size="small" sx={{ minWidth: 120 }}>
              <InputLabel>Logical Operator</InputLabel>
              <Select
                value={localLogicalOperator}
                onChange={(e) =>
                  setLocalLogicalOperator(e.target.value as "AND" | "OR")
                }
                label="Logical Operator"
              >
                <MenuItem value="AND">AND</MenuItem>
                <MenuItem value="OR">OR</MenuItem>
              </Select>
            </FormControl>

            <Button
              onClick={handleApply}
              variant="contained"
              disabled={!hasChanges}
            >
              Apply
            </Button>
          </Box>
        </Box>
      </DialogContent>
    </Dialog>
  );
};

export default CageFilterDialog;
