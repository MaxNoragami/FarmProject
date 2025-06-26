import { Typography, Box, Button, Divider, useMediaQuery, useTheme, Grid, Paper, TablePagination } from '@mui/material';
import { Add, FilterList } from '@mui/icons-material';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import CageCard from '../components/cages/CageCard';
import CageCardSkeleton from '../components/common/CageCardSkeleton';
import AddCageModal from '../components/modals/AddCageModal';
import type { AddCageFormFields } from '../schemas/cageSchemas';
import { useCageData } from '../hooks/useCageData';
import ErrorAlert from '../components/common/ErrorAlert';
import { CageService } from '../services/CageService';

const CagesPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Add modal state
    const [addModalOpen, setAddModalOpen] = React.useState(false);
    const [addCageError, setAddCageError] = React.useState<string | null>(null);

    // Fetch cages data
    const { 
        cages, 
        loading, 
        error, 
        totalCount, 
        refetch 
    } = useCageData({ 
        pageIndex: page, 
        pageSize: rowsPerPage 
    });

    // Handlers
    const handleAddCage = () => {
        setAddCageError(null);
        setAddModalOpen(true);
    };

    const handleModalClose = () => {
        setAddModalOpen(false);
        setAddCageError(null);
    };

    const handleSubmitNewCage = async (data: AddCageFormFields) => {
        setAddCageError(null);
        try {
            await CageService.addCage(data.name);
            setAddModalOpen(false);
            await refetch();
        } catch (err: any) {
            setAddCageError(
                err?.response?.data?.message ||
                err?.message ||
                "An unexpected error occurred."
            );
        }
    };

    // Pagination handlers
    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    // Skeleton cards for when loading
    const skeletonCards = Array.from(new Array(rowsPerPage)).map((_, index) => (
        <Grid 
            size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
            key={`skeleton-${index}`}
        >
            <CageCardSkeleton />
        </Grid>
    ));

    const mobileSkeleton = Array.from(new Array(rowsPerPage)).map((_, index) => (
        <Grid 
            size={{ xs: 12, sm: 6 }}
            key={`mobile-skeleton-${index}`}
        >
            <CageCardSkeleton />
        </Grid>
    ));

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
                                        disabled
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
                                {loading ? mobileSkeleton : cages.map((cage) => (
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
                            Cages
                        </Typography>

                        <Box sx={{ display: 'flex', gap: 1 }}>
                            <Button
                                variant="outlined"
                                startIcon={<FilterList />}
                                disabled
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

                    <Divider sx={{ mb: 3 }} />
                    
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
                            {error && <ErrorAlert message={error} onRetry={refetch} />}
                            
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2, md: 2 }}>
                                {loading ? skeletonCards : cages.map((cage) => (
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

            <AddCageModal
                open={addModalOpen}
                onClose={handleModalClose}
                onSubmit={handleSubmitNewCage}
                error={addCageError}
            />
        </>
    );
};

export default CagesPage;