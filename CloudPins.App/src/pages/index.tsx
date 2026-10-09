import { useAuth } from "@/contexts/authContext";
import { createProfile, login } from "@/services/authService";
import { Badge } from "@heroui/badge";
import { Button } from "@heroui/button";
import { Input } from "@heroui/input";
import ProfileImageCropper from "@/components/profileImageCropper";
import { useEffect, useRef, useState } from "react";
import { FaArrowLeft, FaCamera } from "react-icons/fa6";
import { useNavigate } from "react-router-dom";

type View = "login" | "register" | "profile";

export default function IndexPage() {
  const { loginUser } = useAuth();
  const navigate = useNavigate();
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [view, setView] = useState<View>("login");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");
  const [profileName, setProfileName] = useState("");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [imageToCrop, setImageToCrop] = useState<File | null>(null);
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    return () => {
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    };
  }, [previewUrl]);

  function changeView(nextView: View) {
    setError("");
    setView(nextView);
  }

  async function handleLogin() {
    setError("");
    if (!email.trim() || !password.trim()) return setError("Preencha e-mail e senha.");
    try {
      setIsSubmitting(true);
      const data = await login(email, password);
      loginUser(data);
      navigate("/feed");
    } catch (error: any) {
      setError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  function handleRegister() {
    setError("");
    if (!email.trim()) return setError("Preencha seu e-mail.");
    if (!password || !repeatPassword) return setError("Preencha e confirme sua senha.");
    if (password !== repeatPassword) return setError("As senhas devem ser iguais.");
    changeView("profile");
  }

  async function handleCreateProfile() {
    setError("");
    if (!selectedFile) return setError("Selecione uma foto de perfil.");
    if (!profileName.trim()) return setError("Preencha seu nome de usuário.");
    const formData = new FormData();
    formData.append("Email", email);
    formData.append("Password", password);
    formData.append("Name", profileName);
    formData.append("Image", selectedFile);
    try {
      setIsSubmitting(true);
      const user = await createProfile(formData);
      loginUser(user);
      navigate("/feed");
    } catch (error: any) {
      setError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  const isRegister = view !== "login";
  const isProfile = view === "profile";

  return (
    <main className="lg:max-h-screen min-h-screen bg-[#eaf4ff] p-4 sm:p-8 lg:p-50 flex items-center justify-center">
      <section className="mx-auto grid min-h-[calc(100vh-2rem)] w-full overflow-hidden rounded-[1rem] bg-white shadow-2xl shadow-blue-950/15 lg:h-full lg:grid-cols-[.94fr_1.06fr]">
        <div className="flex flex-col px-6 py-8 sm:px-12 lg:px-16 lg:py-12">
          <div className="flex items-center gap-3">
            <img src="/cloudpins.png" alt="CloudPins" className="h-11 w-11 rounded-xl" />
            <span className="text-xl font-semibold tracking-tight text-[#0b3a75]">CloudPins</span>
          </div>
          <div className="mx-auto flex w-full flex-1 flex-col justify-center py-10">
            {isProfile && <Button isIconOnly aria-label="Voltar" variant="light" className="mb-5 -ml-3 text-[#0b5fbd]" onPress={() => changeView("register")}><FaArrowLeft /></Button>}
            <span className="mb-4 h-3 w-3 rounded-full bg-[#3b9df8] shadow-[10px_8px_0_5px_rgba(138,198,255,.55)]" />
            <p className="text-sm font-medium uppercase tracking-[.18em] text-[#287fd0]">{isProfile ? "Último passo" : isRegister ? "Comece agora" : "Bem-vindo de volta"}</p>
            <h1 className="mt-3 text-3xl font-bold tracking-tight text-[#092b5a] sm:text-4xl">{isProfile ? "Personalize seu perfil" : isRegister ? "Crie sua conta" : "Entre na sua conta"}</h1>
            <p className="mt-3 text-sm leading-6 text-slate-500">{isProfile ? "Escolha uma foto e um nome para aparecer nas suas boards." : isRegister ? "Organize suas inspirações em um só lugar." : "Continue organizando tudo o que inspira você."}</p>
            <div className="mt-7 flex flex-col gap-4">
              {isProfile ? <>
                <input ref={fileInputRef} type="file" accept="image/*" className="hidden" onChange={(event) => { const file = event.target.files?.[0]; if (file) setImageToCrop(file); event.target.value = ""; }} />
                <button type="button" className="mx-auto rounded-full" onClick={() => fileInputRef.current?.click()}>
                  <Badge placement="bottom-right" content={<span className="p-1"><FaCamera /></span>} color="primary" size="lg">{previewUrl ? <img src={previewUrl} alt="Prévia do perfil" className="h-24 w-24 rounded-full object-cover cursor-pointer" /> : <div className="h-24 w-24 rounded-full bg-[#dceeff] cursor-pointer" />}</Badge>
                </button>
                <p className="-mt-2 text-center text-xs text-slate-500">Clique para selecionar sua foto</p>
                <Input value={profileName} onValueChange={setProfileName} label="Nome de usuário" placeholder="Como você quer ser chamado?" />
                <Button color="primary" size="lg" variant="shadow" isLoading={isSubmitting} onPress={handleCreateProfile}>Finalizar cadastro</Button>
              </> : <>
                <Input value={email} onValueChange={setEmail} type="email" label="E-mail" placeholder="voce@email.com" />
                <Input value={password} onValueChange={setPassword} type="password" label="Senha" placeholder="Digite sua senha" />
                {isRegister && <Input value={repeatPassword} onValueChange={setRepeatPassword} type="password" label="Confirme sua senha" placeholder="Repita sua senha" />}
                <Button color="primary" size="lg" variant="shadow" isLoading={isSubmitting} onPress={isRegister ? handleRegister : handleLogin}>{isRegister ? "Continuar" : "Entrar"}</Button>
              </>}
              {error && <p className="text-center text-sm text-danger">{error}</p>}
            </div>
            {!isProfile && <p className="mt-7 text-center text-sm text-slate-500">{isRegister ? "Já possui uma conta?" : "Ainda não possui uma conta?"}{" "}<button type="button" onClick={() => changeView(isRegister ? "login" : "register")} className="font-semibold text-[#0877df] hover:underline cursor-pointer">{isRegister ? "Entrar" : "Criar conta"}</button></p>}
          </div>
          <p className="text-center text-xs text-slate-400">CloudPins — onde a inspiração vira organização.</p>
        </div>
        <aside className="relative hidden min-h-[680px] overflow-hidden bg-[#0b5fbd] lg:block">
          <img src="/login-image.jpg" alt="Pessoa organizando suas inspirações" className="absolute inset-0 h-full w-full object-cover" />
          <div className="absolute inset-0 bg-gradient-to-t from-[#062b5c]/85 via-[#0b5fbd]/10 to-[#0b5fbd]/20" />
          <div className="absolute inset-x-8 bottom-8 rounded-2xl border border-white/25 bg-white/15 p-6 text-white backdrop-blur-md"><p className="text-xl font-semibold leading-snug">“Suas ideias merecem um lugar para crescer.”</p><p className="mt-3 text-sm text-blue-100">Salve referências, crie boards e encontre inspiração todos os dias.</p></div>
        </aside>
      </section>
      <ProfileImageCropper file={imageToCrop} isOpen={Boolean(imageToCrop)} onClose={() => setImageToCrop(null)} onComplete={(file, url) => { if (previewUrl) URL.revokeObjectURL(previewUrl); setSelectedFile(file); setPreviewUrl(url); setImageToCrop(null); }} />
    </main>
  );
}
