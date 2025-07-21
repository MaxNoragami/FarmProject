export interface Order {
  id: number;
  customerId: number;
  orderStatus: number;
  orderDate: string;
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNum: string;
}

export interface CustomerWithOrders {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNum: string;
  orders: Order[];
}

export interface CustomerResponse {
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  items: Customer[];
}
