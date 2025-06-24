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
    onApply: () => void;
    sortBy?: string;
    onSortByChange?: (sortBy: string) => void;
    sortOrder?: 'asc' | 'desc';
    onSortOrderChange?: (order: 'asc' | 'desc') => void;
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
                        label="Pair ID"
                        value={tempFilters.pairId}
                        onChange={(e) => onTempFiltersChange({ ...tempFilters, pairId: e.target.value })}
                        placeholder="Filter by pair ID..."
                        InputProps={{
                            endAdornment: tempFilters.pairId && (
                                <IconButton onClick={onClearPairId} size="small">
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
                            {pairingStatusOptions.map((status) => (
                                <MenuItem key={status} value={status}>
                                    {status}
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
                            disabled={!tempFilters.pairId.trim() && !tempFilters.status.trim()}
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
