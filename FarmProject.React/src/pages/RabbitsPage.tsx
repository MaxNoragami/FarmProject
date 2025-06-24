import { Typography, Box, Button, Divider, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, TableSortLabel, Dialog, DialogTitle, DialogContent, DialogActions, TextField, Select, MenuItem, FormControl, InputLabel, Chip, IconButton, Card, CardContent, useMediaQuery, useTheme } from '@mui/material';
import { Add, FilterList, Delete, Close, Clear } from '@mui/icons-material';
import { visuallyHidden } from '@mui/utils';
import * as React from 'react';
import { BreedingStatus, breedingStatusOptions } from '../types/BreedingStatus';
import { type RabbitData, mockRabbitsData } from '../data/mockData';
import { Helmet } from 'react-helmet-async';

interface Column {
  id: 'number' | 'rabbitId' | 'name' | 'cageId' | 'status';
  label: string;
  minWidth?: number;
  align?: 'right' | 'center';
  sortable?: boolean;
  numeric?: boolean;
  filterable?: boolean;
}

const columns: readonly Column[] = [
  { id: 'number', label: '#', minWidth: 50, align: 'center', sortable: false, filterable: false },
  { id: 'rabbitId', label: 'RABBIT ID', minWidth: 100, align: 'center', sortable: true, numeric: true, filterable: false },
  { id: 'name', label: 'NAME', minWidth: 150, sortable: true, numeric: false, filterable: true },
  { id: 'cageId', label: 'CAGE ID', minWidth: 100, align: 'center', sortable: false, numeric: true, filterable: false },
  { id: 'status', label: 'STATUS', minWidth: 120, align: 'center', sortable: true, numeric: false, filterable: true },
];

function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

type Order = 'asc' | 'desc';

function getComparator<Key extends keyof any>(
  order: Order,
  orderBy: Key,
): (
  a: { [key in Key]: number | string },
  b: { [key in Key]: number | string },
) => number {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

interface Filter {
  id: string;
  column: keyof RabbitData;
  operator: 'contains' | 'equals' | 'startsWith';
  value: string;
}

const RabbitsPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md')); // Changed back to 'md'

    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);
    const [order, setOrder] = React.useState<Order>('asc');
    const [orderBy, setOrderBy] = React.useState<keyof RabbitData>('rabbitId');
    const [filters, setFilters] = React.useState<Filter[]>([]);
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [logicalOperator, setLogicalOperator] = React.useState<'AND' | 'OR'>('AND');
    const [tempFilters, setTempFilters] = React.useState<{
        name: string;
        status: string;
    }>({ name: '', status: '' });

    const handleAddRabbit = () => {
        console.log('Add rabbit clicked');
    };

    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    const handleRequestSort = (
        event: React.MouseEvent<unknown>,
        property: keyof RabbitData,
    ) => {
        const isAsc = orderBy === property && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        setOrderBy(property);
    };

    const createSortHandler =
        (property: keyof RabbitData) => (event: React.MouseEvent<unknown>) => {
            handleRequestSort(event, property);
        };

    const applyFilters = (data: RabbitData[]): RabbitData[] => {
        if (filters.length === 0) return data;
        
        return data.filter(row => {
            const results = filters.map(filter => {
                const value = String(row[filter.column]).toLowerCase();
                const filterValue = filter.value.toLowerCase();
                
                switch (filter.operator) {
                    case 'contains':
                        return value.includes(filterValue);
                    case 'equals':
                        return value === filterValue;
                    case 'startsWith':
                        return value.startsWith(filterValue);
                    default:
                        return true;
                }
            });
            
            return logicalOperator === 'AND' 
                ? results.every(result => result)
                : results.some(result => result);
        });
    };

    const clearNameFilter = () => {
        const newTempFilters = { ...tempFilters, name: '' };
        setTempFilters(newTempFilters);
        
        // Immediately apply filters
        const newFilters: Filter[] = [];
        
        if (newTempFilters.status.trim()) {
            newFilters.push({
                id: `status-${Date.now()}`,
                column: 'status',
                operator: 'equals',
                value: newTempFilters.status.trim()
            });
        }
        
        setFilters(newFilters);
    };

    const clearStatusFilter = () => {
        const newTempFilters = { ...tempFilters, status: '' };
        setTempFilters(newTempFilters);
        
        // Immediately apply filters
        const newFilters: Filter[] = [];
        
        if (newTempFilters.name.trim()) {
            newFilters.push({
                id: `name-${Date.now()}`,
                column: 'name',
                operator: 'contains',
                value: newTempFilters.name.trim()
            });
        }
        
        setFilters(newFilters);
    };

    const removeFilter = (filterId: string) => {
        const filterToRemove = filters.find(f => f.id === filterId);
        setFilters(filters.filter(f => f.id !== filterId));
        
        if (filterToRemove) {
            if (filterToRemove.column === 'name') {
                setTempFilters({ ...tempFilters, name: '' });
            } else if (filterToRemove.column === 'status') {
                setTempFilters({ ...tempFilters, status: '' });
            }
        }
    };

    const applyFiltersFromDialog = () => {
        const newFilters: Filter[] = [];
        
        if (tempFilters.name.trim()) {
            newFilters.push({
                id: `name-${Date.now()}`,
                column: 'name',
                operator: 'contains',
                value: tempFilters.name.trim()
            });
        }
        
        if (tempFilters.status.trim()) {
            newFilters.push({
                id: `status-${Date.now()}`,
                column: 'status',
                operator: 'equals',
                value: tempFilters.status.trim()
            });
        }
        
        setFilters(newFilters);
        setFilterDialogOpen(false);
    };

    const clearFilters = () => {
        setTempFilters({ name: '', status: '' });
        setFilters([]);
    };

    const visibleRows = React.useMemo(
        () => {
            const filteredData = applyFilters(mockRabbitsData);
            return [...filteredData]
                .sort(getComparator(order, orderBy))
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
        },
        [order, orderBy, page, rowsPerPage, filters, logicalOperator],
    );

    const filteredRowsCount = React.useMemo(() => applyFilters(mockRabbitsData).length, [filters, logicalOperator]);

    const RabbitCard = ({ rabbit }: { rabbit: RabbitData }) => (
        <Card sx={{ mb: 2 }}>
            <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Typography variant="h6" component="div">
                        {rabbit.name}
                    </Typography>
                    <Chip 
                        label={rabbit.status} 
                        color={rabbit.status === 'Available' ? 'success' : 'warning'} 
                        size="small" 
                    />
                </Box>
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 1 }}>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            RABBIT ID
                        </Typography>
                        <Typography variant="body1">
                            {rabbit.rabbitId}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" color="text.secondary">
                            CAGE ID
                        </Typography>
                        <Typography variant="body1">
                            {rabbit.cageId}
                        </Typography>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    );

    const getSortableColumns = () => {
        return columns.filter(col => col.sortable);
    };

    return (
        <>
            <Helmet>
                <title>Breeding Rabbits - Farm Project</title>
            </Helmet>
            
            {isMobile ? (
                // Mobile Layout
                <>
                    {/* Sticky Header */}
                    <Box 
                        sx={{ 
                            position: 'sticky',
                            top: 0,
                            backgroundColor: '#f5f5f5',
                            zIndex: 1,
                            px: 2,
                            pt: 2,
                            pb: 2
                        }}
                    >
                        <Box sx={{ 
                            backgroundColor: 'white',
                            borderRadius: 1,
                            p: 2,
                            boxShadow: 1
                        }}>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                                <Typography variant="h5">
                                    Breeding Rabbits
                                </Typography>

                                <Box sx={{ display: 'flex', gap: 1 }}>
                                    <Button
                                        variant="outlined"
                                        startIcon={<FilterList />}
                                        onClick={() => setFilterDialogOpen(true)}
                                        size="small"
                                    >
                                        Filter
                                    </Button>
                                    <Button
                                        variant="contained"
                                        startIcon={<Add />}
                                        onClick={handleAddRabbit}
                                        size="small"
                                    >
                                        Add
                                    </Button>
                                </Box>
                            </Box>
                            
                            {/* Active Filters Display */}
                            {filters.length > 0 && (
                                <Box sx={{ mb: 2, display: 'flex', flexWrap: 'wrap', gap: 1, alignItems: 'center' }}>
                                    <Typography variant="body2" sx={{ mr: 1 }}>
                                        Filters ({logicalOperator}):
                                    </Typography>
                                    {filters.map((filter) => (
                                        <Chip
                                            key={filter.id}
                                            label={`${columns.find(col => col.id === filter.column)?.label} ${filter.operator} "${filter.value}"`}
                                            onDelete={() => removeFilter(filter.id)}
                                            size="small"
                                        />
                                    ))}
                                </Box>
                            )}

                            <Divider />
                        </Box>
                    </Box>

                    {/* Content Area */}
                    <Box sx={{ 
                        flex: 1,
                        overflow: 'auto',
                        px: 2
                    }}>
                        <Box sx={{ py: 2 }}>
                            {visibleRows.map((rabbit) => (
                                <RabbitCard key={rabbit.rabbitId} rabbit={rabbit} />
                            ))}
                        </Box>
                    </Box>

                    {/* Pagination */}
                    <Box sx={{ 
                        flexShrink: 0,
                        px: 2, 
                        pb: 1,
                        backgroundColor: '#f5f5f5'
                    }}>
                        <Paper sx={{ borderRadius: 1 }}>
                            <TablePagination
                                rowsPerPageOptions={[10, 25, 100]}
                                component="div"
                                count={filteredRowsCount}
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
                // Desktop Layout
                <>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h5">
                            Breeding Rabbits
                        </Typography>

                        <Box sx={{ display: 'flex', gap: 1 }}>
                            <Button
                                variant="outlined"
                                startIcon={<FilterList />}
                                onClick={() => setFilterDialogOpen(true)}
                            >
                                Filter
                            </Button>
                            <Button
                                variant="contained"
                                startIcon={<Add />}
                                onClick={handleAddRabbit}
                            >
                                Add
                            </Button>
                        </Box>
                    </Box>
                    
                    {/* Active Filters Display */}
                    {filters.length > 0 && (
                        <Box sx={{ mb: 2, display: 'flex', flexWrap: 'wrap', gap: 1, alignItems: 'center' }}>
                            <Typography variant="body2" sx={{ mr: 1 }}>
                                Filters ({logicalOperator}):
                            </Typography>
                            {filters.map((filter) => (
                                <Chip
                                    key={filter.id}
                                    label={`${columns.find(col => col.id === filter.column)?.label} ${filter.operator} "${filter.value}"`}
                                    onDelete={() => removeFilter(filter.id)}
                                    size="small"
                                />
                            ))}
                        </Box>
                    )}

                    <Divider sx={{ mb: 3 }} />
                    
                    <Paper sx={{ width: '100%', overflow: 'hidden', height: 'fit-content', maxHeight: '100%' }}>
                        <TableContainer sx={{ maxHeight: 'calc(100vh - 300px)' }}>
                            <Table stickyHeader aria-label="rabbits table">
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
                                                        direction={orderBy === column.id ? order : 'asc'}
                                                        onClick={createSortHandler(column.id)}
                                                    >
                                                        {column.label}
                                                        {orderBy === column.id ? (
                                                            <Box component="span" sx={visuallyHidden}>
                                                                {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
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
                                    {visibleRows.map((row) => {
                                        return (
                                            <TableRow hover role="checkbox" tabIndex={-1} key={row.rabbitId}>
                                                {columns.map((column) => {
                                                    const value = row[column.id];
                                                    return (
                                                        <TableCell key={column.id} align={column.align}>
                                                            {value}
                                                        </TableCell>
                                                    );
                                                })}
                                            </TableRow>
                                        );
                                    })}
                                </TableBody>
                            </Table>
                        </TableContainer>
                        <TablePagination
                            rowsPerPageOptions={[10, 25, 100]}
                            component="div"
                            count={filteredRowsCount}
                            rowsPerPage={rowsPerPage}
                            page={page}
                            onPageChange={handleChangePage}
                            onRowsPerPageChange={handleChangeRowsPerPage}
                            labelRowsPerPage="Rows:"
                        />
                    </Paper>
                </>
            )}

            {/* Filter Dialog */}
            <Dialog open={filterDialogOpen} onClose={() => setFilterDialogOpen(false)} maxWidth="sm" fullWidth>
                <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    Filter Options
                    <IconButton onClick={() => setFilterDialogOpen(false)} size="small">
                        <Close />
                    </IconButton>
                </DialogTitle>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3, mt: 2 }}>
                        <TextField
                            fullWidth
                            label="Name"
                            value={tempFilters.name}
                            onChange={(e) => setTempFilters({ ...tempFilters, name: e.target.value })}
                            placeholder="Filter by rabbit name..."
                            InputProps={{
                                endAdornment: tempFilters.name && (
                                    <IconButton onClick={clearNameFilter} size="small">
                                        <Clear />
                                    </IconButton>
                                )
                            }}
                        />
                        
                        <FormControl fullWidth>
                            <InputLabel>Status</InputLabel>
                            <Select
                                value={tempFilters.status}
                                onChange={(e) => setTempFilters({ ...tempFilters, status: e.target.value })}
                                label="Status"
                                endAdornment={tempFilters.status && (
                                    <IconButton onClick={clearStatusFilter} size="small" sx={{ mr: 2 }}>
                                        <Clear />
                                    </IconButton>
                                )}
                            >
                                <MenuItem value="">All</MenuItem>
                                {breedingStatusOptions.map((status) => (
                                    <MenuItem key={status} value={status}>
                                        {status}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>

                        {isMobile && (
                            <>
                                <FormControl fullWidth>
                                    <InputLabel>Sort By</InputLabel>
                                    <Select
                                        value={orderBy}
                                        onChange={(e) => setOrderBy(e.target.value as keyof RabbitData)}
                                        label="Sort By"
                                    >
                                        {getSortableColumns().map((column) => (
                                            <MenuItem key={column.id} value={column.id}>
                                                {column.label}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>

                                <FormControl fullWidth>
                                    <InputLabel>Sort Order</InputLabel>
                                    <Select
                                        value={order}
                                        onChange={(e) => setOrder(e.target.value as Order)}
                                        label="Sort Order"
                                    >
                                        <MenuItem value="asc">Ascending</MenuItem>
                                        <MenuItem value="desc">Descending</MenuItem>
                                    </Select>
                                </FormControl>
                            </>
                        )}
                        
                        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 2 }}>
                            <FormControl size="small" sx={{ minWidth: 120 }}>
                                <InputLabel>Logical Operator</InputLabel>
                                <Select
                                    value={logicalOperator}
                                    onChange={(e) => setLogicalOperator(e.target.value as 'AND' | 'OR')}
                                    label="Logical Operator"
                                >
                                    <MenuItem value="AND">AND</MenuItem>
                                    <MenuItem value="OR">OR</MenuItem>
                                </Select>
                            </FormControl>
                            
                            <Button 
                                onClick={applyFiltersFromDialog} 
                                variant="contained"
                                disabled={!tempFilters.name.trim() && !tempFilters.status.trim()}
                            >
                                Apply
                            </Button>
                        </Box>
                    </Box>
                </DialogContent>
            </Dialog>
        </>
    );
};

export default RabbitsPage;
