import { z } from "zod";

export const addPairSchema = z.object({
  maleRabbitId: z
    .number()
    .positive("Male rabbit ID must be a positive number")
    .int("Male rabbit ID must be a whole number"),
  femaleRabbitId: z.number().positive("Please select a female rabbit"),
});

export type AddPairFormFields = z.infer<typeof addPairSchema>;
