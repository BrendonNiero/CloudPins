import { createContext, useContext, useEffect, useState } from "react";

type User = {
  userId: string;
  name: string;
  email: string;
  profileUrl: string;
};

type AuthContextType = {
  user: User | null;
  loginUser: (data: any) => void;
  logout: () => void;
  updateUser: (data: Partial<User>) => void;
};

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);

  // Hydrate do localStorage ao montar
  useEffect(() => {
    const raw = localStorage.getItem("user");
    if (!raw) return;
    try {
      const parsed: User = JSON.parse(raw);
      setUser(parsed);
    } catch {
      localStorage.removeItem("token");
    }
  }, []);

  function persistUser(u: User | null) {
    if (u) localStorage.setItem("user", JSON.stringify(u));
    else localStorage.removeItem("user");
  }

  function loginUser(data: any) {
    localStorage.setItem("token", data.token);
    const u: User = {
      userId: data.userId,
      name: data.name,
      email: data.email,
      profileUrl: data.profileUrl,
    };
    setUser(u);
    persistUser(u);
  }

  function updateUser(data: Partial<User>) {
    setUser((prev) => {
        if(!prev) return prev;

      const updated: User = {
        ...prev,
        ...data,
      };

      persistUser(updated);
      return updated;
    });
  }

  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  }

  return (
    <AuthContext.Provider value={{ user, loginUser, updateUser, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext)!;
}