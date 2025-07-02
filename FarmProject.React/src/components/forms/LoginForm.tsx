import React from 'react';
import { Box, TextField, Button, Typography } from '@mui/material';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, type LoginFormFields } from '../../schemas/loginSchemas';
import { Link as RouterLink } from 'react-router-dom';

interface LoginFormProps {
  onSubmit: (data: LoginFormFields) => Promise<void>;
  isSubmitting?: boolean;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSubmit, isSubmitting }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormFields>({
    resolver: zodResolver(loginSchema),
  });

  return (
    <Box 
      component="form" 
      onSubmit={handleSubmit(onSubmit)} 
      sx={{ display: 'flex', flexDirection: 'column', gap: { xs: 2.5, sm: 3 } }}
    >
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

      <Button
        type="submit"
        variant="contained"
        size="medium"
        disabled={isSubmitting}
        sx={{ 
          alignSelf: 'center',
          width: { xs: '70%', sm: '60%' },
          mt: { xs: 1, sm: 2 },
          py: { xs: 1, sm: 1.2 },
          fontSize: { xs: '0.95rem', sm: '1rem' },
          fontWeight: 600,
          borderRadius: { xs: 1.5, sm: 2 }
        }}
      >
        {isSubmitting ? 'Logging in...' : 'Login'}
      </Button>
      <Typography
        variant="body2"
        sx={{ mt: 0.5, mb: 0.5, textAlign: 'center', color: 'text.secondary', lineHeight: 1.2 }}
      >
        Don't have an account?{' '}
        <RouterLink
          to="/register"
          style={{
            textDecoration: 'none',
            color: 'var(--mui-palette-primary-main, #1976d2)',
            fontWeight: 600
          }}
        >
          Register
        </RouterLink>
      </Typography>
    </Box>
  );
};

export default LoginForm;
