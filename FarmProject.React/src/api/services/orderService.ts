import { apiClient } from "../config";
import type { OrderResponse } from "../../types/Order";
import type { OrderDetails } from "../types/Order";

export interface OrderFilters {
  customerId?: number;
  orderStatus?: number;
  orderDate?: string;
}

export interface OrderQueryParams {
  pageIndex?: number;
  pageSize?: number;
  filters?: OrderFilters;
  logicalOperator?: number;
  sort?: string;
}

export const OrderService = {
  getOrders: async (params: OrderQueryParams): Promise<OrderResponse> => {
    const queryParams = new URLSearchParams();

    if (params.pageIndex !== undefined) {
      queryParams.append("pageIndex", params.pageIndex.toString());
    }
    if (params.pageSize !== undefined) {
      queryParams.append("pageSize", params.pageSize.toString());
    }
    if (params.logicalOperator !== undefined) {
      queryParams.append("logicalOperator", params.logicalOperator.toString());
    }
    if (params.sort) {
      queryParams.append("sort", params.sort);
    }

    if (params.filters) {
      Object.entries(params.filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null && value !== "") {
          queryParams.append(key, value.toString());
        }
      });
    }

    const response = await apiClient.get<OrderResponse>(
      `/orders?${queryParams.toString()}`
    );
    return response.data;
  },

  addOrder: async (data: {
    customerId: number;
    orderRequests: Array<{ cageId: number; amount: number }>;
  }) => {
    const response = await apiClient.post("/orders", data);
    return response.data;
  },

  getOrderById: async (orderId: number): Promise<OrderDetails> => {
    const response = await apiClient.get<OrderDetails>(`/orders/${orderId}`);
    return response.data;
  },
};
