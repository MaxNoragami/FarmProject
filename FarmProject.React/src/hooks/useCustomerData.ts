import { useState, useEffect, useCallback } from "react";
import {
  CustomerService,
  type CustomerQueryParams,
} from "../api/services/customerService";
import {
  mapCustomersToData,
  type CustomerData,
} from "../utils/customerMappers";

interface UseCustomerDataOptions extends CustomerQueryParams {}

interface UseCustomerDataReturn {
  customers: CustomerData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  refetch: () => Promise<void>;
}

export const useCustomerData = (
  options: UseCustomerDataOptions
): UseCustomerDataReturn => {
  const [customers, setCustomers] = useState<CustomerData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);

  const fetchCustomers = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await CustomerService.getCustomers({
        ...options,
        pageIndex: (options.pageIndex || 0) + 1,
      });
      const mappedCustomers = mapCustomersToData(response.items);
      setCustomers(mappedCustomers);
      setTotalCount(response.totalPages * (options.pageSize || 10));
    } catch (err: any) {
      setError(
        err?.response?.data?.message ||
          err?.message ||
          "Failed to fetch customers"
      );
      setCustomers([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [
    options.pageIndex,
    options.pageSize,
    options.filters,
    options.logicalOperator,
    options.sort,
  ]);

  useEffect(() => {
    fetchCustomers();
  }, [fetchCustomers]);

  return {
    customers,
    loading,
    error,
    totalCount,
    refetch: fetchCustomers,
  };
};
