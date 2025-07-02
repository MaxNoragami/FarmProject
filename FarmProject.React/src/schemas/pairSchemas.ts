import { z } from "zod";

export const addPairSchema = z.object({
  maleRabbitId: z.number().positive("Please enter a valid male rabbit ID"),
  femaleRabbitId: z.number().positive("Please select a female rabbit"),
});

export const updatePairStatusSchema = z.object({
  pairingStatus: z.number().min(1).max(2, "Please select a valid status"),
});

export type AddPairFormFields = z.infer<typeof addPairSchema>;
export type UpdatePairStatusFormFields = z.infer<typeof updatePairStatusSchema>;
