import { useState, useEffect, useCallback } from 'react';
import { RabbitService } from '../api/services/rabbitService';
import { mapApiRabbitsToUI, type RabbitData } from '../utils/rabbitMappers';

interface UseAvailableFemaleRabbitsResult {
  rabbits: RabbitData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  pageIndex: number;
  setPageIndex: (page: number) => void;
  refetch: () => Promise<void>;
}

interface UseAvailableFemaleRabbitsOptions {
  pageSize: number;
  initialPage?: number;
}

export const useAvailableFemaleRabbits = ({
  pageSize,
  initialPage = 1
}: UseAvailableFemaleRabbitsOptions): UseAvailableFemaleRabbitsResult => {
  const [rabbits, setRabbits] = useState<RabbitData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [pageIndex, setPageIndexInternal] = useState(initialPage);

  const setPageIndex = useCallback((newPageOrUpdater: number | ((prevPage: number) => number)) => {
    if (typeof newPageOrUpdater === 'function') {
      setPageIndexInternal(prevPage => newPageOrUpdater(prevPage));
    } else {
      setPageIndexInternal(newPageOrUpdater);
    }
  }, []);

  const fetchRabbits = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await RabbitService.getRabbits({
        pageIndex: pageIndex,
        pageSize,
        breedingStatus: 0,
        logicalOperator: 0,
      });

      const uiRabbits = mapApiRabbitsToUI(response.items);
      setRabbits(uiRabbits);
      setTotalCount(response.totalPages * pageSize);
      setTotalPages(response.totalPages);
    } catch (err) {
      console.error('Error fetching available female rabbits:', err);
      setError('Failed to load available female rabbits. Please try again.');
      setRabbits([]);
      setTotalCount(0);
      setTotalPages(0);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize]);

  useEffect(() => {
    fetchRabbits();
  }, [fetchRabbits]);

  return {
    rabbits,
    loading,
    error,
    totalCount,
    totalPages,
    pageIndex,
    setPageIndex,
    refetch: fetchRabbits,
  };
};
