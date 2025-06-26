import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';
import { pairingStatusOptions } from '../../types/PairingStatus';

interface PairFilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        pairId: string;
        status: string;
    };
    onTempFiltersChange: (filters: { pairId: string; status: string }) => void;
    onClearPairId: () => void;
    onClearStatus: () => void;
    logicalOperator: 'AND' | 'OR';
    onLogicalOperatorChange: (operator: 'AND' | 'OR') => void;
    onApply: (params: { filters: { pairId: string; status: string }, sortBy: string, sortOrder: 'asc' | 'desc' }) => void;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const PairFilterDialog: React.FC<PairFilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    onTempFiltersChange,
    onClearPairId,
    onClearStatus,
    logicalOperator,
    onLogicalOperatorChange,
    onApply,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    // Local state for filters, sorting, and logical operator
    const [localFilters, setLocalFilters] = React.useState({ ...tempFilters });
    const [localSortBy, setLocalSortBy] = React.useState(sortBy || '');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>(sortOrder || 'asc');
    const [localLogicalOperator, setLocalLogicalOperator] = React.useState<'AND' | 'OR'>(logicalOperator);

    // Sync local state with props when dialog opens
    React.useEffect(() => {
        if (open) {
            setLocalFilters({ ...tempFilters });
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
            setLocalLogicalOperator(logicalOperator);
        }
    }, [open, tempFilters, sortBy, sortOrder, logicalOperator]);

    // Enable Apply if any filter, sort, or logical operator value has changed
    const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
    const filtersChanged =
        localFilters.pairId !== (tempFilters.pairId || '') ||
        localFilters.status !== (tempFilters.status || '');
    const logicalChanged = localLogicalOperator !== logicalOperator;
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
                        label="Pair ID"
                        value={localFilters.pairId}
                        onChange={(e) => setLocalFilters({ ...localFilters, pairId: e.target.value })}
                        placeholder="Filter by pair ID..."
                        InputProps={{
                            endAdornment: localFilters.pairId && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, pairId: '' }); onClearPairId(); }} size="small">
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
                            {pairingStatusOptions.map((status) => (
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

export default PairFilterDialog;
