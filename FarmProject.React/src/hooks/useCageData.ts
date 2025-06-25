import { useState, useEffect, useCallback } from 'react';
import { CageService } from '../api/services/cageService';
import { mapApiCagesToUI } from '../utils/cageMappers';
import { type CageData } from '../data/mockCageData';

interface UseCageDataResult {
  cages: CageData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  refetch: () => Promise<void>;
}

interface UseCageDataOptions {
  pageIndex: number;
  pageSize: number;
}

export const useCageData = ({ pageIndex, pageSize }: UseCageDataOptions): UseCageDataResult => {
  const [cages, setCages] = useState<CageData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);

  const fetchCages = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await CageService.getCages({
        pageIndex: pageIndex + 1,
        pageSize,
        logicalOperator: 0,
      });

      const uiCages = mapApiCagesToUI(response.items);
      setCages(uiCages);
      setTotalPages(response.totalPages);
      setTotalCount(response.totalPages * pageSize);
      setHasNextPage(response.hasNextPage);
      setHasPreviousPage(response.hasPreviousPage);
    } catch (err) {
      console.error('Error fetching cages:', err);
      setError('Failed to load cages. Please try again.');
      setCages([]);
      setTotalCount(0);
      setTotalPages(0);
      setHasNextPage(false);
      setHasPreviousPage(false);
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
    hasNextPage,
    hasPreviousPage,
    refetch: fetchCages,
  };
};
