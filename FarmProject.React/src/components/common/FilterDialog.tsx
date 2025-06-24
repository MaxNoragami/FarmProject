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
    onApply: () => void;
    isMobile?: boolean;
    sortBy?: string;
    onSortByChange?: (sortBy: string) => void;
    sortOrder?: 'asc' | 'desc';
    onSortOrderChange?: (order: 'asc' | 'desc') => void;
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
    onSortByChange,
    sortOrder,
    onSortOrderChange,
    sortableColumns = []
}) => {
    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                Filter Options
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
                        placeholder="Filter by name..."
                        InputProps={{
                            endAdornment: tempFilters.name && (
                                <IconButton onClick={onClearName} size="small">
                                    <Clear />
                                </IconButton>
                            )
                        }}
                    />
                    
                    <FormControl fullWidth>
                        <InputLabel>Status</InputLabel>
                        <Select
                            value={tempFilters.status}
                            onChange={(e) => onTempFiltersChange({ ...tempFilters, status: e.target.value })}
                            label="Status"
                            endAdornment={tempFilters.status && (
                                <IconButton onClick={onClearStatus} size="small" sx={{ mr: 2 }}>
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

                    {isMobile && sortBy && onSortByChange && sortOrder && onSortOrderChange && (
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
                            disabled={!tempFilters.name.trim() && !tempFilters.status.trim()}
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
