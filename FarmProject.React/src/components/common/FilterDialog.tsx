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
    onApply: (params: { filters: { name: string; status: string }, sortBy: string, sortOrder: 'asc' | 'desc', logicalOperator: 'AND' | 'OR' }) => void;
    isMobile?: boolean;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const FilterDialog: React.FC<FilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    statusOptions,
    logicalOperator,
    onApply,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    const [name, setName] = React.useState('');
    const [status, setStatus] = React.useState('');
    const [localSortBy, setLocalSortBy] = React.useState('');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>('asc');
    const [localLogicalOperator, setLocalLogicalOperator] = React.useState<'AND' | 'OR'>('AND');

    React.useEffect(() => {
        if (open) {
            setName(tempFilters.name);
            setStatus(tempFilters.status);
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
            setLocalLogicalOperator(logicalOperator);
        }
    }, [open, tempFilters, sortBy, sortOrder, logicalOperator]);

    // Check if any changes have been made
    const hasChanges = React.useMemo(() => {
        const filtersChanged = name !== tempFilters.name || status !== tempFilters.status;
        const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
        const operatorChanged = localLogicalOperator !== logicalOperator;
        
        return filtersChanged || sortChanged || operatorChanged;
    }, [name, status, localSortBy, localSortOrder, localLogicalOperator, tempFilters, sortBy, sortOrder, logicalOperator]);

    const handleApply = () => {
        onApply({
            filters: { name, status },
            sortBy: localSortBy,
            sortOrder: localSortOrder,
            logicalOperator: localLogicalOperator
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
                        label="Name"
                        variant="outlined"
                        fullWidth
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        slotProps={{
                            input: {
                                endAdornment: name && (
                                    <IconButton onClick={() => setName('')} size="small">
                                        <Clear />
                                    </IconButton>
                                )
                            }
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Status</InputLabel>
                        <Select
                            value={status}
                            onChange={(e) => setStatus(e.target.value)}
                            label="Status"
                            endAdornment={status && (
                                <IconButton onClick={() => setStatus('')} size="small" sx={{ mr: 2 }}>
                                    <Clear />
                                </IconButton>
                            )}
                        >
                            <MenuItem value="">All</MenuItem>
                            {statusOptions.map((statusOption) => (
                                <MenuItem key={statusOption} value={statusOption}>
                                    {statusOption}
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
                                onChange={(e) => setLocalLogicalOperator(e.target.value as 'AND' | 'OR')}
                                label="Logical Operator"
                            >
                                <MenuItem value="AND">AND</MenuItem>
                                <MenuItem value="OR">OR</MenuItem>
                            </Select>
                        </FormControl>
                        
                        <Button 
                            onClick={handleApply} 
                            variant="contained"
                            disabled={!hasChanges}
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