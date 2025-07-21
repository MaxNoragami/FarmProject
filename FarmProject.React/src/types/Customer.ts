export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNum: string;
}

export interface CustomerResponse {
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  items: Customer[];
}
