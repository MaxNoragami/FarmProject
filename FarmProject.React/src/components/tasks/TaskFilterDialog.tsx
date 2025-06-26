import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton, Checkbox, FormControlLabel } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';
import { farmTaskTypeOptions, getFarmTaskTypeLabel } from '../../types/FarmTaskType';

interface TaskFilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        taskId: string;
        taskType: string;
        isCompleted: boolean | null;
    };
    onTempFiltersChange: (filters: { taskId: string; taskType: string; isCompleted: boolean | null }) => void;
    onClearTaskId: () => void;
    onClearTaskType: () => void;
    onClearCompleted: () => void;
    logicalOperator: 'AND' | 'OR';
    onLogicalOperatorChange: (operator: 'AND' | 'OR') => void;
    onApply: (params: { filters: { taskId: string; taskType: string; isCompleted: boolean | null }, sortBy: string, sortOrder: 'asc' | 'desc' }) => void;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const TaskFilterDialog: React.FC<TaskFilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    onTempFiltersChange,
    onClearTaskId,
    onClearTaskType,
    onClearCompleted,
    logicalOperator,
    onLogicalOperatorChange,
    onApply,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    // Local state for filters and sorting
    const [localFilters, setLocalFilters] = React.useState({ ...tempFilters });
    const [localSortBy, setLocalSortBy] = React.useState(sortBy || '');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>(sortOrder || 'asc');

    // Sync local state with props when dialog opens
    React.useEffect(() => {
        if (open) {
            setLocalFilters({ ...tempFilters });
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
        }
    }, [open, tempFilters, sortBy, sortOrder]);

    const handleApply = () => {
        onApply({
            filters: localFilters,
            sortBy: localSortBy,
            sortOrder: localSortOrder
        });
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                Filter & Sort Options
                <IconButton onClick={onClose} size="small">
                    <Close />
                </IconButton>
            </DialogTitle>
            <DialogContent>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3, mt: 2 }}>
                    <TextField
                        fullWidth
                        label="Task ID"
                        value={localFilters.taskId}
                        onChange={(e) => setLocalFilters({ ...localFilters, taskId: e.target.value })}
                        placeholder="Filter by task ID..."
                        InputProps={{
                            endAdornment: localFilters.taskId && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, taskId: '' }); onClearTaskId(); }} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Task Type</InputLabel>
                        <Select
                            value={localFilters.taskType}
                            onChange={(e) => setLocalFilters({ ...localFilters, taskType: e.target.value })}
                            label="Task Type"
                            endAdornment={localFilters.taskType && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, taskType: '' }); onClearTaskType(); }} size="small" sx={{ mr: 2 }}>
                                    <Clear />
                                </IconButton>
                            )}
                        >
                            <MenuItem value="">All</MenuItem>
                            {farmTaskTypeOptions.map((type) => (
                                <MenuItem key={type} value={type}>
                                    {getFarmTaskTypeLabel(type)}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    {(sortBy && sortableColumns.length > 0) && (
                        <>
                            <FormControl fullWidth>
                                <InputLabel>Sort By</InputLabel>
                                <Select
                                    value={localSortBy}
                                    onChange={(e) => setLocalSortBy(e.target.value)}
                                    label="Sort By"
                                >
                                    {sortableColumns.map((column) => (
                                        <MenuItem key={column.id} value={column.id}>
                                            {column.label}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                            <FormControl fullWidth>
                                <InputLabel>Sort Order</InputLabel>
                                <Select
                                    value={localSortOrder}
                                    onChange={(e) => setLocalSortOrder(e.target.value as 'asc' | 'desc')}
                                    label="Sort Order"
                                >
                                    <MenuItem value="asc">Ascending</MenuItem>
                                    <MenuItem value="desc">Descending</MenuItem>
                                </Select>
                            </FormControl>
                        </>
                    )}
                    
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={localFilters.isCompleted === true}
                                    indeterminate={localFilters.isCompleted === null}
                                    onChange={() => {
                                        if (localFilters.isCompleted === null) {
                                            setLocalFilters({ ...localFilters, isCompleted: true });
                                        } else if (localFilters.isCompleted === true) {
                                            setLocalFilters({ ...localFilters, isCompleted: false });
                                        } else {
                                            setLocalFilters({ ...localFilters, isCompleted: null });
                                        }
                                    }}
                                />
                            }
                            label="Is Completed?"
                        />
                        {localFilters.isCompleted !== null && (
                            <IconButton onClick={() => { setLocalFilters({ ...localFilters, isCompleted: null }); onClearCompleted(); }} size="small">
                                <Clear />
                            </IconButton>
                        )}
                    </Box>

                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 2 }}>
                        <FormControl size="small" sx={{ minWidth: 120 }}>
                            <InputLabel>Logical Operator</InputLabel>
                            <Select
                                value={logicalOperator}
                                onChange={(e) => onLogicalOperatorChange(e.target.value as 'AND' | 'OR')}
                                label="Logical Operator"
                            >
                                <MenuItem value="AND">AND</MenuItem>
                                <MenuItem value="OR">OR</MenuItem>
                            </Select>
                        </FormControl>
                        
                        <Button 
                            onClick={handleApply} 
                            variant="contained"
                            disabled={
                                !(
                                    localFilters.taskId.trim() ||
                                    localFilters.taskType.trim() ||
                                    localFilters.isCompleted !== null ||
                                    (sortBy && (localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc')))
                                )
                            }
                        >
                            Apply
                        </Button>
                    </Box>
                </Box>
            </DialogContent>
        </Dialog>
    );
};

export default TaskFilterDialog;
