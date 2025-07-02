import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Grid, Paper, TablePagination, IconButton, Modal, Backdrop, Skeleton } from '@mui/material';
import { Add, FilterList, ChevronLeft, ChevronRight } from '@mui/icons-material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { useTaskData } from '../hooks/useTaskData';
import { type TaskData } from '../utils/taskMappers';
import TaskCard from '../components/tasks/TaskCard';
import TaskFilterDialog from '../components/tasks/TaskFilterDialog';
import ErrorAlert from '../components/common/ErrorAlert';
import type { TaskFilter } from '../hooks/useTaskFilters';
import { getSortableTaskColumns } from '../constants/taskColumns';

const TasksPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Date state
    const [selectedDate, setSelectedDate] = React.useState(dayjs());

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Filter state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [filters, setFilters] = React.useState<{
        taskType?: string;
        isCompleted?: boolean;
    }>({});
    const [logicalOperator, setLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    const [tempFilters, setTempFilters] = React.useState<{
        taskId: string;
        taskType: string;
        isCompleted: boolean | null;
    }>({ taskId: '', taskType: '', isCompleted: null });
    const [tempSortBy, setTempSortBy] = React.useState('dueOn');
    const [tempSortOrder, setTempSortOrder] = React.useState<'asc' | 'desc'>('asc');

    // Sorting state
    const [sortBy, setSortBy] = React.useState<string>('dueOn');
    const [sortOrder, setSortOrder] = React.useState<'asc' | 'desc'>('asc');

    // Convert filters for API
    const apiFilters = React.useMemo(() => {
        const converted: any = {};
        if (filters.taskType) {
            // Map task type string to number if needed
            converted.farmTaskType = 0; // Default to NestPreparation for now
        }
        if (typeof filters.isCompleted === 'boolean') {
            converted.isCompleted = filters.isCompleted;
        }
        return converted;
    }, [filters]);

    // Format date for API (YYYY-MM-DD)
    const formatDateForApi = (date: any) => {
        return date.format('YYYY-MM-DD');
    };

    // Fetch tasks data
    const {
        tasks,
        loading,
        error,
        totalCount,
        refetch,
        completeTask
    } = useTaskData({
        pageIndex: page,
        pageSize: rowsPerPage,
        dueOn: formatDateForApi(selectedDate),
        filters: apiFilters,
        logicalOperator: logicalOperator === 'AND' ? 0 : 1,
        sort: sortBy ? `${sortBy}:${sortOrder}` : undefined,
    });

    // Handlers
    const handleCompleteTask = async (taskId: number) => {
        try {
            await completeTask(taskId);
        } catch (err) {
            console.error('Failed to complete task:', err);
        }
    };

    // Clear individual filters
    const handleClearTaskTypeFilter = () => {
        setFilters(prev => ({ ...prev, taskType: undefined }));
        setPage(0);
    };

    const handleClearCompletedFilter = () => {
        setFilters(prev => ({ ...prev, isCompleted: undefined }));
        setPage(0);
    };

    // Filter chips component
    const FilterChips = () => {
        const hasFilters = filters.taskType || typeof filters.isCompleted === 'boolean';
        
        if (!hasFilters) return null;

        return (
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1, mb: 2, alignItems: 'center' }}>
                <Typography variant="body2" color="text.secondary" sx={{ mr: 1 }}>
                    Filters ({logicalOperator}):
                </Typography>
                
                {filters.taskType && (
                    <Chip
                        label={`TASK TYPE is "${filters.taskType}"`}
                        onDelete={handleClearTaskTypeFilter}
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

                {typeof filters.isCompleted === 'boolean' && (
                    <Chip
                        label={`IS COMPLETED is "${filters.isCompleted ? 'Yes' : 'No'}"`}
                        onDelete={handleClearCompletedFilter}
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

    // Update apply filters handler
    const handleApplyFiltersAndSort = React.useCallback(({ 
        filters, 
        sortBy: newSortBy, 
        sortOrder: newSortOrder 
    }: { 
        filters: { taskId: string; taskType: string; isCompleted: boolean | null }, 
        sortBy: string, 
        sortOrder: 'asc' | 'desc' 
    }) => {
        const newFilters: any = {};
        if (filters.taskType.trim()) newFilters.taskType = filters.taskType.trim();
        if (filters.isCompleted !== null) newFilters.isCompleted = filters.isCompleted;
        
        setFilters(newFilters);
        setSortBy(newSortBy || 'dueOn');
        setSortOrder(newSortOrder || 'asc');
        setPage(0);
        setFilterDialogOpen(false);
    }, []);

    // Open modal: sync temp state with current state
    const handleOpenFilterDialog = () => {
        setTempFilters({ 
            taskId: '', 
            taskType: filters.taskType || '', 
            isCompleted: filters.isCompleted !== undefined ? filters.isCompleted : null 
        });
        setTempSortBy(String(sortBy));
        setTempSortOrder(sortOrder);
        setFilterDialogOpen(true);
    };

    // Data calculation - no date filtering needed since API already filters by date
    const visibleTasks = React.useMemo(() => {
        return tasks.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    }, [tasks, page, rowsPerPage]);

    const getFilterLabel = (filter: TaskFilter) => {
        const columnLabels: Record<string, string> = {
            taskId: 'TASK ID',
            taskType: 'TASK TYPE',
            isCompleted: 'IS COMPLETED'
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

    const handlePreviousDay = () => {
        setSelectedDate(prev => prev.subtract(1, 'day'));
        setPage(0);
    };

    const handleNextDay = () => {
        setSelectedDate(prev => prev.add(1, 'day'));
        setPage(0);
    };

    const formatSelectedDate = (date: any) => {
        return date.format('ddd, MMM D, YYYY');
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Helmet>
                <title>Tasks Management - Farm Project</title>
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
                                    Tasks
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
                                </Box>
                            </Box>

                            {/* Date Navigation - Mobile */}
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1, mb: 2 }}>
                                <IconButton onClick={handlePreviousDay} size="small">
                                    <ChevronLeft />
                                </IconButton>
                                <DatePicker
                                    value={selectedDate}
                                    onChange={(newValue) => {
                                        if (newValue) {
                                            setSelectedDate(newValue);
                                            setPage(0);
                                        }
                                    }}
                                    slotProps={{
                                        textField: {
                                            size: 'small',
                                            sx: { 
                                                minWidth: 140,
                                                maxWidth: 160,
                                                '& .MuiInputBase-root': {
                                                    height: 36
                                                }
                                            }
                                        }
                                    }}
                                />
                                <IconButton onClick={handleNextDay} size="small">
                                    <ChevronRight />
                                </IconButton>
                            </Box>
                            
                            {/* Active Filters Display */}
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
                                    visibleTasks.map((task) => (
                                        <Grid 
                                            size={{ xs: 12, sm: 6 }}
                                            key={task.id}
                                        >
                                            <TaskCard 
                                                task={task} 
                                                onCompleteTask={handleCompleteTask}
                                            />
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
                            Tasks
                        </Typography>
                        {/* Date Navigation - Desktop */}
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                            <IconButton onClick={handlePreviousDay}>
                                <ChevronLeft />
                            </IconButton>
                            <DatePicker
                                value={selectedDate}
                                onChange={(newValue) => {
                                    if (newValue) {
                                        setSelectedDate(newValue);
                                        setPage(0);
                                    }
                                }}
                                slotProps={{
                                    textField: {
                                        size: 'small',
                                        sx: { 
                                            minWidth: 140,
                                            maxWidth: 160,
                                            '& .MuiInputBase-root': {
                                                height: 36
                                            }
                                        }
                                    }
                                }}
                            />
                            <IconButton onClick={handleNextDay}>
                                <ChevronRight />
                            </IconButton>
                        </Box>
                        <Box sx={{ display: 'flex', gap: 1 }}>
                            <Button
                                variant="outlined"
                                startIcon={<FilterList />}
                                onClick={handleOpenFilterDialog}
                            >
                                Filter
                            </Button>
                        </Box>
                    </Box>
                    
                    {/* Active Filters Display */}
                    <FilterChips />

                    <Divider sx={{ mb: 3 }} />
                    
                    {/* Cards Grid Container */}
                    <Paper sx={{ 
                        width: '100%', 
                        overflow: 'hidden', 
                        display: 'flex', 
                        flexDirection: 'column',
                        height: (filters.taskType || typeof filters.isCompleted === 'boolean') ? 'calc(100vh - 280px)' : 'calc(100vh - 240px)'
                    }}>
                        {/* Cards Grid */}
                        <Box sx={{ 
                            flex: 1,
                            overflow: 'auto',
                            p: 2
                        }}>
                            <Grid container rowSpacing={2} columnSpacing={{ xs: 1, sm: 2, md: 2 }}>
                                {loading ? (
                                    Array.from(new Array(rowsPerPage)).map((_, index) => (
                                        <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={`skeleton-${index}`}>
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
                                    visibleTasks.map((task) => (
                                        <Grid 
                                            size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                                            key={task.id}
                                        >
                                            <TaskCard 
                                                task={task} 
                                                onCompleteTask={handleCompleteTask}
                                            />
                                        </Grid>
                                    ))
                                )}
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

            <TaskFilterDialog
                open={filterDialogOpen}
                onClose={() => setFilterDialogOpen(false)}
                tempFilters={tempFilters}
                onTempFiltersChange={setTempFilters}
                onClearTaskId={() => setTempFilters((f) => ({ ...f, taskId: '' }))}
                onClearTaskType={() => setTempFilters((f) => ({ ...f, taskType: '' }))}
                onClearCompleted={() => setTempFilters((f) => ({ ...f, isCompleted: null }))}
                logicalOperator={logicalOperator}
                onLogicalOperatorChange={setLogicalOperator}
                onApply={handleApplyFiltersAndSort}
                sortBy={tempSortBy}
                sortOrder={tempSortOrder}
                sortableColumns={getSortableTaskColumns()}
            />
        </LocalizationProvider>
    );
};

export default TasksPage;
