import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton, Checkbox, FormControlLabel } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';
import { offspringTypeOptions } from '../../types/OffspringType';

interface CageFilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        name: string;
        offspringType: string;
        isOccupied: boolean | null;
    };
    onTempFiltersChange: (filters: { name: string; offspringType: string; isOccupied: boolean | null }) => void;
    onClearName: () => void;
    onClearOffspringType: () => void;
    onClearOccupied: () => void;
    logicalOperator: 'AND' | 'OR';
    onLogicalOperatorChange: (operator: 'AND' | 'OR') => void;
    onApply: (params: { filters: { name: string; offspringType: string; isOccupied: boolean | null }, sortBy: string, sortOrder: 'asc' | 'desc' }) => void;
    sortBy?: string;
    onSortByChange?: (sortBy: string) => void;
    sortOrder?: 'asc' | 'desc';
    onSortOrderChange?: (order: 'asc' | 'desc') => void;
    sortableColumns?: Array<{ id: string; label: string }>;
}

const CageFilterDialog: React.FC<CageFilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    onTempFiltersChange,
    onClearName,
    onClearOffspringType,
    onClearOccupied,
    logicalOperator,
    onLogicalOperatorChange,
    onApply,
    sortBy,
    onSortByChange,
    sortOrder,
    onSortOrderChange,
    sortableColumns = []
}) => {
    // Local state for filters and sorting
    const [localFilters, setLocalFilters] = React.useState({ ...tempFilters });
    const [localSortBy, setLocalSortBy] = React.useState(sortBy || '');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>(sortOrder || 'asc');
    const [localLogicalOperator, setLocalLogicalOperator] = React.useState<'AND' | 'OR'>(logicalOperator || 'AND');

    // Sync local state with props when dialog opens
    React.useEffect(() => {
        if (open) {
            setLocalFilters({ ...tempFilters });
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
            setLocalLogicalOperator(logicalOperator || 'AND');
        }
    }, [open, tempFilters, sortBy, sortOrder, logicalOperator]);

    // Enable Apply if any filter, sort, or logical operator value has changed
    const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
    const filtersChanged =
        localFilters.name !== (tempFilters.name || '') ||
        localFilters.offspringType !== (tempFilters.offspringType || '') ||
        localFilters.isOccupied !== (tempFilters.isOccupied ?? null);
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
                        label="Name"
                        value={localFilters.name}
                        onChange={(e) => setLocalFilters({ ...localFilters, name: e.target.value })}
                        placeholder="Filter by cage name..."
                        InputProps={{
                            endAdornment: localFilters.name && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, name: '' }); onClearName(); }} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Offspring Type</InputLabel>
                        <Select
                            value={localFilters.offspringType}
                            onChange={(e) => setLocalFilters({ ...localFilters, offspringType: e.target.value })}
                            label="Offspring Type"
                            endAdornment={localFilters.offspringType && (
                                <IconButton onClick={() => { setLocalFilters({ ...localFilters, offspringType: '' }); onClearOffspringType(); }} size="small" sx={{ mr: 2 }}>
                                    <Clear />
                                </IconButton>
                            )}
                        >
                            <MenuItem value="">All</MenuItem>
                            {offspringTypeOptions.map((type) => (
                                <MenuItem key={type} value={type}>
                                    {type}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    {sortBy && onSortByChange && sortOrder && onSortOrderChange && (
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
                                    checked={localFilters.isOccupied === true}
                                    indeterminate={localFilters.isOccupied === null}
                                    onChange={() => {
                                        if (localFilters.isOccupied === null) {
                                            setLocalFilters({ ...localFilters, isOccupied: true });
                                        } else if (localFilters.isOccupied === true) {
                                            setLocalFilters({ ...localFilters, isOccupied: false });
                                        } else {
                                            setLocalFilters({ ...localFilters, isOccupied: null });
                                        }
                                    }}
                                />
                            }
                            label="Is Occupied?"
                        />
                        {localFilters.isOccupied !== null && (
                            <IconButton onClick={() => { setLocalFilters({ ...localFilters, isOccupied: null }); onClearOccupied(); }} size="small">
                                <Clear />
                            </IconButton>
                        )}
                    </Box>
                    
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

export default CageFilterDialog;
