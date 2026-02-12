import HeaderVisitant from "@/components/headerVisitant";
import { Button } from "@heroui/button";
import { Card, CardBody } from "@heroui/card";
import { Input } from "@heroui/input";
import { Link } from "@heroui/link";

export default function Register()
{
    return(
        <main className="p-5 h-screen overflow-hidden">
            <HeaderVisitant />
            <section className="h-full w-full flex items-center justify-center">
                <Card className="min-w-[500px]">
                    <CardBody className="flex flex-col items-center justify-center text-center p-7">
                        <img src="/cloudpins.png" className="h-16 w-16 rounded-xl mb-5"/>
                        <h4 className="text-3xl">Criar uma nova conta</h4>
                        <p className="text-default-500 mb-5">Registre uma nova conta para ter acesso a todos os recursos.</p>
                        <div className="flex flex-col gap-3 w-full border-b border-b-default-500 pb-5">
                            <Input label="Email"/>
                            <Input label="Senha" type="password"/>
                            <Input label="Repita sua senha" type="password"/>
                            <Button color="primary" variant="shadow">Entrar</Button>
                        </div>
                        <div className="w-full flex items-center gap-1 mt-5">
                            <p>Já tem uma conta?</p>
                            <Link showAnchorIcon href="/login">Logar</Link>
                        </div>
                    </CardBody>
                </Card>
            </section>
        </main>
    );
}