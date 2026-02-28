import HeaderVisitant from "@/components/headerVisitant";
import { Badge } from "@heroui/badge";
import { Button } from "@heroui/button";
import { Card, CardBody } from "@heroui/card";
import { Input } from "@heroui/input";
import { Link } from "@heroui/link";
import { useRef, useState } from "react";
import { FaCamera } from "react-icons/fa6";

export default function Register()
{
    // STEP 1
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");
    const [firstStepError, setFirstStepError] = useState("");

    // STEP 2
    const [step, setStep] = useState(1);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);
    const [profileName, setProfileName] = useState("");

    const fileInputRef = useRef<HTMLInputElement | null>(null);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);

    function handleClickSelectImage() {
        if(fileInputRef.current){
            fileInputRef.current.click();
        }
    }

    function handleSubmitFirstStep()
    {
        setFirstStepError("");

        if(!email){
            setFirstStepError("Preencha seu email.")
            return;
        }
        if(!password || !repeatPassword)
        {
            setFirstStepError("Preencha sua senha.");
            return;
        }
        if(password != repeatPassword)
        {
            setFirstStepError("As senhas devem ser iguais");
            return;
        }
        setStep(2);
    }

    function handleCreateUser()
    {

    }

    return(
        <main className="p-5 h-screen overflow-hidden">
            <HeaderVisitant />
            <section className="h-full w-full flex items-center justify-center">
                {step == 1 ?
                <Card className="w-full md:max-w-[500px]">
                    <CardBody className="flex flex-col items-center justify-center text-center p-7">
                        <img src="/cloudpins.png" className="h-12 w-12 md:h-16 md:w-16 rounded-xl mb-5"/>
                        <h4 className="text-3xl">Criar uma nova conta</h4>
                        <p className="text-default-500 mb-5">Registre uma nova conta para ter acesso a todos os recursos.</p>
                        <div className="flex flex-col gap-3 w-full border-b border-b-default-500 pb-5">
                            <Input label="Email"/>
                            <Input label="Senha" type="password"/>
                            <Input label="Repita sua senha" type="password"/>
                            <Button color="primary" variant="shadow" onPress={handleSubmitFirstStep}>Entrar</Button>
                            {firstStepError && <p className="text-red-500">{firstStepError}</p>}
                        </div>
                        <div className="w-full flex items-center gap-1 mt-5">
                            <p>Já tem uma conta?</p>
                            <Link showAnchorIcon href="/login">Logar</Link>
                        </div>
                    </CardBody>
                </Card>
                :
                <Card>
                    <div className="w-full flex items-center justify-center mt-10 mb-5">
                        <input onChange={(e) => {
                            const file = e.target.files?.[0];
                            if(file){ 
                                setSelectedFile(file);
                                const preview = URL.createObjectURL(file);
                                setPreviewUrl(preview);
                            }
                        }}
                        type="file" accept="image/*" ref={fileInputRef}
                        className="hidden" />
                        <div className="cursor-pointer hover:opacity-80 transition ease-in-out" onClick={handleClickSelectImage}>
                            <Badge 
                             placement="bottom-right" content={<div className="py-3 px-2"><FaCamera /></div>} size="lg">
                                {previewUrl &&
                                <img src={previewUrl} className="w-24 h-24 rounded-full"/>
                                }
                            </Badge>
                        </div>
                    </div>
                    <Input value={profileName}
                    onChange={(e) => setProfileName(e.target.value)}
                    label="Nome de usuário" />
                </Card>
                }
            </section>
        </main>
    );
}