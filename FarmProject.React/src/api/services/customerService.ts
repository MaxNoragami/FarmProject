import { apiClient } from "../config";
import type {
  CustomerResponse,
  Customer,
  CustomerWithOrders,
} from "../../types/Customer";

export interface CustomerFilters {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNum?: string;
}

export interface CustomerQueryParams {
  pageIndex?: number;
  pageSize?: number;
  filters?: CustomerFilters;
  logicalOperator?: number;
  sort?: string;
}

export const CustomerService = {
  getCustomers: async (
    params: CustomerQueryParams
  ): Promise<CustomerResponse> => {
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

    const response = await apiClient.get<CustomerResponse>(
      `/customers?${queryParams.toString()}`
    );
    return response.data;
  },

  getCustomerById: async (customerId: number): Promise<CustomerWithOrders> => {
    const response = await apiClient.get<CustomerWithOrders>(
      `/customers/${customerId}`
    );
    return response.data;
  },

  addCustomer: async (customerData: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNum: string;
  }): Promise<Customer> => {
    const response = await apiClient.post<Customer>("/customers", customerData);
    return response.data;
  },
};
