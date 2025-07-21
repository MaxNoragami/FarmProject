import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TableSortLabel,
  Skeleton,
  Box,
} from "@mui/material";
import * as React from "react";
import { type CustomerData } from "../../utils/customerMappers";

interface CustomerTableProps {
  customers: CustomerData[];
  loading: boolean;
  sortBy: string;
  sortOrder: "asc" | "desc";
  onSort: (field: string) => void;
}

const CustomerTable: React.FC<CustomerTableProps> = ({
  customers,
  loading,
  sortBy,
  sortOrder,
  onSort,
}) => {
  const createSortHandler = (property: string) => () => {
    onSort(property);
  };

  if (loading) {
    return (
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Customer ID</TableCell>
              <TableCell>First Name</TableCell>
              <TableCell>Last Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Phone</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Array.from({ length: 10 }).map((_, index) => (
              <TableRow key={index}>
                <TableCell>
                  <Skeleton width={60} />
                </TableCell>
                <TableCell>
                  <Skeleton width={100} />
                </TableCell>
                <TableCell>
                  <Skeleton width={100} />
                </TableCell>
                <TableCell>
                  <Skeleton width={150} />
                </TableCell>
                <TableCell>
                  <Skeleton width={120} />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  if (customers.length === 0) {
    return (
      <Box sx={{ textAlign: "center", py: 4 }}>
        <Paper sx={{ p: 3 }}>No customers found.</Paper>
      </Box>
    );
  }

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>
              <TableSortLabel
                active={sortBy === "id"}
                direction={sortBy === "id" ? sortOrder : "asc"}
                onClick={createSortHandler("id")}
              >
                Customer ID
              </TableSortLabel>
            </TableCell>
            <TableCell>
              <TableSortLabel
                active={sortBy === "firstName"}
                direction={sortBy === "firstName" ? sortOrder : "asc"}
                onClick={createSortHandler("firstName")}
              >
                First Name
              </TableSortLabel>
            </TableCell>
            <TableCell>
              <TableSortLabel
                active={sortBy === "lastName"}
                direction={sortBy === "lastName" ? sortOrder : "asc"}
                onClick={createSortHandler("lastName")}
              >
                Last Name
              </TableSortLabel>
            </TableCell>
            <TableCell>
              <TableSortLabel
                active={sortBy === "email"}
                direction={sortBy === "email" ? sortOrder : "asc"}
                onClick={createSortHandler("email")}
              >
                Email
              </TableSortLabel>
            </TableCell>
            <TableCell>Phone</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {customers.map((customer) => (
            <TableRow key={customer.id}>
              <TableCell>{customer.id}</TableCell>
              <TableCell>{customer.firstName}</TableCell>
              <TableCell>{customer.lastName}</TableCell>
              <TableCell>{customer.email}</TableCell>
              <TableCell>{customer.phoneNum}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default CustomerTable;
