import { useState, useEffect, useCallback } from "react";
import {
  OrderService,
  type OrderQueryParams,
} from "../api/services/orderService";
import { mapOrdersToData, type OrderData } from "../utils/orderMappers";

interface UseOrderDataOptions extends OrderQueryParams {}

interface UseOrderDataReturn {
  orders: OrderData[];
  loading: boolean;
  error: string | null;
  totalCount: number;
  refetch: () => Promise<void>;
}

export const useOrderData = (
  options: UseOrderDataOptions
): UseOrderDataReturn => {
  const [orders, setOrders] = useState<OrderData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);

  const fetchOrders = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await OrderService.getOrders({
        ...options,
        pageIndex: (options.pageIndex || 0) + 1,
      });
      const mappedOrders = mapOrdersToData(response.items);
      setOrders(mappedOrders);
      setTotalCount(response.totalPages * (options.pageSize || 10));
    } catch (err: any) {
      setError(
        err?.response?.data?.message || err?.message || "Failed to fetch orders"
      );
      setOrders([]);
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
    fetchOrders();
  }, [fetchOrders]);

  return {
    orders,
    loading,
    error,
    totalCount,
    refetch: fetchOrders,
  };
};
