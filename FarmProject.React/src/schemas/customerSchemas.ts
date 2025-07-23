import { z } from "zod";

export const addCustomerSchema = z.object({
  firstName: z
    .string()
    .min(1, "First name cannot be empty")
    .min(3, "First name must not be shorter than 3 characters")
    .max(64, "First name must not be longer than 64 characters"),
  lastName: z
    .string()
    .min(1, "Last name cannot be empty")
    .min(3, "Last name must not be shorter than 3 characters")
    .max(64, "Last name must not be longer than 64 characters"),
  email: z
    .string()
    .min(1, "Email cannot be empty")
    .min(3, "Email must not be shorter than 3 characters")
    .max(64, "Email must not be longer than 64 characters")
    .email("Email format is invalid"),
  phoneNum: z
    .string()
    .min(1, "Phone number cannot be empty")
    .min(3, "Phone number must not be shorter than 3 digits")
    .max(16, "Phone number must not be longer than 16 characters")
    .regex(/^\+?[\d\s\-\(\)]*$/, "Phone number format is invalid"),
});

export type AddCustomerFormFields = z.infer<typeof addCustomerSchema>;
