import { useState, useEffect, useCallback } from 'react';
import { CageService } from '../services/CageService';
import { type CageData } from '../data/mockCageData'; // Or the correct path

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
}

export const useAvailableCages = ({
  pageSize,
  initialPage = 1
}: UseAvailableCagesOptions): UseAvailableCagesResult => {
  const [cages, setCages] = useState<CageData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageIndex, setPageIndexInternal] = useState(initialPage);

  // Create a wrapper function that can accept either a direct value or a function
  const setPageIndex = useCallback((newPageOrUpdater: number | ((prevPage: number) => number)) => {
    if (typeof newPageOrUpdater === 'function') {
      setPageIndexInternal(prevPage => newPageOrUpdater(prevPage));
    } else {
      setPageIndexInternal(newPageOrUpdater);
    }
  }, []);

  const fetchCages = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await CageService.getCages({
        pageIndex: pageIndex,
        pageSize,
        filters: { isOccupied: false }, // Only get available cages
        logicalOperator: 0,
      });

      setCages(response.items);
      // Calculate totalCount from available info (pageSize * totalPages)
      // Note: This is an approximation as the last page may not be full
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
  }, [pageIndex, pageSize]);

  useEffect(() => {
    fetchCages();
  }, [fetchCages]);

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
