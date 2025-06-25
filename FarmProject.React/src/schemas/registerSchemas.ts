import { z } from "zod";

export const userRoleOptions = ["Logistics", "Worker"] as const;
export type UserRole = (typeof userRoleOptions)[number];

export const registerSchema = z.object({
  firstName: z.string().min(1, 'First name is required').max(50, 'First name must be less than 50 characters'),
  lastName: z.string().min(1, 'Last name is required').max(50, 'Last name must be less than 50 characters'),
  email: z.string().email('Please enter a valid email address'),
  password: z.string()
    .min(8, 'Password must be at least 8 characters long')
    .regex(/[0-9]/, 'Password must contain at least one digit')
    .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
    .regex(/[A-Z]/, 'Password must contain at least one uppercase letter'),
  role: z.custom<UserRole>((val): val is UserRole => userRoleOptions.includes(val as UserRole), {
    message: "Role is required"
  }),
});

export type RegisterFormFields = z.infer<typeof registerSchema>;
