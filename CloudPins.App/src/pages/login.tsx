import HeaderVisitant from "@/components/headerVisitant";
import { useAuth } from "@/contexts/authContext";
import { login } from "@/services/authService";
import { Button } from "@heroui/button";
import { Card, CardBody } from "@heroui/card";
import { Input } from "@heroui/input";
import { Link } from "@heroui/link";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login()
{
    const { loginUser } = useAuth();
    const navigate = useNavigate();

    const [email, setEmail] = useState("brendon@gmail.com");
    const [password, setPassword] = useState("123");
    const [error, setError] = useState("");

    async function handleLogin()
    {
        try{
            const data = await login(email, password);
            loginUser(data);
            navigate("/feed");
        }
        catch
        {
            setError("Email ou senha inválidos!");
        }
    }

    return(
        <main className="p-5 h-screen overflow-hidden">
            <HeaderVisitant />
            <section className="h-full w-full flex items-center justify-center">
                <Card className="min-w-[500px]">
                    <CardBody className="flex flex-col items-center justify-center text-center p-7">
                        <img src="/cloudpins.png" className="h-16 w-16 rounded-xl mb-5"/>
                        <h4 className="text-3xl">Entrar em sua conta</h4>
                        <p className="text-default-500 mb-5">Realize o acesso a sua conta existente</p>
                        <div className="flex flex-col gap-3 w-full border-b border-b-default-500 pb-5">
                            <Input label="Email"/>
                            <Input label="Senha" type="password"/>
                            <Button onPress={handleLogin} color="primary" variant="shadow">Entrar</Button>
                        </div>
                        {error && <p className="text-red-500">{error}</p>}
                        <div className="w-f ull flex items-center gap-1 mt-5">
                            <p>Não possui uma conta?</p>
                            <Link showAnchorIcon href="/register">Cadastre-se</Link>
                        </div>
                    </CardBody>
                </Card>
            </section>
        </main>
    );
}