import { useState, useMemo } from 'react';

export interface PairFilter {
  id: string;
  column: string;
  operator: 'contains' | 'equals' | 'startsWith';
  value: string;
}

export function usePairFilters<T extends Record<string, any>>(data: T[]) {
    const [filters, setFilters] = useState<PairFilter[]>([]);
    const [logicalOperator, setLogicalOperator] = useState<'AND' | 'OR'>('AND');

    const filteredData = useMemo(() => {
        if (filters.length === 0) return data;
        
        return data.filter(row => {
            const results = filters.map(filter => {
                const value = String(row[filter.column]).toLowerCase();
                const filterValue = filter.value.toLowerCase();
                
                switch (filter.operator) {
                    case 'contains':
                        return value.includes(filterValue);
                    case 'equals':
                        return value === filterValue;
                    case 'startsWith':
                        return value.startsWith(filterValue);
                    default:
                        return true;
                }
            });
            
            return logicalOperator === 'AND' 
                ? results.every(result => result)
                : results.some(result => result);
        });
    }, [data, filters, logicalOperator]);

    const removeFilter = (filterId: string) => {
        setFilters(filters.filter(f => f.id !== filterId));
    };

    return {
        filters,
        setFilters,
        logicalOperator,
        setLogicalOperator,
        filteredData,
        removeFilter
    };
}
