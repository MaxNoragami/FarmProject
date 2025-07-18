import { z } from "zod";

export const completeWeaningTaskSchema = z.object({
  newCageId: z
    .number({
      required_error: "New cage is required",
      invalid_type_error: "New cage must be a number",
    })
    .positive("New cage ID must be a positive number"),
});

export type CompleteWeaningTaskFormFields = z.infer<
  typeof completeWeaningTaskSchema
>;

export const completeOffspringSeparationTaskSchema = z
  .object({
    otherCageId: z
      .number({
        required_error: "Target cage is required",
        invalid_type_error: "Cage ID must be a number",
      })
      .positive("Cage ID must be a positive number")
      .optional(),
    femaleOffspringCount: z
      .number({
        required_error: "Female offspring count is required",
        invalid_type_error: "Count must be a number",
      })
      .int("Must be a whole number")
      .min(0, "Count cannot be negative"),
  })
  .refine(
    (data) => {
      return data.femaleOffspringCount === 0 || data.otherCageId !== undefined;
    },
    {
      message:
        "Target cage is required when female offspring count is greater than 0",
      path: ["otherCageId"],
    }
  );

export type CompleteOffspringSeparationTaskFormFields = z.infer<
  typeof completeOffspringSeparationTaskSchema
>;
