import { useState, useEffect, useCallback } from 'react';
import { RabbitService } from '../api/services/rabbitService';
import { mapApiRabbitsToUI, type RabbitData } from '../utils/rabbitMappers';

interface UseRabbitDataResult {
  rabbits: RabbitData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  refetch: () => Promise<void>;
}

interface UseRabbitDataOptions {
  pageIndex: number;
  pageSize: number;
  filters?: {
    name?: string;
    breedingStatus?: number;
    cageId?: number;
  };
  logicalOperator?: number;
  sort?: string;
}

export const useRabbitData = ({
  pageIndex,
  pageSize,
  filters = {},
  logicalOperator = 0,
  sort = '',
}: UseRabbitDataOptions): UseRabbitDataResult => {
  const [rabbits, setRabbits] = useState<RabbitData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);

  const fetchRabbits = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await RabbitService.getRabbits({
        pageIndex: pageIndex + 1,
        pageSize,
        logicalOperator,
        ...filters,
        ...(sort ? { sort } : {}),
      });

      const uiRabbits = mapApiRabbitsToUI(response.items);
      setRabbits(uiRabbits);
      setTotalPages(response.totalPages);
      setTotalCount(response.totalPages * pageSize);
      setHasNextPage(response.hasNextPage);
      setHasPreviousPage(response.hasPreviousPage);
    } catch (err) {
      console.error('Error fetching rabbits:', err);
      setError('Failed to load rabbits. Please try again.');
      setRabbits([]);
      setTotalCount(0);
      setTotalPages(0);
      setHasNextPage(false);
      setHasPreviousPage(false);
    } finally {
      setLoading(false);
    }
  }, [pageIndex, pageSize, filters, logicalOperator, sort]);

  useEffect(() => {
    fetchRabbits();
  }, [fetchRabbits]);

  return {
    rabbits,
    loading,
    error,
    totalCount,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    refetch: fetchRabbits,
  };
};

