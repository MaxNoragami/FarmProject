import { Typography, Box, Button, Divider, Chip, useMediaQuery, useTheme, Grid, Paper, TablePagination, IconButton, Modal, Backdrop } from '@mui/material';
import { Add, FilterList, ChevronLeft, ChevronRight } from '@mui/icons-material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';
import * as React from 'react';
import { Helmet } from 'react-helmet-async';
import { mockTasksData, type TaskData } from '../data/mockTaskData';
import { useTaskFilters, type TaskFilter } from '../hooks/useTaskFilters';
import { useTaskSorting } from '../hooks/useTaskSorting';
import { getSortableTaskColumns } from '../constants/taskColumns';
import TaskCard from '../components/tasks/TaskCard';
import TaskFilterDialog from '../components/tasks/TaskFilterDialog';

const TasksPage = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    // Tasks state
    const [tasks, setTasks] = React.useState(mockTasksData);
    
    // Date state (using dayjs)
    const [selectedDate, setSelectedDate] = React.useState(dayjs());

    // Filter state
    const {
        filters,
        setFilters,
        logicalOperator,
        setLogicalOperator,
        filteredData,
        removeFilter
    } = useTaskFilters(tasks);

    // Sort state
    const {
        order,
        orderBy,
        sortedData,
        handleRequestSort,
        setOrder,
        setOrderBy
    } = useTaskSorting(filteredData, 'dueOn');

    // Pagination state
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(12);

    // Filter dialog state
    const [filterDialogOpen, setFilterDialogOpen] = React.useState(false);
    const [tempFilters, setTempFilters] = React.useState<{
        taskId: string;
        taskType: string;
        isCompleted: boolean | null;
    }>({ taskId: '', taskType: '', isCompleted: null });

    // Handlers
    const handleToggleTaskComplete = (taskId: string, newStatus: boolean) => {
        setTasks(prevTasks => 
            prevTasks.map(task => 
                task.taskId === taskId 
                    ? { ...task, isCompleted: newStatus }
                    : task
            )
        );
    };

    const clearTaskIdFilter = () => {
        const newTempFilters = { ...tempFilters, taskId: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearTaskTypeFilter = () => {
        const newTempFilters = { ...tempFilters, taskType: '' };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const clearCompletedFilter = () => {
        const newTempFilters = { ...tempFilters, isCompleted: null };
        setTempFilters(newTempFilters);
        updateFiltersFromTemp(newTempFilters);
    };

    const updateFiltersFromTemp = (filters: { taskId: string; taskType: string; isCompleted: boolean | null }) => {
        const newFilters: TaskFilter[] = [];
        
        if (filters.taskId.trim()) {
            newFilters.push({
                id: `taskId-${Date.now()}`,
                column: 'taskId',
                operator: 'contains',
                value: filters.taskId.trim()
            });
        }
        
        if (filters.taskType.trim()) {
            newFilters.push({
                id: `taskType-${Date.now()}`,
                column: 'taskType',
                operator: 'equals',
                value: filters.taskType.trim()
            });
        }

        if (filters.isCompleted !== null) {
            newFilters.push({
                id: `isCompleted-${Date.now()}`,
                column: 'isCompleted',
                operator: 'equals',
                value: String(filters.isCompleted)
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
            if (filterToRemove.column === 'taskId') {
                setTempFilters({ ...tempFilters, taskId: '' });
            } else if (filterToRemove.column === 'taskType') {
                setTempFilters({ ...tempFilters, taskType: '' });
            } else if (filterToRemove.column === 'isCompleted') {
                setTempFilters({ ...tempFilters, isCompleted: null });
            }
        }
    };

    const applyFiltersFromDialog = () => {
        updateFiltersFromTemp(tempFilters);
        setFilterDialogOpen(false);
    };

    // Data calculation with date filtering (updated for dayjs)
    const visibleTasks = React.useMemo(() => {
        const dateFilteredTasks = sortedData.filter(task => {
            const taskDueDate = dayjs(task.dueOn);
            return taskDueDate.isSame(selectedDate, 'day');
        });
        
        return dateFilteredTasks.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    }, [sortedData, page, rowsPerPage, selectedDate]);

    // Update filtered data count to include date filtering (updated for dayjs)
    const dateFilteredCount = React.useMemo(() => {
        return sortedData.filter(task => {
            const taskDueDate = dayjs(task.dueOn);
            return taskDueDate.isSame(selectedDate, 'day');
        }).length;
    }, [sortedData, selectedDate]);

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
                                        onClick={() => setFilterDialogOpen(true)}
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
                                {visibleTasks.map((task) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6 }}
                                        key={task.id}
                                    >
                                        <TaskCard 
                                            task={task} 
                                            onToggleComplete={handleToggleTaskComplete}
                                        />
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
                                count={dateFilteredCount}
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
                                onClick={() => setFilterDialogOpen(true)}
                            >
                                Filter
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
                                {visibleTasks.map((task) => (
                                    <Grid 
                                        size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                                        key={task.id}
                                    >
                                        <TaskCard 
                                            task={task} 
                                            onToggleComplete={handleToggleTaskComplete}
                                        />
                                    </Grid>
                                ))}
                            </Grid>
                        </Box>

                        {/* Desktop Pagination */}
                        <TablePagination
                            rowsPerPageOptions={[12, 24, 48]}
                            component="div"
                            count={dateFilteredCount}
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
                onClearTaskId={clearTaskIdFilter}
                onClearTaskType={clearTaskTypeFilter}
                onClearCompleted={clearCompletedFilter}
                logicalOperator={logicalOperator}
                onLogicalOperatorChange={setLogicalOperator}
                onApply={applyFiltersFromDialog}
                sortBy={String(orderBy)}
                onSortByChange={(value) => setOrderBy(value as keyof TaskData)}
                sortOrder={order}
                onSortOrderChange={setOrder}
                sortableColumns={getSortableTaskColumns()}
            />
        </LocalizationProvider>
    );
};

export default TasksPage;
