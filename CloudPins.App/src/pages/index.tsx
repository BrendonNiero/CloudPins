import HeaderVisitant from "@/components/headerVisitant";
import { Button } from "@heroui/button";
import { Link } from "@heroui/link";

export default function IndexPage() {
  return (
    <main className="p-5 h-screen overflow-hidden">
      <HeaderVisitant />
      <section className="h-full w-full flex items-center justify-center">
        <div className="w-[800px]  flex items-center justify-center flex-col text-center gap-5">
          <h1 className="text-8xl">Onde a inspiração vira organização.</h1>
          <p className="text-2xl">Crie sua conta agora mesmo para começar.</p>
          <div className="flex items-center gap-3">
            <Button as={Link} href="/register" color="primary" variant="shadow">Cadastre-se</Button>
            <Button as={Link} href="/login" variant="flat">Entrar</Button>
          </div>
        </div>
      </section>
    </main>
  );
}
