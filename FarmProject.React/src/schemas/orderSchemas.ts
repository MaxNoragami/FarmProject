import { z } from "zod";

export const addOrderRequestSchema = z.object({
  cageId: z.number().min(1, "Please select a cage"),
  amount: z.number().min(1, "Amount must be at least 1"),
});

export const addOrderSchema = z.object({
  customerId: z.number().min(1, "Please select a customer"),
  orderRequests: z
    .array(addOrderRequestSchema)
    .min(1, "At least one order request is required"),
});

export type AddOrderRequestFormFields = z.infer<typeof addOrderRequestSchema>;
export type AddOrderFormFields = z.infer<typeof addOrderSchema>;

export interface OrderRequestItem {
  cageId: number;
  amount: number;
  cageName?: string;
  offspringType?: number;
}
