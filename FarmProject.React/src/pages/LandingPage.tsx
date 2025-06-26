import { Link as RouterLink } from 'react-router-dom';

const LandingPage = () => {
  return (
    <div>
          <RouterLink
              to="/login"
              style={{
                  textDecoration: 'none',
                  color: 'var(--mui-palette-primary-main, #1976d2)',
                  fontWeight: 600
              }}
          >
              Login
          </RouterLink>

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
    </div>
  );
}

export default LandingPage;