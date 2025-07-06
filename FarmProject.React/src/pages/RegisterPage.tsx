import React, { useEffect, useLayoutEffect, useState } from "react";
import { Helmet } from "react-helmet-async";
import AuthFormBase from "../components/forms/AuthFormBase";
import RegisterForm from "../components/forms/RegisterForm";
import { type RegisterFormFields } from "../schemas/registerSchemas";
import { useUser } from "../contexts/UserContext";
import { useNavigate } from "react-router-dom";

const RegisterPage: React.FC = () => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);
  const { register: registerUser, user } = useUser();
  const navigate = useNavigate();

  useLayoutEffect(() => {
    const isMobile = window.matchMedia("(max-width: 600px)").matches;
    if (isMobile) {
      document.documentElement.style.overflow = "hidden";
      document.body.style.overflow = "hidden";
      document.body.style.position = "fixed";
      document.body.style.width = "100%";
      document.body.style.height = "100%";
      window.scrollTo(0, 0);
      let viewportMeta = document.querySelector('meta[name="viewport"]');
      if (viewportMeta) {
        viewportMeta.setAttribute(
          "content",
          "width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover"
        );
      }
    }
    return () => {
      document.documentElement.style.overflow = "auto";
      document.body.style.overflow = "auto";
      document.body.style.position = "static";
      document.body.style.width = "auto";
      document.body.style.height = "auto";
    };
  }, []);

  useEffect(() => {
    const isMobile = window.matchMedia("(max-width: 600px)").matches;
    const handleResize = () => {
      if (isMobile) {
        window.scrollTo(0, 0);
      }
    };
    window.addEventListener("resize", handleResize);
    window.addEventListener("orientationchange", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
      window.removeEventListener("orientationchange", handleResize);
    };
  }, []);

  useEffect(() => {
    if (user) {
      navigate("/tasks", { replace: true });
    }
  }, [user, navigate]);

  const handleRegister = async (data: RegisterFormFields) => {
    setIsSubmitting(true);
    setFormError(null);
    try {
      await registerUser(data);
    } catch (err: any) {
      setFormError(
        err?.response?.data?.message ||
          err?.message ||
          "An unexpected error occurred."
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <>
      <Helmet>
        <title>Register - Farm Project</title>
      </Helmet>
      <AuthFormBase title="Farm Project">
        {formError && (
          <div style={{ marginBottom: 16 }}>
            <span style={{ color: "#d32f2f", fontSize: 14 }}>{formError}</span>
          </div>
        )}
        <RegisterForm onSubmit={handleRegister} isSubmitting={isSubmitting} />
      </AuthFormBase>
    </>
  );
};

export default RegisterPage;
