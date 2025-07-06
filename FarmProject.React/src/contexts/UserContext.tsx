import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
} from "react";
import type { User, UserContextType } from "../types/User";
import { AuthService } from "../services/AuthService";
import { setApiToken } from "../api/config";

const UserContext = createContext<UserContextType | undefined>(undefined);

export const useUser = () => {
  const context = useContext(UserContext);
  if (context === undefined) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
};

interface UserProviderProps {
  children: React.ReactNode;
}

function getUserFromToken(token: string | null): User | null {
  if (!token) return null;
  try {
    const payload = token.split(".")[1];
    const decoded = JSON.parse(
      atob(payload.replace(/-/g, "+").replace(/_/g, "/"))
    );

    let role = decoded.role;
    if (Array.isArray(role)) {
      role = role[0];
    }
    return { ...decoded, role };
  } catch {
    return null;
  }
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    setUser(getUserFromToken(token));
    setApiToken(token);
  }, [token]);

  const login = useCallback(async (email: string, password: string) => {
    const { token: newToken } = await AuthService.login(email, password);
    setToken(newToken);
  }, []);

  const register = useCallback(async (data: any) => {
    const { token: newToken } = await AuthService.register(data);
    setToken(newToken);
  }, []);

  const logout = useCallback(() => {
    setToken(null);
  }, []);

  return (
    <UserContext.Provider value={{ user, setUser, login, register, logout }}>
      {children}
    </UserContext.Provider>
  );
};
