import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Grid, Paper, TablePagination, Skeleton } from '@mui/material';
import { Add, FilterList } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { usePairData } from '../hooks/usePairData';
import { pairingStatusOptions, pairingStatusStringToEnum, getPairingStatusColor } from '../types/PairingStatus';
import PairCard from '../components/pairs/PairCard';
import ErrorAlert from '../components/common/ErrorAlert';
import FilterDialog from '../components/common/FilterDialog';
import AddPairModal from '../components/modals/AddPairModal';
import { PairService } from '../api/services/pairService';
import type { AddPairFormFields } from '../schemas/pairSchemas';

const PairsPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Modal state
    const [addModalOpen, setAddModalOpen] = React.useState(false);

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Filter state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [filters, setFilters] = React.useState<{
        pairId?: string;
        status?: string;
    }>({});
    const [logicalOperator, setLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    // Temp state for modal form
    const [tempFilters, setTempFilters] = React.useState<{
        pairId: string;
        status: string;
    }>({ pairId: '', status: '' });
    const [tempLogicalOperator, setTempLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    // Sorting state
    const [sortBy, setSortBy] = React.useState<string>('pairId');
    const [sortOrder, setSortOrder] = React.useState<'asc' | 'desc'>('asc');

    // Convert filters for API
    const apiFilters = React.useMemo(() => {
        const converted: any = {};
        if (filters.status && pairingStatusStringToEnum[filters.status] !== undefined) {
            converted.pairingStatus = pairingStatusStringToEnum[filters.status];
        }
        return converted;
    }, [filters]);

    // Map UI sort field to API sort field
    const getApiSortField = (uiSortField: string) => {
        if (uiSortField === 'pairId') return 'id';
        return uiSortField;
    };

    // Fetch pairs data
    const {
        pairs,
        loading,
        error,
        totalCount,
        refetch
    } = usePairData({
        pageIndex: page,
        pageSize: rowsPerPage,
        filters: apiFilters,
        logicalOperator: logicalOperator === 'AND' ? 0 : 1,
        sort: sortBy ? `${getApiSortField(sortBy)}:${sortOrder}` : undefined,
    });

    // Pagination handlers
    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    // Add pair handlers
    const handleOpenAddModal = () => {
        setAddModalOpen(true);
    };

    const handleCloseAddModal = () => {
        setAddModalOpen(false);
    };

    const handleAddPair = async (data: AddPairFormFields) => {
        try {
            await PairService.addPair(data.femaleRabbitId, data.maleRabbitId);
            setAddModalOpen(false);
            await refetch();
        } catch (err: any) {
            // Error handling should be done inside the AddPairModal component
            throw err;
        }
    };

    // Filter handlers
    const handleOpenFilterDialog = () => {
        setTempFilters({
            pairId: filters.pairId || '',
            status: filters.status || '',
        });
        setTempLogicalOperator(logicalOperator);
        setFilterDialogOpen(true);
    };

    const handleApplyFilters = ({
        filters: modalFilters,
        sortBy: modalSortBy,
        sortOrder: modalSortOrder,
        logicalOperator: modalLogicalOperator
    }: {
        filters: { pairId: string; status: string },
        sortBy: string,
        sortOrder: 'asc' | 'desc',
        logicalOperator: 'AND' | 'OR'
    }) => {
        
        const newFilters: any = {};
        if (modalFilters.pairId.trim()) newFilters.pairId = modalFilters.pairId.trim();
        if (modalFilters.status && modalFilters.status !== '') newFilters.status = modalFilters.status;
        
        setFilters(newFilters);
        setSortBy(modalSortBy || 'pairId');
        setSortOrder(modalSortOrder || 'asc');
        setLogicalOperator(modalLogicalOperator);
        setPage(0);
        setFilterDialogOpen(false);
    };

    // Clear individual filters
    const handleClearPairIdFilter = () => {
        setFilters(prev => ({ ...prev, pairId: undefined }));
        setPage(0);
    };

    const handleClearStatusFilter = () => {
        setFilters(prev => ({ ...prev, status: undefined }));
        setPage(0);
    };

    // Filter chips component
    const FilterChips = () => {
        const hasFilters = filters.pairId || filters.status;
        
        if (!hasFilters) return null;

        return (
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1, mb: 2, alignItems: 'center' }}>
                <Typography variant="body2" color="text.secondary" sx={{ mr: 1 }}>
                    Filters ({logicalOperator}):
                </Typography>
                
                {filters.pairId && (
                    <Chip
                        label={`PAIR ID contains "${filters.pairId}"`}
                        onDelete={handleClearPairIdFilter}
                        size="small"
                        variant="filled"
                        sx={{ 
                            backgroundColor: '#e0e0e0',
                            color: '#424242',
                            '& .MuiChip-deleteIcon': {
                                color: '#757575',
                                fontSize: '16px',
                                '&:hover': {
                                    color: '#424242'
                                }
                            }
                        }}
                    />
                )}
                
                {filters.status && (
                    <Chip
                        label={`STATUS is "${filters.status}"`}
                        onDelete={handleClearStatusFilter}
                        size="small"
                        variant="filled"
                        sx={{ 
                            backgroundColor: '#e0e0e0',
                            color: '#424242',
                            '& .MuiChip-deleteIcon': {
                                color: '#757575',
                                fontSize: '16px',
                                '&:hover': {
                                    color: '#424242'
                                }
                            }
                        }}
                    />
                )}
            </Box>
        );
    };

    // Skeleton cards for loading
    const skeletonCards = Array.from(new Array(rowsPerPage)).map((_, index) => (
        <Grid 
            size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
            key={`skeleton-${index}`}
        >
            <Paper sx={{ p: 2, height: '100%' }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Skeleton variant="text" width={120} height={32} />
                    <Skeleton variant="rounded" width={80} height={24} />
                </Box>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
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
    ));

    const mobileSkeleton = Array.from(new Array(rowsPerPage)).map((_, index) => (
        <Grid 
            size={{ xs: 12, sm: 6 }}
            key={`mobile-skeleton-${index}`}
        >
            <Paper sx={{ p: 2, height: '100%' }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                    <Skeleton variant="text" width={120} height={32} />
                    <Skeleton variant="rounded" width={80} height={24} />
                </Box>
                
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
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
    ));

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
                    <Box sx={{ 
                        flex: 1,
                        overflow: 'auto',
                        px: 2
                    }}>
                        <Box sx={{ py: 2 }}>
                            {error && <ErrorAlert message={error} onRetry={refetch} />}
                            
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2 }}>
                                {loading ? mobileSkeleton : pairs.map((pair) => (
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

                    {/* Cards Grid Container */}
                    <Paper sx={{ 
                        width: '100%', 
                        overflow: 'hidden', 
                        display: 'flex', 
                        flexDirection: 'column',
                        height: 'calc(100vh - 240px)'
                    }}>
                        {/* Cards Grid */}
                        <Box sx={{ 
                            flex: 1,
                            overflow: 'auto',
                            p: 2
                        }}>
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2, md: 2 }}>
                                {loading ? skeletonCards : pairs.map((pair) => (
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
                            count={totalCount}
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

            <FilterDialog
                open={filterDialogOpen}
                onClose={() => setFilterDialogOpen(false)}
                tempFilters={{ name: tempFilters.pairId, status: tempFilters.status }}
                onTempFiltersChange={(filters) => setTempFilters({ pairId: filters.name, status: filters.status })}
                statusOptions={pairingStatusOptions}
                onClearName={() => setTempFilters(f => ({ ...f, pairId: '' }))}
                onClearStatus={() => setTempFilters(f => ({ ...f, status: '' }))}
                logicalOperator={tempLogicalOperator}
                onLogicalOperatorChange={setTempLogicalOperator}
                onApply={({ filters: modalFilters, sortBy: modalSortBy, sortOrder: modalSortOrder, logicalOperator: modalLogicalOperator }) => {
                    handleApplyFilters({
                        filters: { pairId: modalFilters.name, status: modalFilters.status },
                        sortBy: modalSortBy,
                        sortOrder: modalSortOrder,
                        logicalOperator: modalLogicalOperator
                    });
                }}
                sortBy={sortBy}
                sortOrder={sortOrder}
                sortableColumns={[
                    { id: 'pairId', label: 'Pair ID' }
                ]}
            />

            <AddPairModal
                open={addModalOpen}
                onClose={handleCloseAddModal}
                onSubmit={handleAddPair}
            />
        </>
    );
};

export default PairsPage;