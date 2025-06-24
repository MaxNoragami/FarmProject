import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Grid, Paper, TablePagination } from '@mui/material';
import { Add, FilterList } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { mockCagesData, type CageData } from '../data/mockCageData';
import { useCageFilters, type CageFilter } from '../hooks/useCageFilters';
import CageCard from '../components/cages/CageCard';
import CageFilterDialog from '../components/cages/CageFilterDialog';
import { useCageSorting } from '../hooks/useCageSorting';
import { getSortableCageColumns } from '../constants/cageColumns';

const CagesPage = () => {
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
    } = useCageFilters(mockCagesData);

    // Filter dialog state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [tempFilters, setTempFilters] = React.useState<{
        name: string;
        offspringType: string;
        isOccupied: boolean | null;
    }>({ name: '', offspringType: '', isOccupied: null });

    // Sort state
    const {
        order,
        orderBy,
        sortedData,
        handleRequestSort,
        setOrder,
        setOrderBy
    } = useCageSorting(filteredData, 'cageId');

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Handlers
    const handleAddCage = () => {
        console.log('Add cage clicked');
    };

    const clearNameFilter = () => {
        const newTempFilters = { ...tempFilters, name: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearOffspringTypeFilter = () => {
        const newTempFilters = { ...tempFilters, offspringType: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearOccupiedFilter = () => {
        const newTempFilters = { ...tempFilters, isOccupied: null };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const updateFiltersFromTemp = (filters: { name: string; offspringType: string; isOccupied: boolean | null }) => {
        const newFilters: CageFilter[] = [];
        
        if (filters.name.trim()) {
            newFilters.push({
                id: `name-${Date.now()}`,
                column: 'name',
                operator: 'contains',
                value: filters.name.trim()
            });
        }
        
        if (filters.offspringType.trim()) {
            newFilters.push({
                id: `offspringType-${Date.now()}`,
                column: 'offspringType',
                operator: 'equals',
                value: filters.offspringType.trim()
            });
        }

        if (filters.isOccupied !== null) {
            newFilters.push({
                id: `isOccupied-${Date.now()}`,
                column: 'isOccupied',
                operator: 'equals',
                value: String(filters.isOccupied)
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
            } else if (filterToRemove.column === 'offspringType') {
                setTempFilters({ ...tempFilters, offspringType: '' });
            } else if (filterToRemove.column === 'isOccupied') {
                setTempFilters({ ...tempFilters, isOccupied: null });
            }
        }
    };

    const applyFiltersFromDialog = () => {
        updateFiltersFromTemp(tempFilters);
        setFilterDialogOpen(false);
    };

    // Data calculation
    const visibleCages = React.useMemo(() => {
        return sortedData.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    }, [sortedData, page, rowsPerPage]);

    const getFilterLabel = (filter: CageFilter) => {
        const columnLabels: Record<string, string> = {
            name: 'NAME',
            offspringType: 'OFFSPRING TYPE',
            isOccupied: 'IS OCCUPIED'
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
                <title>Cages Management - Farm Project</title>
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
                                    Cages
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
                                        onClick={handleAddCage}
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
                                {visibleCages.map((cage) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6 }}
                                        key={cage.id}
                                    >
                                        <CageCard cage={cage} />
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
                            Cages
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
                                onClick={handleAddCage}
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
                                {visibleCages.map((cage) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                                        key={cage.id}
                                    >
                                        <CageCard cage={cage} />
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

            <CageFilterDialog
                open={filterDialogOpen}
                onClose={() => setFilterDialogOpen(false)}
                tempFilters={tempFilters}
                onTempFiltersChange={setTempFilters}
                onClearName={clearNameFilter}
                onClearOffspringType={clearOffspringTypeFilter}
                onClearOccupied={clearOccupiedFilter}
                logicalOperator={logicalOperator}
                onLogicalOperatorChange={setLogicalOperator}
                onApply={applyFiltersFromDialog}
                sortBy={String(orderBy)}
                onSortByChange={(value) => setOrderBy(value as keyof CageData)}
                sortOrder={order}
                onSortOrderChange={setOrder}
                sortableColumns={getSortableCageColumns()}
            />
        </>
    );
};

export default CagesPage;

