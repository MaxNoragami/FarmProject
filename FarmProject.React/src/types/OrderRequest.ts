export interface OrderRequest {
  id: number;
  orderId: number;
  offspringType: number;
  amount: number;
  sacrificedAmount: number;
  cageId: number;
  orderRequestStatus: number;
}

export enum OrderRequestStatus {
  Waiting = 0,
  Completed = 1,
  Failed = 2,
}

export const orderRequestStatusLabels: Record<OrderRequestStatus, string> = {
  [OrderRequestStatus.Waiting]: "Waiting",
  [OrderRequestStatus.Completed]: "Completed",
  [OrderRequestStatus.Failed]: "Failed",
};

export const getOrderRequestStatusLabel = (status: number): string => {
  return orderRequestStatusLabels[status as OrderRequestStatus] || "Unknown";
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
