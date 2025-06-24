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
    onApply: () => void;
    sortBy?: string;
    onSortByChange?: (sortBy: string) => void;
    sortOrder?: 'asc' | 'desc';
    onSortOrderChange?: (order: 'asc' | 'desc') => void;
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
    onSortByChange,
    sortOrder,
    onSortOrderChange,
    sortableColumns = []
}) => {
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
                        value={tempFilters.taskId}
                        onChange={(e) => onTempFiltersChange({ ...tempFilters, taskId: e.target.value })}
                        placeholder="Filter by task ID..."
                        InputProps={{
                            endAdornment: tempFilters.taskId && (
                                <IconButton onClick={onClearTaskId} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Task Type</InputLabel>
                        <Select
                            value={tempFilters.taskType}
                            onChange={(e) => onTempFiltersChange({ ...tempFilters, taskType: e.target.value })}
                            label="Task Type"
                            endAdornment={tempFilters.taskType && (
                                <IconButton onClick={onClearTaskType} size="small" sx={{ mr: 2 }}>
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

                    {sortBy && onSortByChange && sortOrder && onSortOrderChange && (
                        <>
                            <FormControl fullWidth>
                                <InputLabel>Sort By</InputLabel>
                                <Select
                                    value={sortBy}
                                    onChange={(e) => onSortByChange(e.target.value)}
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
                                    value={sortOrder}
                                    onChange={(e) => onSortOrderChange(e.target.value as 'asc' | 'desc')}
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
                                    checked={tempFilters.isCompleted === true}
                                    indeterminate={tempFilters.isCompleted === null}
                                    onChange={(e) => {
                                        if (tempFilters.isCompleted === null) {
                                            onTempFiltersChange({ ...tempFilters, isCompleted: true });
                                        } else if (tempFilters.isCompleted === true) {
                                            onTempFiltersChange({ ...tempFilters, isCompleted: false });
                                        } else {
                                            onTempFiltersChange({ ...tempFilters, isCompleted: null });
                                        }
                                    }}
                                />
                            }
                            label="Is Completed?"
                        />
                        {tempFilters.isCompleted !== null && (
                            <IconButton onClick={onClearCompleted} size="small">
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
                            onClick={onApply} 
                            variant="contained"
                            disabled={!tempFilters.taskId.trim() && !tempFilters.taskType.trim() && tempFilters.isCompleted === null}
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
