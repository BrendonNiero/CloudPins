export default function IndexPage() {
  return (
    <main className="p-5 h-screen overflow-hidden">
      <div>
        <div className="flex items-center gap-3">
          <img src="/cloudpins.png" className="h-12 rounded-xl"/>
          <h3 className="text-2xl">CloudPins</h3>
        </div>
      </div>
      <section className="h-full w-full flex items-center justify-center">
        <div className="w-[800px]  flex items-center justify-center flex-col text-center gap-5">
          <h1 className="text-8xl">Onde a inspiração vira organização.</h1>
          <p className="text-2xl">Crie sua conta agora mesmo para começar.</p>
          <div className="flex items-center gap-5">
            <button>Cadastre-se</button>
            <button>Entrar</button>
          </div>
        </div>
      </section>
    </main>
  );
}
