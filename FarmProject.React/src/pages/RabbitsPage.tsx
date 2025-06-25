import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Paper, TablePagination } from '@mui/material';
import { Add, FilterList } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { breedingStatusOptions } from '../types/BreedingStatus';
import { type RabbitData, mockRabbitsData } from '../data/mockData';
import { rabbitColumns, getSortableColumns } from '../constants/rabbitColumns';
import { useTableFilters, type Filter } from '../hooks/useTableFilters';
import { useTableSorting } from '../hooks/useTableSorting';
import FilterDialog from '../components/common/FilterDialog';
import DataTable from '../components/common/DataTable';
import RabbitCard from '../components/rabbits/RabbitCard';
import RabbitTableRow from '../components/rabbits/RabbitTableRow';

const RabbitsPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    // Filter state
    const {
        filters,
        setFilters,
        logicalOperator,
        setLogicalOperator,
        filteredData,
        removeFilter
    } = useTableFilters(mockRabbitsData);

    // Sort state
    const {
        order,
        orderBy,
        sortedData,
        handleRequestSort,
        setOrder,
        setOrderBy
    } = useTableSorting(filteredData, 'rabbitId');

    // Filter dialog state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [tempFilters, setTempFilters] = React.useState<{
        name: string;
        status: string;
    }>({ name: '', status: '' });

    // Handlers
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

    const clearNameFilter = () => {
        const newTempFilters = { ...tempFilters, name: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearStatusFilter = () => {
        const newTempFilters = { ...tempFilters, status: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const updateFiltersFromTemp = (filters: { name: string; status: string }) => {
        const newFilters: Filter[] = [];
        
        if (filters.name.trim()) {
            newFilters.push({
                id: `name-${Date.now()}`,
                column: 'name',
                operator: 'contains',
                value: filters.name.trim()
            });
        }
        
        if (filters.status.trim()) {
            newFilters.push({
                id: `status-${Date.now()}`,
                column: 'status',
                operator: 'equals',
                value: filters.status.trim()
            });
        }
        
        setFilters(newFilters);
        setPage(0);
    };

    const handleRemoveFilter = (filterId: string) => {
        const filterToRemove = filters.find(f => f.id === filterId);
        removeFilter(filterId);
        setPage(0);
        
        if (filterToRemove) {
            if (filterToRemove.column === 'name') {
                setTempFilters({ ...tempFilters, name: '' });
            } else if (filterToRemove.column === 'status') {
                setTempFilters({ ...tempFilters, status: '' });
            }
        }
    };

    const applyFiltersFromDialog = () => {
        updateFiltersFromTemp(tempFilters);
        setFilterDialogOpen(false);
    };

    // Data calculation
    const visibleRows = React.useMemo(() => {
        return sortedData.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    }, [sortedData, page, rowsPerPage]);

    const renderTableRow = (rabbit: RabbitData) => (
        <RabbitTableRow rabbit={rabbit} columns={rabbitColumns} />
    );

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
                                    Rabbits
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
                                            label={`${rabbitColumns.find(col => col.id === filter.column)?.label} ${filter.operator} "${filter.value}"`}
                                            onDelete={() => handleRemoveFilter(filter.id)}
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
                                count={filteredData.length}
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
                                    label={`${rabbitColumns.find(col => col.id === filter.column)?.label} ${filter.operator} "${filter.value}"`}
                                    onDelete={() => handleRemoveFilter(filter.id)}
                                    size="small"
                                />
                            ))}
                        </Box>
                    )}

                    <Divider sx={{ mb: 3 }} />
                    
                    <DataTable
                        columns={rabbitColumns}
                        data={visibleRows}
                        page={page}
                        rowsPerPage={rowsPerPage}
                        totalCount={filteredData.length}
                        order={order}
                        orderBy={String(orderBy)}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        onRequestSort={(_, property) => handleRequestSort(property as keyof RabbitData)}
                        renderRow={renderTableRow}
                        getRowKey={(rabbit) => rabbit.rabbitId}
                        hasActiveFilters={filters.length > 0}
                    />
                </>
            )}

            <FilterDialog
                open={filterDialogOpen}
                onClose={() => setFilterDialogOpen(false)}
                tempFilters={tempFilters}
                onTempFiltersChange={setTempFilters}
                statusOptions={breedingStatusOptions}
                onClearName={clearNameFilter}
                onClearStatus={clearStatusFilter}
                logicalOperator={logicalOperator}
                onLogicalOperatorChange={setLogicalOperator}
                onApply={applyFiltersFromDialog}
                isMobile={isMobile}
                sortBy={String(orderBy)}
                onSortByChange={(value) => setOrderBy(value as keyof RabbitData)}
                sortOrder={order}
                onSortOrderChange={setOrder}
                sortableColumns={getSortableColumns()}
            />
        </>
    );
};

export default RabbitsPage;
