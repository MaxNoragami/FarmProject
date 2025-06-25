export interface User {
  sub: string;
  email: string;
  FirstName: string;
  LastName: string;
  role: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}

export interface UserContextType {
  user: User | null;
  setUser: (user: User | null) => void;
}
