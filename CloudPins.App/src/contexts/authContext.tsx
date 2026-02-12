import { createContext, useContext, useState } from "react";

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
};

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }){
    const [user, setUser] = useState<User | null>(null);

    function loginUser(data: any){
        localStorage.setItem("token", data.token);
        setUser({
            userId: data.userId,
            name: data.name,
            email: data.email,
            profileUrl: data.profileUrl
        });
    }

    function logout(){
        localStorage.removeItem("token");
        setUser(null);
    }

    return (
        <AuthContext.Provider value={{ user, loginUser, logout}}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth()
{

    return useContext(AuthContext)!;
}