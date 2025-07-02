import React from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate, Outlet, useLocation } from 'react-router-dom'
import { HelmetProvider } from 'react-helmet-async'
import './index.css'
import BaseLayout from './BaseLayout'
import RabbitsPage from './pages/RabbitsPage'
import TasksPage from './pages/TasksPage'
import CagesPage from './pages/CagesPage'
import PairsPage from './pages/PairsPage'
import RegisterPage from './pages/RegisterPage'
import { UserProvider, useUser } from './contexts/UserContext'
import LoginPage from './pages/LoginPage'
import LandingPage from './pages/LandingPage'

// PrivateRoute component
function PrivateRoute() {
  const { user } = useUser();
  const location = useLocation();
  // Check token expiry
  if (!user) {
    return <Navigate to="/" state={{ from: location }} replace />;
  }
  const now = Math.floor(Date.now() / 1000);
  if (user.exp && user.exp < now) {
    return <Navigate to="/" state={{ from: location }} replace />;
  }
  return <Outlet />;
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <HelmetProvider>
      <UserProvider>
        <BrowserRouter>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
              <Route path="/" element={<LandingPage />} />
              <Route element={<PrivateRoute />}>
                <Route element={<BaseLayout />}>
                  <Route path="rabbits" element={<RabbitsPage />} />
                  <Route path="tasks" element={<TasksPage />} />
                  <Route path="cages" element={<CagesPage />} />
                  <Route path="pairs" element={<PairsPage />} />
                </Route>
              </Route>
            </Routes>
        </BrowserRouter>
      </UserProvider>
    </HelmetProvider>
  </React.StrictMode>,
)