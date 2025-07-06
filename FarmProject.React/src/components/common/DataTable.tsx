import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TableSortLabel,
  Box,
} from "@mui/material";
import { visuallyHidden } from "@mui/utils";
import * as React from "react";
import type { Column } from "../../constants/rabbitColumns";

interface DataTableProps<T> {
  columns: readonly Column[];
  data: T[];
  page: number;
  rowsPerPage: number;
  totalCount: number;
  order: "asc" | "desc";
  orderBy: string;
  onPageChange: (event: unknown, newPage: number) => void;
  onRowsPerPageChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onRequestSort: (event: React.MouseEvent<unknown>, property: string) => void;
  renderRow: (row: T, columns: readonly Column[]) => React.ReactNode;
  getRowKey: (row: T) => string | number;
  hasActiveFilters?: boolean;
}

function DataTable<T>({
  columns,
  data,
  page,
  rowsPerPage,
  totalCount,
  order,
  orderBy,
  onPageChange,
  onRowsPerPageChange,
  onRequestSort,
  renderRow,
  getRowKey,
  hasActiveFilters = false,
}: DataTableProps<T>) {
  const createSortHandler =
    (property: string) => (event: React.MouseEvent<unknown>) => {
      onRequestSort(event, property);
    };

  const containerHeight = hasActiveFilters
    ? "calc(100vh - 280px)"
    : "calc(100vh - 240px)";

  return (
    <Paper
      sx={{
        width: "100%",
        overflow: "hidden",
        display: "flex",
        flexDirection: "column",
        height: containerHeight,
      }}
    >
      <TableContainer
        sx={{
          flex: 1,
          overflow: "auto",
        }}
      >
        <Table stickyHeader aria-label="data table">
          <TableHead>
            <TableRow>
              {columns.map((column) => (
                <TableCell
                  key={column.id}
                  align={column.align}
                  style={{ minWidth: column.minWidth }}
                  sortDirection={orderBy === column.id ? order : false}
                >
                  {column.sortable ? (
                    <TableSortLabel
                      active={orderBy === column.id}
                      direction={orderBy === column.id ? order : "asc"}
                      onClick={createSortHandler(column.id)}
                    >
                      {column.label}
                      {orderBy === column.id ? (
                        <Box component="span" sx={visuallyHidden}>
                          {order === "desc"
                            ? "sorted descending"
                            : "sorted ascending"}
                        </Box>
                      ) : null}
                    </TableSortLabel>
                  ) : (
                    column.label
                  )}
                </TableCell>
              ))}
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((row) => (
              <TableRow
                hover
                role="checkbox"
                tabIndex={-1}
                key={getRowKey(row)}
              >
                {renderRow(row, columns)}
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[10, 25, 100]}
        component="div"
        count={totalCount}
        rowsPerPage={rowsPerPage}
        page={page}
        onPageChange={onPageChange}
        onRowsPerPageChange={onRowsPerPageChange}
        labelRowsPerPage="Rows:"
        sx={{
          borderTop: 1,
          borderColor: "divider",
          flexShrink: 0,
        }}
      />
    </Paper>
  );
}

export default DataTable;
