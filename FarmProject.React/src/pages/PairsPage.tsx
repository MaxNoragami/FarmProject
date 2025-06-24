import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Grid, Paper, TablePagination } from '@mui/material';
import { Add, FilterList } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { mockPairsData, type PairData } from '../data/mockPairData';
import { usePairFilters, type PairFilter } from '../hooks/usePairFilters';
import { usePairSorting } from '../hooks/usePairSorting';
import { getSortablePairColumns } from '../constants/pairColumns';
import PairCard from '../components/pairs/PairCard';
import PairFilterDialog from '../components/pairs/PairFilterDialog';

const PairsPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Filter state
    const {
        filters,
        setFilters,
        logicalOperator,
        setLogicalOperator,
        filteredData,
        removeFilter
    } = usePairFilters(mockPairsData);

    // Sort state
    const {
        order,
        orderBy,
        sortedData,
        handleRequestSort,
        setOrder,
        setOrderBy
    } = usePairSorting(filteredData, 'pairId');

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Filter dialog state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [tempFilters, setTempFilters] = React.useState<{
        pairId: string;
        status: string;
    }>({ pairId: '', status: '' });

    // Handlers
    const handleAddPair = () => {
        console.log('Add pair clicked');
    };

    const clearPairIdFilter = () => {
        const newTempFilters = { ...tempFilters, pairId: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearStatusFilter = () => {
        const newTempFilters = { ...tempFilters, status: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const updateFiltersFromTemp = (filters: { pairId: string; status: string }) => {
        const newFilters: PairFilter[] = [];
        
        if (filters.pairId.trim()) {
            newFilters.push({
                id: `pairId-${Date.now()}`,
                column: 'pairId',
                operator: 'contains',
                value: filters.pairId.trim()
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
            if (filterToRemove.column === 'pairId') {
                setTempFilters({ ...tempFilters, pairId: '' });
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
    const visiblePairs = React.useMemo(() => {
        return sortedData.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    }, [sortedData, page, rowsPerPage]);

    const getFilterLabel = (filter: PairFilter) => {
        const columnLabels: Record<string, string> = {
            pairId: 'PAIR ID',
            status: 'STATUS'
        };
        return columnLabels[filter.column] || filter.column;
    };

    // Pagination handlers
    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    return (
        <>
            <Helmet>
                <title>Breeding Pairs - Farm Project</title>
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
                                    Breeding Pairs
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
                                        onClick={handleAddPair}
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
                                            label={`${getFilterLabel(filter)} ${filter.operator} "${filter.value}"`}
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
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2 }}>
                                {visiblePairs.map((pair) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6 }}
                                        key={pair.id}
                                    >
                                        <PairCard pair={pair} />
                                    </Grid>
                                ))}
                            </Grid>
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
                                rowsPerPageOptions={[12, 24, 48]}
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
                    {/* Header */}
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h5">
                            Breeding Pairs
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
                                onClick={handleAddPair}
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
                                    label={`${getFilterLabel(filter)} ${filter.operator} "${filter.value}"`}
                                    onDelete={() => handleRemoveFilter(filter.id)}
                                    size="small"
                                />
                            ))}
                        </Box>
                    )}

                    <Divider sx={{ mb: 3 }} />
                    
                    {/* Cards Grid Container */}
                    <Paper sx={{ 
                        width: '100%', 
                        overflow: 'hidden', 
                        display: 'flex', 
                        flexDirection: 'column',
                        height: filters.length > 0 ? 'calc(100vh - 280px)' : 'calc(100vh - 240px)'
                    }}>
                        {/* Cards Grid */}
                        <Box sx={{ 
                            flex: 1,
                            overflow: 'auto',
                            p: 2
                        }}>
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2, md: 2 }}>
                                {visiblePairs.map((pair) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                                        key={pair.id}
                                    >
                                        <PairCard pair={pair} />
                                    </Grid>
                                ))}
                            </Grid>
                        </Box>

                        {/* Desktop Pagination */}
                        <TablePagination
                            rowsPerPageOptions={[12, 24, 48]}
                            component="div"
                            count={filteredData.length}
                            rowsPerPage={rowsPerPage}
                            page={page}
                            onPageChange={handleChangePage}
                            onRowsPerPageChange={handleChangeRowsPerPage}
                            labelRowsPerPage="Rows:"
                            sx={{ 
                                borderTop: 1, 
                                borderColor: 'divider',
                                flexShrink: 0
                            }}
                        />
                    </Paper>
                </>
            )}

            <PairFilterDialog
                open={filterDialogOpen}
                onClose={() => setFilterDialogOpen(false)}
                tempFilters={tempFilters}
                onTempFiltersChange={setTempFilters}
                onClearPairId={clearPairIdFilter}
                onClearStatus={clearStatusFilter}
                logicalOperator={logicalOperator}
                onLogicalOperatorChange={setLogicalOperator}
                onApply={applyFiltersFromDialog}
                sortBy={String(orderBy)}
                onSortByChange={(value) => setOrderBy(value as keyof PairData)}
                sortOrder={order}
                onSortOrderChange={setOrder}
                sortableColumns={getSortablePairColumns()}
            />
        </>
    );
};

export default PairsPage;
