import { useState, useEffect, useCallback } from 'react';
import { PairService } from '../api/services/pairService';
import { mapApiPairsToUI, type PairData } from '../utils/pairMappers';

interface UsePairDataResult {
  pairs: PairData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  refetch: () => Promise<void>;
}

interface UsePairDataOptions {
  pageIndex: number;
  pageSize: number;
  filters?: {
    pairingStatus?: number;
    femaleRabbitId?: number;
    maleRabbitId?: number;
  };
  logicalOperator?: number;
  sort?: string;
}

export const usePairData = ({
  pageIndex,
  pageSize,
  filters = {},
  logicalOperator = 0,
  sort = '',
}: UsePairDataOptions): UsePairDataResult => {
  const [pairs, setPairs] = useState<PairData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);

  const fetchPairs = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await PairService.getPairs({
        pageIndex: pageIndex + 1,
        pageSize,
        logicalOperator,
        ...filters,
        ...(sort ? { sort } : {}),
      });

      const uiPairs = mapApiPairsToUI(response.items);
      setPairs(uiPairs);
      setTotalPages(response.totalPages);
      setTotalCount(response.totalPages * pageSize);
      setHasNextPage(response.hasNextPage);
      setHasPreviousPage(response.hasPreviousPage);
    } catch (err) {
      console.error('Error fetching pairs:', err);
      setError('Failed to load pairs. Please try again.');
      setPairs([]);
      setTotalCount(0);
      setTotalPages(0);
      setHasNextPage(false);
      setHasPreviousPage(false);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize, filters, logicalOperator, sort]);

  useEffect(() => {
    fetchPairs();
  }, [fetchPairs]);

  return {
    pairs,
    loading,
    error,
    totalCount,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    refetch: fetchPairs,
  };
};
