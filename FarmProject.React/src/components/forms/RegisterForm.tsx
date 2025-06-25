import React from 'react';
import { Box, TextField, Button, FormControl, InputLabel } from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { registerSchema, type RegisterFormFields, userRoleOptions, type UserRole } from '../../schemas/registerSchemas';

interface RegisterFormProps {
  onSubmit: (data: RegisterFormFields) => Promise<void>;
  isSubmitting?: boolean;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onSubmit, isSubmitting }) => {
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<RegisterFormFields>({
    resolver: zodResolver(registerSchema),
    defaultValues: { role: userRoleOptions[0] as UserRole }
  });

  return (
    <Box 
      component="form" 
      onSubmit={handleSubmit(onSubmit)} 
      sx={{ display: 'flex', flexDirection: 'column', gap: { xs: 2.5, sm: 3 } }}
    >
      <Box sx={{ display: 'flex', gap: 2 }}>
        <TextField
          {...register('firstName')}
          label="First Name"
          variant="outlined"
          fullWidth
          error={!!errors.firstName}
          helperText={errors.firstName?.message}
          placeholder="Enter your first name"
          size="medium"
          sx={{
            '& .MuiInputBase-root': {
              fontSize: { xs: '1rem', sm: '1.1rem' }
            }
          }}
        />
        <TextField
          {...register('lastName')}
          label="Last Name"
          variant="outlined"
          fullWidth
          error={!!errors.lastName}
          helperText={errors.lastName?.message}
          placeholder="Enter your last name"
          size="medium"
          sx={{
            '& .MuiInputBase-root': {
              fontSize: { xs: '1rem', sm: '1.1rem' }
            }
          }}
        />
      </Box>

      <TextField
        {...register('email')}
        label="Email"
        variant="outlined"
        fullWidth
        type="email"
        error={!!errors.email}
        helperText={errors.email?.message}
        placeholder="Enter your email address"
        size="medium"
        sx={{
          '& .MuiInputBase-root': {
            fontSize: { xs: '1rem', sm: '1.1rem' }
          }
        }}
      />

      <TextField
        {...register('password')}
        label="Password"
        variant="outlined"
        fullWidth
        type="password"
        error={!!errors.password}
        helperText={errors.password?.message}
        placeholder="Enter your password"
        size="medium"
        sx={{
          '& .MuiInputBase-root': {
            fontSize: { xs: '1rem', sm: '1.1rem' }
          }
        }}
      />

      <FormControl fullWidth error={!!errors.role}>
        <InputLabel shrink htmlFor="role-native">Role</InputLabel>
        <Controller
          name="role"
          control={control}
          render={({ field }) => (
            <TextField
              select
              SelectProps={{
                native: true,
              }}
              label="Role"
              id="role-native"
              {...field}
              value={field.value as string}
              error={!!errors.role}
              helperText={errors.role?.message}
              size="medium"
              sx={{
                mt: 0,
                '& .MuiInputBase-root': {
                  fontSize: { xs: '1rem', sm: '1.1rem' }
                }
              }}
            >
              {userRoleOptions.map((role) => (
                <option key={role} value={role}>{role}</option>
              ))}
            </TextField>
          )}
        />
        {errors.role && (
          <Box sx={{ color: 'error.main', fontSize: 13, mt: 0.5, ml: 2 }}>
            {errors.role.message}
          </Box>
        )}
      </FormControl>

      <Button
        type="submit"
        variant="contained"
        size="large"
        fullWidth
        disabled={isSubmitting}
        sx={{ 
          mt: { xs: 1, sm: 2 },
          py: { xs: 1.5, sm: 2 },
          fontSize: { xs: '1rem', sm: '1.1rem' },
          fontWeight: 600,
          borderRadius: { xs: 1.5, sm: 2 }
        }}
      >
        {isSubmitting ? 'Registering...' : 'Register'}
      </Button>
    </Box>
  );
};

export default RegisterForm;
