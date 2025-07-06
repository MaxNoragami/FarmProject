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
import BottomNavigation from "@mui/material/BottomNavigation";
import BottomNavigationAction from "@mui/material/BottomNavigationAction";
import useMediaQuery from "@mui/material/useMediaQuery";
import { Assignment, Bento, CrueltyFree, Favorite } from "@mui/icons-material";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import UserAvatar from "./components/common/UserAvatar";

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

const Drawer = styled(MuiDrawer)(() => ({
  width: drawerWidth,
  flexShrink: 0,
  whiteSpace: "nowrap",
  boxSizing: "border-box",
}));

const BaseLayout = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const handleNavigation = (path: string) => {
    navigate(path);
  };

  const getBottomNavValue = () => {
    switch (location.pathname) {
      case "/tasks":
        return 0;
      case "/rabbits":
        return 1;
      case "/cages":
        return 2;
      case "/pairs":
        return 3;
      default:
        return 1;
    }
  };

  const handleBottomNavChange = (
    event: React.SyntheticEvent,
    newValue: number
  ) => {
    const paths = ["/tasks", "/rabbits", "/cages", "/pairs"];
    navigate(paths[newValue]);
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
          <BottomNavigationAction label="Tasks" icon={<Assignment />} />
          <BottomNavigationAction label="Rabbits" icon={<CrueltyFree />} />
          <BottomNavigationAction label="Cages" icon={<Bento />} />
          <BottomNavigationAction label="Pairs" icon={<Favorite />} />
        </BottomNavigation>
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
      <Drawer variant="permanent">
        <DrawerHeader />
        <Divider />
        <List>
          {/* Tasks */}
          <ListItem key={"tasks"} disablePadding sx={{ display: "block" }}>
            <ListItemButton
              sx={{
                minHeight: 48,
                px: 1,
                justifyContent: "center",
                backgroundColor:
                  location.pathname === "/tasks"
                    ? "rgba(0, 0, 0, 0.04)"
                    : "transparent",
              }}
              onClick={() => handleNavigation("/tasks")}
            >
              <ListItemIcon
                sx={{
                  minWidth: 0,
                  justifyContent: "center",
                  color:
                    location.pathname === "/tasks"
                      ? "primary.main"
                      : "grey.600",
                }}
              >
                <Assignment />
              </ListItemIcon>
            </ListItemButton>
          </ListItem>

          {/* Rabbits */}
          <ListItem key={"rabbits"} disablePadding sx={{ display: "block" }}>
            <ListItemButton
              sx={{
                minHeight: 48,
                px: 1,
                justifyContent: "center",
                backgroundColor:
                  location.pathname === "/rabbits"
                    ? "rgba(0, 0, 0, 0.04)"
                    : "transparent",
              }}
              onClick={() => handleNavigation("/rabbits")}
            >
              <ListItemIcon
                sx={{
                  minWidth: 0,
                  justifyContent: "center",
                  color:
                    location.pathname === "/rabbits"
                      ? "primary.main"
                      : "grey.600",
                }}
              >
                <CrueltyFree />
              </ListItemIcon>
            </ListItemButton>
          </ListItem>

          {/* Cages */}
          <ListItem key={"cages"} disablePadding sx={{ display: "block" }}>
            <ListItemButton
              sx={{
                minHeight: 48,
                px: 1,
                justifyContent: "center",
                backgroundColor:
                  location.pathname === "/cages"
                    ? "rgba(0, 0, 0, 0.04)"
                    : "transparent",
              }}
              onClick={() => handleNavigation("/cages")}
            >
              <ListItemIcon
                sx={{
                  minWidth: 0,
                  justifyContent: "center",
                  color:
                    location.pathname === "/cages"
                      ? "primary.main"
                      : "grey.600",
                }}
              >
                <Bento />
              </ListItemIcon>
            </ListItemButton>
          </ListItem>

          {/* Pairs */}
          <ListItem key={"pairs"} disablePadding sx={{ display: "block" }}>
            <ListItemButton
              sx={{
                minHeight: 48,
                px: 1,
                justifyContent: "center",
                backgroundColor:
                  location.pathname === "/pairs"
                    ? "rgba(0, 0, 0, 0.04)"
                    : "transparent",
              }}
              onClick={() => handleNavigation("/pairs")}
            >
              <ListItemIcon
                sx={{
                  minWidth: 0,
                  justifyContent: "center",
                  color:
                    location.pathname === "/pairs"
                      ? "primary.main"
                      : "grey.600",
                }}
              >
                <Favorite />
              </ListItemIcon>
            </ListItemButton>
          </ListItem>
        </List>
        <Divider />
      </Drawer>

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          backgroundColor: "#f5f5f5",
          height: "100vh",
          display: "flex",
          flexDirection: "column",
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
            p: 3,
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
