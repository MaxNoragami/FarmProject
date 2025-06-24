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
    onApply: () => void;
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
                        value={tempFilters.name}
                        onChange={(e) => onTempFiltersChange({ ...tempFilters, name: e.target.value })}
                        placeholder="Filter by cage name..."
                        InputProps={{
                            endAdornment: tempFilters.name && (
                                <IconButton onClick={onClearName} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Offspring Type</InputLabel>
                        <Select
                            value={tempFilters.offspringType}
                            onChange={(e) => onTempFiltersChange({ ...tempFilters, offspringType: e.target.value })}
                            label="Offspring Type"
                            endAdornment={tempFilters.offspringType && (
                                <IconButton onClick={onClearOffspringType} size="small" sx={{ mr: 2 }}>
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
                                    checked={tempFilters.isOccupied === true}
                                    indeterminate={tempFilters.isOccupied === null}
                                    onChange={(e) => {
                                        if (tempFilters.isOccupied === null) {
                                            onTempFiltersChange({ ...tempFilters, isOccupied: true });
                                        } else if (tempFilters.isOccupied === true) {
                                            onTempFiltersChange({ ...tempFilters, isOccupied: false });
                                        } else {
                                            onTempFiltersChange({ ...tempFilters, isOccupied: null });
                                        }
                                    }}
                                />
                            }
                            label="Is Occupied?"
                        />
                        {tempFilters.isOccupied !== null && (
                            <IconButton onClick={onClearOccupied} size="small">
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
                            disabled={!tempFilters.name.trim() && !tempFilters.offspringType.trim() && tempFilters.isOccupied === null}
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
