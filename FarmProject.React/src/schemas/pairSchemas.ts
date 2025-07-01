import { z } from "zod";

export const addPairSchema = z.object({
  maleRabbitId: z.number().positive("Please enter a valid male rabbit ID"),
  femaleRabbitId: z.number().positive("Please select a female rabbit"),
});

export type AddPairFormFields = z.infer<typeof addPairSchema>;
