import React, { useEffect, useLayoutEffect, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import AuthFormBase from '../components/forms/AuthFormBase';
import LoginForm from '../components/forms/LoginForm';
import { type LoginFormFields } from '../schemas/loginSchemas';

const LoginPage: React.FC = () => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  useLayoutEffect(() => {
    const isMobile = window.matchMedia('(max-width: 600px)').matches;
    if (isMobile) {
      document.documentElement.style.overflow = 'hidden';
      document.body.style.overflow = 'hidden';
      document.body.style.position = 'fixed';
      document.body.style.width = '100%';
      document.body.style.height = '100%';
      window.scrollTo(0, 0);
      let viewportMeta = document.querySelector('meta[name="viewport"]');
      if (viewportMeta) {
        viewportMeta.setAttribute('content', 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover');
      }
    }
    return () => {
      document.documentElement.style.overflow = 'auto';
      document.body.style.overflow = 'auto';
      document.body.style.position = 'static';
      document.body.style.width = 'auto';
      document.body.style.height = 'auto';
    };
  }, []);

  useEffect(() => {
    const isMobile = window.matchMedia('(max-width: 600px)').matches;
    const handleResize = () => {
      if (isMobile) {
        window.scrollTo(0, 0);
      }
    };
    window.addEventListener('resize', handleResize);
    window.addEventListener('orientationchange', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
      window.removeEventListener('orientationchange', handleResize);
    };
  }, []);

  const handleLogin = async (data: LoginFormFields) => {
    setIsSubmitting(true);
    setFormError(null);
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      setIsSubmitting(false);
      console.log('Login submitted:', data);
    } catch (err: any) {
      setIsSubmitting(false);
      setFormError(err?.message || "An unexpected error occurred.");
    }
  };

  return (
    <>
      <Helmet>
        <title>Login - Farm Project</title>
      </Helmet>
      <AuthFormBase title="Farm Project">
        {formError && (
          <div style={{ marginBottom: 16 }}>
            <span style={{ color: "#d32f2f", fontSize: 14 }}>{formError}</span>
          </div>
        )}
        <LoginForm onSubmit={handleLogin} isSubmitting={isSubmitting} />
      </AuthFormBase>
    </>
  );
};

export default LoginPage;
