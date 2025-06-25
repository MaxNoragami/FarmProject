import React from 'react';
import { Box, Card, CardContent, Typography } from '@mui/material';

interface AuthFormBaseProps {
  title: string;
  children: React.ReactNode;
  maxWidth?: number | string;
}

const AuthFormBase: React.FC<AuthFormBaseProps> = ({ title, children, maxWidth = 440 }) => {
  return (
    <Box sx={{ 
      width: '100vw',
      height: { xs: '100dvh', sm: '100vh' },
      backgroundColor: '#f5f5f5',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      position: 'fixed',
      margin: 0,
      padding: 0,
      left: 0,
      top: 0,
      zIndex: 9999,
      boxSizing: 'border-box',
      overflow: 'hidden',
      transform: 'translateY(0)',
    }}>
      <Card sx={{ 
        width: '100%',
        maxWidth,
        boxShadow: { xs: 1, sm: 3 },
        borderRadius: { xs: 2, sm: 3 },
        mx: { xs: 3, sm: 'auto' },
        my: { xs: 0, sm: 'auto' },
        top: 0,
        bottom: 0,
        left: 0,
        right: 0,
        position: 'relative',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
      }}>
        <CardContent sx={{ p: { xs: 4, sm: 4 }, '&:last-child': { pb: { xs: 4, sm: 4 } } }}>
          <Box sx={{ textAlign: 'center', mb: { xs: 3, sm: 4 } }}>
            <Typography 
              variant={{ xs: "h5", sm: "h4" } as any}
              component="h1"
              fontWeight="bold"
              color="primary"
              sx={{ fontSize: { xs: '1.5rem', sm: '2rem' }, lineHeight: 1.2 }}
            >
              {title}
            </Typography>
          </Box>
          {children}
        </CardContent>
      </Card>
    </Box>
  );
};

export default AuthFormBase;
