import { styled, useTheme } from "@mui/material/styles";
import Box from "@mui/material/Box";
import MuiDrawer from "@mui/material/Drawer";
import MuiAppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import CssBaseline from "@mui/material/CssBaseline";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import BottomNavigation from "@mui/material/BottomNavigation";
import BottomNavigationAction from "@mui/material/BottomNavigationAction";
import Drawer from "@mui/material/Drawer";
import useMediaQuery from "@mui/material/useMediaQuery";
import {
  Assignment,
  Bento,
  CrueltyFree,
  Favorite,
  PeopleAlt,
  Payments,
  Menu,
  MenuOpen,
} from "@mui/icons-material";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import UserAvatar from "./components/common/UserAvatar";
import * as React from "react";

const drawerWidth = 41;

const DrawerHeader = styled("div")(({ theme }) => ({
  display: "flex",
  alignItems: "center",
  justifyContent: "flex-end",
  padding: theme.spacing(0, 1),
  ...theme.mixins.toolbar,
}));

const AppBar = styled(MuiAppBar)(({ theme }) => ({
  zIndex: theme.zIndex.drawer + 1,
}));

const BaseLayout = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));
  const [mobileMenuOpen, setMobileMenuOpen] = React.useState(false);

  const navigationItems = [
    { path: "/tasks", label: "Tasks", icon: <Assignment /> },
    { path: "/rabbits", label: "Rabbits", icon: <CrueltyFree /> },
    { path: "/cages", label: "Cages", icon: <Bento /> },
    { path: "/pairs", label: "Pairs", icon: <Favorite /> },
    { path: "/customers", label: "Customers", icon: <PeopleAlt /> },
    { path: "/orders", label: "Orders", icon: <Payments /> },
  ];

  const handleNavigation = (path: string) => {
    navigate(path);
    setMobileMenuOpen(false);
  };

  const getBottomNavValue = () => {
    const index = navigationItems.findIndex(
      (item) => item.path === location.pathname
    );
    if (index !== -1 && index < 3) {
      return index;
    } else if (index >= 3) {
      return 3;
    }
    return 0;
  };

  const handleBottomNavChange = (
    event: React.SyntheticEvent,
    newValue: number
  ) => {
    if (newValue === 3 && navigationItems.length > 4) {
      setMobileMenuOpen(!mobileMenuOpen);
    } else if (newValue < 3) {
      navigate(navigationItems[newValue].path);
    }
  };

  const handleMobileMenuClose = () => {
    setMobileMenuOpen(false);
  };

  if (isMobile) {
    return (
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          height: "100dvh",
          width: "100vw",
          overflow: "hidden",
        }}
      >
        <CssBaseline />
        <AppBar position="fixed" sx={{ width: "100%" }}>
          <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
            <Typography variant="h6" noWrap component="div">
              Farm Project
            </Typography>
            <UserAvatar />
          </Toolbar>
        </AppBar>

        <Box
          component="main"
          sx={{
            flexGrow: 1,
            backgroundColor: "#f5f5f5",
            pt: 8,
            pb: 9,
            overflow: "hidden",
            display: "flex",
            flexDirection: "column",
            width: "100%",
            minHeight: 0,
          }}
        >
          <Outlet />
        </Box>

        <BottomNavigation
          value={getBottomNavValue()}
          onChange={handleBottomNavChange}
          sx={{
            position: "fixed",
            bottom: 0,
            left: 0,
            right: 0,
            width: "100%",
            borderTop: 1,
            borderColor: "divider",
            zIndex: 1000,
            height: 64,
          }}
        >
          {navigationItems.slice(0, 3).map((item, index) => (
            <BottomNavigationAction
              key={item.path}
              label={item.label}
              icon={item.icon}
            />
          ))}
          {navigationItems.length > 4 && (
            <BottomNavigationAction
              label="Menu"
              icon={mobileMenuOpen ? <MenuOpen /> : <Menu />}
            />
          )}
          {navigationItems.length === 4 && (
            <BottomNavigationAction
              label={navigationItems[3].label}
              icon={navigationItems[3].icon}
            />
          )}
        </BottomNavigation>

        {/* Mobile Menu Drawer */}
        <Drawer
          anchor="bottom"
          open={mobileMenuOpen}
          onClose={handleMobileMenuClose}
          sx={{
            zIndex: 1100,
            "& .MuiDrawer-paper": {
              borderTopLeftRadius: 16,
              borderTopRightRadius: 16,
              maxHeight: "50vh",
            },
          }}
        >
          <Box sx={{ p: 2 }}>
            <Typography
              variant="h6"
              sx={{ mb: 2, textAlign: "center" }}
            ></Typography>
            <List>
              {navigationItems.slice(3).map((item) => (
                <ListItem key={item.path} disablePadding>
                  <ListItemButton
                    onClick={() => handleNavigation(item.path)}
                    sx={{
                      backgroundColor:
                        location.pathname === item.path
                          ? "rgba(0, 0, 0, 0.04)"
                          : "transparent",
                      borderRadius: 1,
                      mb: 0.5,
                    }}
                  >
                    <ListItemIcon
                      sx={{
                        color:
                          location.pathname === item.path
                            ? "primary.main"
                            : "grey.600",
                      }}
                    >
                      {item.icon}
                    </ListItemIcon>
                    <ListItemText
                      primary={item.label}
                      sx={{
                        color:
                          location.pathname === item.path
                            ? "primary.main"
                            : "text.primary",
                      }}
                    />
                  </ListItemButton>
                </ListItem>
              ))}
            </List>
          </Box>
        </Drawer>
      </Box>
    );
  }

  return (
    <Box sx={{ display: "flex", height: "100vh", width: "100vw" }}>
      <CssBaseline />
      <AppBar position="fixed" sx={{ width: "100%" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
          <Typography variant="h6" noWrap component="div">
            Farm Project
          </Typography>
          <UserAvatar />
        </Toolbar>
      </AppBar>
      <MuiDrawer variant="permanent">
        <DrawerHeader />
        <Divider />
        <List>
          {navigationItems.map((item) => (
            <ListItem key={item.path} disablePadding sx={{ display: "block" }}>
              <ListItemButton
                sx={{
                  minHeight: 48,
                  px: 1,
                  justifyContent: "center",
                  backgroundColor:
                    location.pathname === item.path
                      ? "rgba(0, 0, 0, 0.04)"
                      : "transparent",
                }}
                onClick={() => handleNavigation(item.path)}
              >
                <ListItemIcon
                  sx={{
                    minWidth: 0,
                    justifyContent: "center",
                    color:
                      location.pathname === item.path
                        ? "primary.main"
                        : "grey.600",
                  }}
                >
                  {item.icon}
                </ListItemIcon>
              </ListItemButton>
            </ListItem>
          ))}
        </List>
        <Divider />
      </MuiDrawer>

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          backgroundColor: "#f5f5f5",
          height: "100vh",
          display: "flex",
          flexDirection: "column",
          ml: `${drawerWidth}px`, 
        }}
      >
        <DrawerHeader />
        <Box
          sx={{
            backgroundColor: "white",
            borderRadius: 1,
            boxShadow: 1,
            flex: 1,
            m: 3,
            p : 3,
            display: "flex",
            flexDirection: "column",
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
};

export default BaseLayout;
