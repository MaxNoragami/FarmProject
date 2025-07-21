export enum OrderStatus {
  Processing = 0,
  Completed = 1,
  Failed = 2,
}

export const orderStatusLabels: Record<OrderStatus, string> = {
  [OrderStatus.Processing]: "Processing",
  [OrderStatus.Completed]: "Completed",
  [OrderStatus.Failed]: "Failed",
};

export const orderStatusOptions = [
  { value: "0", label: "Processing" },
  { value: "1", label: "Completed" },
  { value: "2", label: "Failed" },
];

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
    case OrderStatus.Processing:
      return "info";
    case OrderStatus.Completed:
      return "success";
    case OrderStatus.Failed:
      return "error";
    default:
      return "default";
  }
};
