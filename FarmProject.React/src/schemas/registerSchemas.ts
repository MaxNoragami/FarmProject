import { z } from "zod";

export const userRoleOptions = ["Logistics", "Worker"] as const;
export type UserRole = (typeof userRoleOptions)[number];

export const registerSchema = z.object({
  firstName: z.string()
    .min(1, 'First name is required')
    .max(48, 'First name max length is 48 characters'),
  lastName: z.string()
    .min(1, 'Last name is required')
    .max(48, 'Last name max length is 48 characters'),
  email: z.string()
    .email('Please enter a valid email address')
    .max(128, 'Email address max length is 128 characters'),
  password: z.string()
    .min(8, 'Password must contain at least 8 chars')
    .max(48, 'Password max length is 48 characters')
    .regex(/[0-9]+/, 'Password must contain at least one digit')
    .regex(/[a-z]+/, 'Password must contain at least one lowercase')
    .regex(/[A-Z]+/, 'Password must contain at least one uppercase'),
  role: z.custom<UserRole>((val): val is UserRole => userRoleOptions.includes(val as UserRole), {
    message: "Role is required"
  }),
});

export type RegisterFormFields = z.infer<typeof registerSchema>;
