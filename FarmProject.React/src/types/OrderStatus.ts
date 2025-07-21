export enum OrderStatus {
  Pending = 0,
  Processing = 1,
  Shipped = 2,
  Delivered = 3,
  Cancelled = 4,
}

export const orderStatusLabels: Record<OrderStatus, string> = {
  [OrderStatus.Pending]: "Pending",
  [OrderStatus.Processing]: "Processing",
  [OrderStatus.Shipped]: "Shipped",
  [OrderStatus.Delivered]: "Delivered",
  [OrderStatus.Cancelled]: "Cancelled",
};

export const getOrderStatusLabel = (status: number): string => {
  return orderStatusLabels[status as OrderStatus] || "Unknown";
};

export const getOrderStatusColor = (
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
    case OrderStatus.Pending:
      return "warning";
    case OrderStatus.Processing:
      return "info";
    case OrderStatus.Shipped:
      return "primary";
    case OrderStatus.Delivered:
      return "success";
    case OrderStatus.Cancelled:
      return "error";
    default:
      return "default";
  }
};
