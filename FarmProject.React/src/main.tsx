import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { HelmetProvider } from 'react-helmet-async'
import './index.css'
import BaseLayout from './BaseLayout'
import RabbitsPage from './pages/RabbitsPage'
import TasksPage from './pages/TasksPage'
import CagesPage from './pages/CagesPage'
import PairsPage from './pages/PairsPage'
import { UserProvider } from './contexts/UserContext'


createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <HelmetProvider>
      <UserProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<BaseLayout />}>
              <Route index element={<Navigate to="/rabbits" replace />} />
              <Route path="rabbits" element={<RabbitsPage />} />
              <Route path="tasks" element={<TasksPage />} />
              <Route path="cages" element={<CagesPage />} />
              <Route path="pairs" element={<PairsPage />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </UserProvider>
    </HelmetProvider>
  </StrictMode>,
)

// Prevent Edge trackpad navigation gestures
if (navigator.userAgent.includes('Edg')) {
  // Prevent swipe gestures from triggering browser navigation
  window.addEventListener('wheel', (e) => {
    if (Math.abs(e.deltaX) > Math.abs(e.deltaY)) {
      e.preventDefault();
    }
  }, { passive: false });

  // Prevent touch gestures
  window.addEventListener('touchstart', (e) => {
    if (e.touches.length > 1) {
      e.preventDefault();
    }
  }, { passive: false });

  // Prevent pointer events that could trigger navigation
  window.addEventListener('pointerdown', (e) => {
    if (e.pointerType === 'touch' && e.isPrimary === false) {
      e.preventDefault();
    }
  }, { passive: false });
}
