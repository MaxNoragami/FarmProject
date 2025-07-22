export interface OrderRequest {
  id: number;
  orderId: number;
  offspringType: number;
  amount: number;
  sacrificedAmount: number;
  cageId: number;
  orderRequestStatus: number;
}

export interface OrderDetails {
  id: number;
  customerId: number;
  orderStatus: number;
  orderDate: string;
  orderRequests: OrderRequest[];
}

// Replace enum with const object and type
export const OrderRequestStatus = {
  Waiting: 0,
  Completed: 1,
  Failed: 2,
} as const;

export type OrderRequestStatusType = typeof OrderRequestStatus[keyof typeof OrderRequestStatus];

export const orderRequestStatusLabels: Record<OrderRequestStatusType, string> = {
  [OrderRequestStatus.Waiting]: "Waiting",
  [OrderRequestStatus.Completed]: "Completed",
  [OrderRequestStatus.Failed]: "Failed",
};

export const getOrderRequestStatusLabel = (status: number): string => {
  return orderRequestStatusLabels[status as OrderRequestStatusType] || "Unknown";
};

export const getOrderRequestStatusColor = (
  status: number
):
  | "default"
  | "primary"
  | "secondary"
  | "error"
  | "info"
  | "success"
  | "warning" => {
  switch (status) {
    case OrderRequestStatus.Waiting:
      return "warning";
    case OrderRequestStatus.Completed:
      return "success";
    case OrderRequestStatus.Failed:
      return "error";
    default:
      return "default";
  }
};