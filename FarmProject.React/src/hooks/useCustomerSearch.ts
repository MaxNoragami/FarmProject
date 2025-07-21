import { useState, useCallback } from "react";
import { CustomerService } from "../api/services/customerService";
import { type CustomerData } from "../utils/customerMappers";
import { mapApiCustomersToUI } from "../utils/customerMappers";

export type SearchField = "firstName" | "lastName" | "email" | "phoneNum";

interface UseCustomerSearchResult {
  customers: CustomerData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  totalPages: number;
  search: (
    query: string,
    field: SearchField,
    pageIndex?: number
  ) => Promise<void>;
  loadPage: (pageIndex: number) => Promise<void>;
  hasSearched: boolean;
}

interface UseCustomerSearchOptions {
  pageSize: number;
}

export const useCustomerSearch = ({
  pageSize,
}: UseCustomerSearchOptions): UseCustomerSearchResult => {
  const [customers, setCustomers] = useState<CustomerData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [hasSearched, setHasSearched] = useState(false);
  const [currentQuery, setCurrentQuery] = useState("");
  const [currentField, setCurrentField] = useState<SearchField>("email");

  const search = useCallback(
    async (query: string, field: SearchField, pageIndex: number = 1) => {
      setLoading(true);
      setError(null);
      setCurrentQuery(query);
      setCurrentField(field);

      try {
        const filters: any = {};

        if (query.trim()) {
          filters[field] = query.trim();
        }

        const response = await CustomerService.getCustomers({
          pageIndex,
          pageSize,
          filters,
          logicalOperator: 0,
        });

        const uiCustomers = mapApiCustomersToUI(response.items);
        setCustomers(uiCustomers);
        setTotalCount(response.totalPages * pageSize);
        setTotalPages(response.totalPages);
        setHasSearched(true);
      } catch (err: any) {
        setError(
          err?.response?.data?.message ||
            err?.message ||
            "Failed to search customers"
        );
        setCustomers([]);
        setTotalCount(0);
        setTotalPages(0);
      } finally {
        setLoading(false);
      }
    },
    [pageSize]
  );

  const loadPage = useCallback(
    async (pageIndex: number) => {
      await search(currentQuery, currentField, pageIndex);
    },
    [search, currentQuery, currentField]
  );

  return {
    customers,
    loading,
    error,
    totalCount,
    totalPages,
    search,
    loadPage,
    hasSearched,
  };
};
