import React, { createContext, useContext, useState } from 'react';
import type { User, UserContextType } from '../types/User';

const UserContext = createContext<UserContextType | undefined>(undefined);

export const useUser = () => {
  const context = useContext(UserContext);
  if (context === undefined) {
    throw new Error('useUser must be used within a UserProvider');
  }
  return context;
};

interface UserProviderProps {
  children: React.ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  // Mock user data
  const [user, setUser] = useState<User | null>({
    sub: "alexeimaxim2004@gmail.com",
    email: "alexeimaxim2004@gmail.com",
      FirstName: "Maxim",
      LastName: "Alexei",
    role: "Worker",
    nbf: 1750775918,
    exp: 1750783118,
    iat: 1750775918,
    iss: "AuthAPI",
    aud: "Swagger-Client"
  });

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};
