import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, TableSortLabel, Chip, Skeleton, Box } from '@mui/material';
import * as React from 'react';
import { type RabbitData } from '../../data/mockData';
import { getBreedingStatusColor } from '../../types/BreedingStatus';

interface RabbitTableProps {
  rabbits: RabbitData[];
  loading: boolean;
  sortBy: string;
  sortOrder: 'asc' | 'desc';
  onSort: (field: string) => void;
}

const RabbitTable: React.FC<RabbitTableProps> = ({
  rabbits,
  loading,
  sortBy,
  sortOrder,
  onSort
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
              <TableCell>Rabbit ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Cage ID</TableCell>
              <TableCell>Status</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Array.from({ length: 10 }).map((_, index) => (
              <TableRow key={index}>
                <TableCell><Skeleton width={60} /></TableCell>
                <TableCell><Skeleton width={120} /></TableCell>
                <TableCell><Skeleton width={60} /></TableCell>
                <TableCell><Skeleton width={100} /></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  if (rabbits.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Paper sx={{ p: 3 }}>
          No rabbits found.
        </Paper>
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
                active={sortBy === 'rabbitId'}
                direction={sortBy === 'rabbitId' ? sortOrder : 'asc'}
                onClick={createSortHandler('rabbitId')}
              >
                Rabbit ID
              </TableSortLabel>
            </TableCell>
            <TableCell>
              <TableSortLabel
                active={sortBy === 'name'}
                direction={sortBy === 'name' ? sortOrder : 'asc'}
                onClick={createSortHandler('name')}
              >
                Name
              </TableSortLabel>
            </TableCell>
            <TableCell>
              Cage ID
            </TableCell>
            <TableCell>
              Status
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rabbits.map((rabbit) => (
            <TableRow key={rabbit.rabbitId} hover>
              <TableCell>{rabbit.rabbitId}</TableCell>
              <TableCell>{rabbit.name}</TableCell>
              <TableCell>{rabbit.cageId}</TableCell>
              <TableCell>
                <Chip 
                  label={rabbit.status} 
                  color={getBreedingStatusColor(rabbit.status)} 
                  size="small" 
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default RabbitTable;
