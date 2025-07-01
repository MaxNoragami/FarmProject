import { Dialog, DialogTitle, DialogContent, TextField, Select, MenuItem, FormControl, InputLabel, Box, Button, IconButton } from '@mui/material';
import { Close, Clear } from '@mui/icons-material';
import * as React from 'react';
import { pairingStatusOptions } from '../../types/PairingStatus';

interface PairFilterDialogProps {
    open: boolean;
    onClose: () => void;
    tempFilters: {
        status: string;
        femaleRabbitId: string;
        maleRabbitId: string;
    };
    onTempFiltersChange: (filters: { status: string; femaleRabbitId: string; maleRabbitId: string }) => void;
    onClearStatus: () => void;
    onClearFemaleRabbitId: () => void;
    onClearMaleRabbitId: () => void;
    logicalOperator: 'AND' | 'OR';
    onLogicalOperatorChange: (operator: 'AND' | 'OR') => void;
    onApply: (params: { filters: { status: string; femaleRabbitId: string; maleRabbitId: string }, sortBy: string, sortOrder: 'asc' | 'desc', logicalOperator: 'AND' | 'OR' }) => void;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
    sortableColumns?: Array<{ id: string; label: string }>;
}

const PairFilterDialog: React.FC<PairFilterDialogProps> = ({
    open,
    onClose,
    tempFilters,
    logicalOperator,
    onApply,
    sortBy,
    sortOrder,
    sortableColumns = []
}) => {
    const [status, setStatus] = React.useState('');
    const [femaleRabbitId, setFemaleRabbitId] = React.useState('');
    const [maleRabbitId, setMaleRabbitId] = React.useState('');
    const [localSortBy, setLocalSortBy] = React.useState('');
    const [localSortOrder, setLocalSortOrder] = React.useState<'asc' | 'desc'>('asc');
    const [localLogicalOperator, setLocalLogicalOperator] = React.useState<'AND' | 'OR'>('AND');
    const [errors, setErrors] = React.useState<{ femaleRabbitId?: string; maleRabbitId?: string }>({});

    React.useEffect(() => {
        if (open) {
            setStatus(tempFilters.status);
            setFemaleRabbitId(tempFilters.femaleRabbitId);
            setMaleRabbitId(tempFilters.maleRabbitId);
            setLocalSortBy(sortBy || '');
            setLocalSortOrder(sortOrder || 'asc');
            setLocalLogicalOperator(logicalOperator);
        }
    }, [open, tempFilters, sortBy, sortOrder, logicalOperator]);

    const hasChanges = React.useMemo(() => {
        const filtersChanged = status !== tempFilters.status || 
                              femaleRabbitId !== tempFilters.femaleRabbitId || 
                              maleRabbitId !== tempFilters.maleRabbitId;
        const sortChanged = localSortBy !== (sortBy || '') || localSortOrder !== (sortOrder || 'asc');
        const operatorChanged = localLogicalOperator !== logicalOperator;
        
        return filtersChanged || sortChanged || operatorChanged;
    }, [status, femaleRabbitId, maleRabbitId, localSortBy, localSortOrder, localLogicalOperator, tempFilters, sortBy, sortOrder, logicalOperator]);

    const validateNumericField = (value: string): string | null => {
        if (!value.trim()) return null; // Empty is valid
        
        const num = Number(value);
        if (isNaN(num) || !Number.isInteger(num) || num <= 0) {
            return `Nums only`;
        }
        return null;
    };

    const handleApply = () => {
        const newErrors: { femaleRabbitId?: string; maleRabbitId?: string } = {};
        
        // Validate female rabbit ID
        const femaleError = validateNumericField(femaleRabbitId);
        if (femaleError) {
            newErrors.femaleRabbitId = femaleError;
        }
        
        // Validate male rabbit ID
        const maleError = validateNumericField(maleRabbitId);
        if (maleError) {
            newErrors.maleRabbitId = maleError;
        }
        
        setErrors(newErrors);
        
        // Only proceed if no errors
        if (Object.keys(newErrors).length === 0) {
            onApply({
                filters: { status, femaleRabbitId, maleRabbitId },
                sortBy: localSortBy,
                sortOrder: localSortOrder,
                logicalOperator: localLogicalOperator
            });
        }
    };

    // Clear errors when values change
    const handleFemaleRabbitIdChange = (value: string) => {
        setFemaleRabbitId(value);
        if (errors.femaleRabbitId) {
            setErrors(prev => ({ ...prev, femaleRabbitId: undefined }));
        }
    };

    const handleMaleRabbitIdChange = (value: string) => {
        setMaleRabbitId(value);
        if (errors.maleRabbitId) {
            setErrors(prev => ({ ...prev, maleRabbitId: undefined }));
        }
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
                    {/* Female and Male Rabbit ID fields side by side */}
                    <Box sx={{ display: 'flex', gap: 2 }}>
                        <TextField
                            label="Female Rabbit ID"
                            variant="outlined"
                            fullWidth
                            value={femaleRabbitId}
                            onChange={(e) => handleFemaleRabbitIdChange(e.target.value)}
                            error={!!errors.femaleRabbitId}
                            helperText={errors.femaleRabbitId}
                            slotProps={{
                                input: {
                                    endAdornment: femaleRabbitId && (
                                        <IconButton onClick={() => {
                                            setFemaleRabbitId('');
                                            setErrors(prev => ({ ...prev, femaleRabbitId: undefined }));
                                        }} size="small">
                                            <Clear />
                                        </IconButton>
                                    )
                                }
                            }}
                        />
                        <TextField
                            label="Male Rabbit ID"
                            variant="outlined"
                            fullWidth
                            value={maleRabbitId}
                            onChange={(e) => handleMaleRabbitIdChange(e.target.value)}
                            error={!!errors.maleRabbitId}
                            helperText={errors.maleRabbitId}
                            slotProps={{
                                input: {
                                    endAdornment: maleRabbitId && (
                                        <IconButton onClick={() => {
                                            setMaleRabbitId('');
                                            setErrors(prev => ({ ...prev, maleRabbitId: undefined }));
                                        }} size="small">
                                            <Clear />
                                        </IconButton>
                                    )
                                }
                            }}
                        />
                    </Box>
                    
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
                            {pairingStatusOptions.map((statusOption) => (
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

export default PairFilterDialog;
