import { useState, useEffect, useCallback } from 'react';
import { CageService } from '../api/services/cageService';
import { mapApiCagesToUI, type CageData } from '../utils/cageMappers';
import { OffspringType } from '../types/OffspringType';

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
  filters?: {
    name?: string;
    offspringType?: number;
    isOccupied?: boolean;
  };
  logicalOperator?: number;
  sort?: string;
}

function normalizeSort(sort?: string): string | undefined {
  if (!sort) return undefined;
  const [field, order] = sort.split(':');
  let normalizedField = field;
  if (field === 'cageId') normalizedField = 'id';
  if (normalizedField !== 'id' && normalizedField !== 'name') return undefined;
  return `${normalizedField}:${order || 'asc'}`;
}

export const useCageData = ({
  pageIndex,
  pageSize,
  filters = {},
  logicalOperator = 0,
  sort = '',
}: UseCageDataOptions): UseCageDataResult => {
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
      const normalizedSort = normalizeSort(sort);
      const response = await CageService.getCages({
        pageIndex: pageIndex + 1,
        pageSize,
        logicalOperator,
        ...filters,
        ...(normalizedSort ? { sort: normalizedSort } : {}),
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
  }, [pageIndex, pageSize, filters, logicalOperator, sort]);

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
}
