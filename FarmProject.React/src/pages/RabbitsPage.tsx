import { Typography, Box, Button, Divider, useMediaQuery, useTheme, TablePagination, Chip, Grid, Paper, Skeleton } from '@mui/material';
import { FilterList, Add } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import RabbitTable from '../components/rabbits/RabbitTable';
import ErrorAlert from '../components/common/ErrorAlert';
import FilterDialog from '../components/common/FilterDialog';
import { breedingStatusOptions, breedingStatusStringToEnum, getBreedingStatusColor } from '../types/BreedingStatus';
import { useRabbitData } from '../hooks/useRabbitData';
import AddRabbitModal from '../components/modals/AddRabbitModal';
import BirthModal from '../components/modals/BirthModal';
import { RabbitService } from '../api/services/rabbitService';
import { type AddRabbitFormFields } from '../schemas/rabbitSchemas';
import { type RabbitData } from '../utils/rabbitMappers';

const RabbitsPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Modal state
    const [addModalOpen, setAddModalOpen] = React.useState(false);
    const [addRabbitError, setAddRabbitError] = React.useState<string | null>(null);

    // Birth modal state
    const [birthModalOpen, setBirthModalOpen] = React.useState(false);
    const [birthModalRabbit, setBirthModalRabbit] = React.useState<RabbitData | null>(null);
    const [birthModalError, setBirthModalError] = React.useState<string | null>(null);

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    // Filter state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [filters, setFilters] = React.useState<{
        name?: string;
        status?: string;
    }>({});
    const [logicalOperator, setLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    // Temp state for modal form
    const [tempFilters, setTempFilters] = React.useState<{
        name: string;
        status: string;
    }>({ name: '', status: '' });
    const [tempLogicalOperator, setTempLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    // Sorting state
    const [sortBy, setSortBy] = React.useState<string>('rabbitId');
    const [sortOrder, setSortOrder] = React.useState<'asc' | 'desc'>('asc');

    // Convert filters for API
    const apiFilters = React.useMemo(() => {
        const converted: any = {};
        if (filters.name) converted.name = filters.name;
        if (filters.status && breedingStatusStringToEnum[filters.status] !== undefined) {
            converted.breedingStatus = breedingStatusStringToEnum[filters.status];
        }
        return converted;
    }, [filters]);

    // Map UI sort field to API sort field
    const getApiSortField = (uiSortField: string) => {
        if (uiSortField === 'rabbitId') return 'id';
        return uiSortField;
    };

    // Fetch rabbits data
    const {
        rabbits,
        loading,
        error,
        totalCount,
        refetch
    } = useRabbitData({
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

    // Add rabbit handlers
    const handleOpenAddModal = () => {
        setAddRabbitError(null);
        setAddModalOpen(true);
    };

    const handleCloseAddModal = () => {
        setAddModalOpen(false);
        setAddRabbitError(null);
    };

    const handleAddRabbit = async (data: AddRabbitFormFields) => {
        setAddRabbitError(null);
        try {
            await RabbitService.addRabbit(data.name, data.cageId);
            setAddModalOpen(false);
            await refetch();
        } catch (err: any) {
            setAddRabbitError(
                err?.response?.data?.message ||
                err?.message ||
                "An unexpected error occurred while adding the rabbit."
            );
            throw err;
        }
    };

    // Birth handlers
    const handleOpenBirthModal = (rabbit: RabbitData) => {
        if (rabbit.status === 'Pregnant') {
            setBirthModalRabbit(rabbit);
            setBirthModalError(null);
            setBirthModalOpen(true);
        }
    };

    const handleCloseBirthModal = () => {
        setBirthModalOpen(false);
        setBirthModalError(null);
    };

    const handleRegisterBirth = async (offspringCount: number) => {
        if (!birthModalRabbit) return;
        
        setBirthModalError(null);
        try {
            await RabbitService.registerBirth(birthModalRabbit.rabbitId, offspringCount);
            setBirthModalOpen(false);
            await refetch();
        } catch (err: any) {
            setBirthModalError(
                err?.response?.data?.message ||
                err?.message ||
                "An unexpected error occurred while registering birth."
            );
            throw err;
        }
    };

    // Filter handlers
    const handleOpenFilterDialog = () => {
        setTempFilters({
            name: filters.name || '',
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
        filters: { name: string; status: string },
        sortBy: string,
        sortOrder: 'asc' | 'desc',
        logicalOperator: 'AND' | 'OR'
    }) => {
        
        const newFilters: any = {};
        if (modalFilters.name.trim()) newFilters.name = modalFilters.name.trim();
        if (modalFilters.status && modalFilters.status !== '') newFilters.status = modalFilters.status;
        
        setFilters(newFilters);
        setSortBy(modalSortBy || 'rabbitId');
        setSortOrder(modalSortOrder || 'asc');
        setLogicalOperator(modalLogicalOperator);
        setPage(0);
        setFilterDialogOpen(false);
    };

    // Clear individual filters
    const handleClearNameFilter = () => {
        setFilters(prev => ({ ...prev, name: undefined }));
        setPage(0);
    };

    const handleClearStatusFilter = () => {
        setFilters(prev => ({ ...prev, status: undefined }));
        setPage(0);
    };

    // Filter chips component
    const FilterChips = () => {
        const hasFilters = filters.name || filters.status;
        
        if (!hasFilters) return null;

        return (
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1, mb: 2, alignItems: 'center' }}>
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

    const isRabbitClickable = (rabbit: RabbitData) => {
        return rabbit.status === 'Pregnant';
    };

    return (
        <>
            <Helmet>
                <title>Rabbits Management - Farm Project</title>
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
                                {loading ? (
                                    Array.from(new Array(rowsPerPage)).map((_, index) => (
                                        <Grid size={{ xs: 12, sm: 6 }} key={`skeleton-${index}`}>
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
                                    ))
                                ) : (
                                    rabbits.map((rabbit) => (
                                        <Grid size={{ xs: 12, sm: 6 }} key={rabbit.rabbitId}>
                                            <Paper 
                                                sx={{ 
                                                    p: 2, 
                                                    height: '100%',
                                                    cursor: isRabbitClickable(rabbit) ? 'pointer' : 'default',
                                                    '&:hover': isRabbitClickable(rabbit) ? {
                                                        backgroundColor: 'rgba(0, 0, 0, 0.04)',
                                                    } : {}
                                                }}
                                                onClick={() => handleOpenBirthModal(rabbit)}
                                            >
                                                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                                                    <Typography variant="h6" component="div">
                                                        {rabbit.name}
                                                    </Typography>
                                                    <Chip 
                                                        label={rabbit.status} 
                                                        color={getBreedingStatusColor(rabbit.status)} 
                                                        size="small" 
                                                    />
                                                </Box>
                                                
                                                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
                                                    <Box>
                                                        <Typography variant="body2" color="text.secondary">
                                                            RABBIT ID
                                                        </Typography>
                                                        <Typography variant="body1" fontWeight="medium">
                                                            {rabbit.rabbitId}
                                                        </Typography>
                                                    </Box>
                                                    <Box>
                                                        <Typography variant="body2" color="text.secondary">
                                                            CAGE ID
                                                        </Typography>
                                                        <Typography variant="body1" fontWeight="medium">
                                                            {rabbit.cageId}
                                                        </Typography>
                                                    </Box>
                                                </Box>
                                            </Paper>
                                        </Grid>
                                    ))
                                )}
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
                                rowsPerPageOptions={[10, 25, 100]}
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
                            Breeding Rabbits
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

                    {/* Table Container with fixed height */}
                    <Paper sx={{ 
                        width: '100%', 
                        overflow: 'hidden', 
                        display: 'flex', 
                        flexDirection: 'column',
                        height: 'calc(100vh - 240px)'
                    }}>
                        {/* Scrollable Table */}
                        <Box sx={{ 
                            flex: 1,
                            overflow: 'auto'
                        }}>
                            <RabbitTable 
                                rabbits={rabbits}
                                loading={loading}
                                sortBy={sortBy}
                                sortOrder={sortOrder}
                                onSort={(field: string) => {
                                    if (sortBy === field) {
                                        setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
                                    } else {
                                        setSortBy(field);
                                        setSortOrder('asc');
                                    }
                                    setPage(0);
                                }}
                                onRabbitClick={handleOpenBirthModal}
                                isRabbitClickable={isRabbitClickable}
                            />
                        </Box>

                        {/* Fixed Pagination */}
                        <TablePagination
                            rowsPerPageOptions={[10, 25, 100]}
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
                tempFilters={tempFilters}
                onTempFiltersChange={setTempFilters}
                statusOptions={breedingStatusOptions}
                onClearName={() => setTempFilters(f => ({ ...f, name: '' }))}
                onClearStatus={() => setTempFilters(f => ({ ...f, status: '' }))}
                logicalOperator={tempLogicalOperator}
                onLogicalOperatorChange={setTempLogicalOperator}
                onApply={handleApplyFilters}
                sortBy={sortBy}
                sortOrder={sortOrder}
                sortableColumns={[
                    { id: 'rabbitId', label: 'Rabbit ID' },
                    { id: 'name', label: 'Name' }
                ]}
            />

            <AddRabbitModal
                open={addModalOpen}
                onClose={handleCloseAddModal}
                onSubmit={handleAddRabbit}
                error={addRabbitError}
            />

            <BirthModal
                open={birthModalOpen}
                onClose={handleCloseBirthModal}
                onSubmit={handleRegisterBirth}
                rabbit={birthModalRabbit}
                error={birthModalError}
            />
        </>
    );
};

export default RabbitsPage;
