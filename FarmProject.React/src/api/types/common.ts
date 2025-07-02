export interface PaginatedRequest {
  pageIndex: number;
  pageSize: number;
  logicalOperator?: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

