import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton, Checkbox, FormControlLabel } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';
import { farmTaskTypeOptions, getFarmTaskTypeLabel } from '../../types/FarmTaskType';

interface TaskFilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        taskType: string;
        isCompleted: boolean | null;
    };
    onTempFiltersChange: (filters: { taskType: string; isCompleted: boolean | null }) => void;
    onClearTaskType: () => void;
    onClearCompleted: () => void;
    onApply: (params: { filters: { taskType: string; isCompleted: boolean | null }, sortBy: string, sortOrder: 'asc' | 'desc' }) => void;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const TaskFilterDialog: React.FC<TaskFilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    onTempFiltersChange,
    onClearTaskType,
    onClearCompleted,
    onApply,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    // Local state for filters and sorting
    const [localFilters, setLocalFilters] = React.useState({ ...tempFilters });
    const [localSortBy, setLocalSortBy] = React.useState(sortBy || '');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>(sortOrder || 'asc');

    // Store the element that opened the dialog
    const triggerElementRef = React.useRef<HTMLElement | null>(null);

    // Store the trigger element when dialog opens
    React.useEffect(() => {
        if (open) {
            triggerElementRef.current = document.activeElement as HTMLElement;
            setLocalFilters({ ...tempFilters });
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
        }
    }, [open, tempFilters, sortBy, sortOrder]);

    // Handle dialog close with proper focus restoration
    const handleClose = React.useCallback(() => {
        onClose();
        setTimeout(() => {
            if (triggerElementRef.current && triggerElementRef.current.focus) {
                triggerElementRef.current.focus();
            }
        }, 100);
    }, [onClose]);

    // Enable Apply if any filter or sort value has changed
    const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
    const filtersChanged =
        localFilters.taskType !== (tempFilters.taskType || '') ||
        localFilters.isCompleted !== (tempFilters.isCompleted ?? null);
    const canApply = sortChanged || filtersChanged;

    const handleApply = () => {
        onApply({
            filters: localFilters,
            sortBy: localSortBy,
            sortOrder: localSortOrder
        });
    };

    return (
        <Dialog 
            open={open} 
            onClose={handleClose} 
            maxWidth="sm" 
            fullWidth
            disableEnforceFocus={false}
            disableAutoFocus={false}
            disableRestoreFocus={true}
            BackdropProps={{
                timeout: 500,
            }}
        >
            <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                Filter & Sort Options
                <IconButton onClick={handleClose} size="small">
                    <Close />
                </IconButton>
            </DialogTitle>
            <DialogContent>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3, mt: 2 }}>
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

                    <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                        <Button 
                            onClick={handleApply} 
                            variant="contained"
                            disabled={!canApply}
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
