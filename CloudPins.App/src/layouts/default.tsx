import HeaderLogged from "@/components/headerLogged";

export default function DefaultLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="relative flex flex-col m-5">
      <HeaderLogged />
      <main className="mt-5">
        {children}
      </main>
    </div>
  );
}
