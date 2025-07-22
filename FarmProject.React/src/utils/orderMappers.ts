import { type Order } from "../types/Order";

export interface OrderData {
  id: number;
  customerId: number;
  orderStatus: number;
  orderDate: string;
  formattedDate: string;
}

export const mapOrdersToData = (orders: Order[]): OrderData[] => {
  return orders.map((order) => ({
    id: order.id,
    customerId: order.customerId,
    orderStatus: order.orderStatus,
    orderDate: order.orderDate,
    formattedDate: new Date(order.orderDate).toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    }),
  }));
};

export const mapApiOrderToUI = (apiOrder: any): OrderData => {
  return {
    id: apiOrder.id,
    customerId: apiOrder.customerId,
    orderStatus: apiOrder.orderStatus,
    orderDate: apiOrder.orderDate,
    formattedDate: new Date(apiOrder.orderDate).toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    }),
  };
};

export const mapApiOrdersToUI = (apiOrders: any[]): OrderData[] => {
  return apiOrders.map(mapApiOrderToUI);
};
