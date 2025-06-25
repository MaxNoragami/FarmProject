import { z } from "zod";

export const addCageSchema = z.object({
  name: z
    .string()
    .min(3, "Cage name must not be shorter than 3 characters")
    .max(256, "Cage name must not be longer than 256 characters"),
});

export type AddCageFormFields = z.infer<typeof addCageSchema>;
