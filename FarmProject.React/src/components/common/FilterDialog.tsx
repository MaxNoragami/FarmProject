import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';

interface FilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        name: string;
        status: string;
    };
    onTempFiltersChange: (filters: { name: string; status: string }) => void;
    statusOptions: string[];
    onClearName: () => void;
    onClearStatus: () => void;
    logicalOperator: 'AND' | 'OR';
    onLogicalOperatorChange: (operator: 'AND' | 'OR') => void;
    onApply: (params: { filters: { name: string; status: string }, sortBy: string, sortOrder: 'asc' | 'desc' }) => void;
    isMobile?: boolean;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const FilterDialog: React.FC<FilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    onTempFiltersChange,
    statusOptions,
    onClearName,
    onClearStatus,
    logicalOperator,
    onLogicalOperatorChange,
    onApply,
    isMobile = false,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    // Local state for sortBy and sortOrder
    const [localSortBy, setLocalSortBy] = React.useState(sortBy || '');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>(sortOrder || 'asc');
    const [localFilters, setLocalFilters] = React.useState({ ...tempFilters });
    const [localLogicalOperator, setLocalLogicalOperator] = React.useState<'AND' | 'OR'>(logicalOperator);

    // Sync local state with props when dialog opens
    React.useEffect(() => {
        if (open) {
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
            setLocalFilters({ ...tempFilters });
            setLocalLogicalOperator(logicalOperator);
        }
    }, [open, sortBy, sortOrder, tempFilters, logicalOperator]);

    // Track if sort/filter/logical values have changed from initial values
    const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
    const filtersChanged =
        localFilters.name !== (tempFilters.name || '') ||
        localFilters.status !== (tempFilters.status || '');
    const logicalChanged = localLogicalOperator !== logicalOperator;

    // Enable Apply if any filter, sort, or logical operator value has changed
    const canApply = sortChanged || filtersChanged || logicalChanged;

    const handleApply = () => {
        onLogicalOperatorChange(localLogicalOperator);
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
                        label="Name"
                        value={localFilters.name}
                        onChange={(e) => setLocalFilters({ ...localFilters, name: e.target.value })}
                        placeholder="Filter by name..."
                        InputProps={{
                            endAdornment: localFilters.name && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, name: '' }); onClearName(); }} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Status</InputLabel>
                        <Select
                            value={localFilters.status}
                            onChange={(e) => setLocalFilters({ ...localFilters, status: e.target.value })}
                            label="Status"
                            endAdornment={localFilters.status && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, status: '' }); onClearStatus(); }} size="small" sx={{ mr: 2 }}>
                                    <Clear />
                                </IconButton>
                            )}
                        >
                            <MenuItem value="">All</MenuItem>
                            {statusOptions.map((status) => (
                                <MenuItem key={status} value={status}>
                                    {status}
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
                    
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 2 }}>
                        <FormControl size="small" sx={{ minWidth: 120 }}>
                            <InputLabel>Logical Operator</InputLabel>
                            <Select
                                value={localLogicalOperator}
                                onChange={(e) => {
                                    const value = e.target.value as 'AND' | 'OR';
                                    setLocalLogicalOperator(value === 'AND' || value === 'OR' ? value : 'AND');
                                }}
                                label="Logical Operator"
                            >
                                <MenuItem value="AND">AND</MenuItem>
                                <MenuItem value="OR">OR</MenuItem>
                            </Select>
                        </FormControl>
                        
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

export default FilterDialog;
