import { z } from "zod";

export const addRabbitSchema = z.object({
  name: z
    .string()
    .min(3, "Rabbit name must not be shorter than 3 characters")
    .max(256, "Rabbit name must not be longer than 256 characters"),
  cageId: z.number().positive("Please select a cage"),
});

export type AddRabbitFormFields = z.infer<typeof addRabbitSchema>;