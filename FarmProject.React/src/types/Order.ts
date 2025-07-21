export interface Order {
  id: number;
  customerId: number;
  orderStatus: number;
  orderDate: string;
}

export interface OrderResponse {
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  items: Order[];
}
