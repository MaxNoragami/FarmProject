import { z } from "zod";

export const loginSchema = z.object({
  email: z.string()
    .email('Please enter a valid email address')
    .max(128, 'Email address max length is 128 characters'),
  password: z.string()
    .min(1, 'Password is required')
    .max(48, 'Password max length is 48 characters'),
});

export type LoginFormFields = z.infer<typeof loginSchema>;
