import React, { useState, useRef, useEffect } from 'react';
import { Avatar, Box, Typography, Collapse, Button, Paper, Divider, Fade, ClickAwayListener } from '@mui/material';
import { Logout } from '@mui/icons-material';
import { useUser } from '../../contexts/UserContext';

const UserAvatar: React.FC = () => {
  const { user, logout } = useUser();
  const [expanded, setExpanded] = useState(false);

  if (!user) return null;

  const handleAvatarClick = () => {
    setExpanded(!expanded);
  };

  const handleClickAway = () => {
    setExpanded(false);
  };

  const handleLogout = () => {
    logout();
    setExpanded(false);
  };

  const getInitials = (firstName: string, lastName: string) => {
    return `${firstName.charAt(0)}${lastName.charAt(0)}`.toUpperCase();
  };

  const getAvatarColor = (name: string) => {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    const hue = hash % 360;
    return `hsl(${hue}, 70%, 60%)`;
  };

  const formatDisplayName = (firstName: string, lastName: string) => {
    const maxFirstNameLength = 12;
    const maxLastNameLength = 24;
    
    let displayFirstName = firstName;
    let displayLastName = lastName;
    
    if (firstName.length > maxFirstNameLength) {
      displayFirstName = `${firstName.charAt(0)}.`;
    }
    if (lastName.length > maxLastNameLength) {
      displayLastName = `${lastName.substring(0, maxLastNameLength - 3)}...`;
    }
    
    return `${displayFirstName} ${displayLastName}`;
  };

  return (
    <ClickAwayListener onClickAway={handleClickAway}>
      <Box sx={{ position: 'relative' }}>
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            cursor: 'pointer',
            transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
            borderRadius: 3,
            px: expanded ? 2 : 0,
            py: 1,
            backgroundColor: expanded ? 'rgba(0, 0, 0, 0.04)' : 'transparent',
            '&:hover': {
              backgroundColor: 'rgba(0, 0, 0, 0.04)'
            }
          }}
          onClick={handleAvatarClick}
        >
          <Avatar
            sx={{
              bgcolor: getAvatarColor(user.FirstName + user.LastName),
              width: 40,
              height: 40,
              fontSize: '1rem',
              fontWeight: 'bold',
              transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)'
            }}
          >
            {getInitials(user.FirstName, user.LastName)}
          </Avatar>
          
          <Collapse 
            orientation="horizontal" 
            in={expanded}
            timeout={300}
            easing="cubic-bezier(0.4, 0, 0.2, 1)"
          >
            <Box sx={{ ml: 2, minWidth: 120 }}>
              <Typography variant="body2" fontWeight="medium" noWrap>
                {formatDisplayName(user.FirstName, user.LastName)}
              </Typography>
            </Box>
          </Collapse>
        </Box>

        <Fade 
          in={expanded}
          timeout={200}
          easing="cubic-bezier(0.4, 0, 0.2, 1)"
        >
          <Paper
            sx={{
              position: 'absolute',
              top: '100%',
              right: 0,
              mt: 1,
              p: 2,
              minWidth: 220,
              zIndex: 1300,
              boxShadow: '0 8px 32px rgba(0, 0, 0, 0.12)',
              borderRadius: 2,
              transform: expanded ? 'translateY(0)' : 'translateY(-8px)',
              transition: 'transform 0.2s cubic-bezier(0.4, 0, 0.2, 1)'
            }}
          >
            <Box sx={{ mb: 2 }}>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                Email
              </Typography>
              <Typography variant="body2" fontWeight="medium">
                {user.email}
              </Typography>
            </Box>

            <Box sx={{ mb: 2 }}>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                Role
              </Typography>
              <Typography variant="body2" fontWeight="medium">
                {user.role}
              </Typography>
            </Box>

            <Divider sx={{ my: 1 }} />

            <Button
              fullWidth
              variant="outlined"
              color="error"
              startIcon={<Logout />}
              onClick={handleLogout}
              sx={{ 
                mt: 1,
                transition: 'all 0.2s ease'
              }}
            >
              Logout
            </Button>
          </Paper>
        </Fade>
      </Box>
    </ClickAwayListener>
  );
};

export default UserAvatar;
