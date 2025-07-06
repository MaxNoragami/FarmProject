import { useState, useEffect, useCallback } from 'react';
import { CageService } from '../services/CageService';
import { type CageData } from '../utils/cageMappers'; 

interface UseAvailableCagesResult {
  cages: CageData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  pageIndex: number;
  setPageIndex: (page: number) => void;
  refetch: () => Promise<void>;
}

interface UseAvailableCagesOptions {
  pageSize: number;
  initialPage?: number;
  enabled?: boolean; // Add this new prop
}

export const useAvailableCages = ({
  pageSize,
  initialPage = 1,
  enabled = true // Default to true for backward compatibility
}: UseAvailableCagesOptions): UseAvailableCagesResult => {
  const [cages, setCages] = useState<CageData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageIndex, setPageIndexInternal] = useState(initialPage);
  const [hasFetched, setHasFetched] = useState(false);

  const setPageIndex = useCallback((newPageOrUpdater: number | ((prevPage: number) => number)) => {
    if (typeof newPageOrUpdater === 'function') {
      setPageIndexInternal(prevPage => newPageOrUpdater(prevPage));
    } else {
      setPageIndexInternal(newPageOrUpdater);
    }
  }, []);

  const fetchCages = useCallback(async () => {
    // Skip fetching if not enabled
    if (!enabled) return;
    
    setLoading(true);
    setError(null);
    
    try {
      const response = await CageService.getCages({
        pageIndex: pageIndex,
        pageSize,
        filters: { isOccupied: false }, 
        logicalOperator: 0,
      });

      setCages(response.items);
      setTotalCount(response.totalPages * pageSize);
      setTotalPages(response.totalPages);
    } catch (err) {
      console.error('Error fetching available cages:', err);
      setError('Failed to load available cages. Please try again.');
      setCages([]);
      setTotalCount(0);
      setTotalPages(0);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize, enabled]); // Add enabled to dependencies

  useEffect(() => {
    // Only fetch data if enabled is true
    if (enabled) {
      fetchCages();
    }
  }, [fetchCages, enabled]); // Add enabled to dependencies

  return {
    cages,
    loading,
    error,
    totalCount,
    totalPages,
    pageIndex,
    setPageIndex,
    refetch: fetchCages,
  };
};

