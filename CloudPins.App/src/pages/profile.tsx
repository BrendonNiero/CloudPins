import DefaultLayout from "@/layouts/default";
import { createBoard, getBoards } from "@/services/boardService";
import { getProfile, updateProfile } from "@/services/profileService";
import { Board } from "@/types/board";
import { ProfileDetail } from "@/types/profileDetail";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { Link } from "@heroui/link";
import { Skeleton } from "@heroui/skeleton";
import { User } from "@heroui/user";
import { useEffect, useState, useRef } from "react";
import { FaPen } from "react-icons/fa";
import { Modal, ModalBody, ModalContent, ModalFooter, useDisclosure  } from "@heroui/modal";
import { Input } from "@heroui/input";
import {Badge} from "@heroui/badge";
import { FaCamera } from "react-icons/fa";
import { Checkbox } from "@heroui/checkbox";
import { cn } from "@heroui/theme";
import { FaLock } from "react-icons/fa";
import { useAuth } from "@/contexts/authContext";
import { FiLogOut } from "react-icons/fi";
import { useNavigate } from "react-router-dom";


export default function Profile()
{
    const { logout } = useAuth();
    const navigate = useNavigate();
    const [boards, setBoards] = useState<Board[]>([]);
    const [loadingBoards, setLoadingBoard] = useState(true);
    const [errorBoards, setErrorBoards] = useState("");

    const [profile, setProfile] = useState<ProfileDetail>();
    const [loadingProfile, setLoadingProfile] = useState(true);
    const [errorProfile, setErrorProfile] = useState("");
    const { updateUser } = useAuth();

    const [editedName, setEditedName] = useState("");
    // PARA SELECT IMAGE
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const fileInputRef = useRef<HTMLInputElement | null>(null);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);

    // MODAL PROFILE
    const { isOpen: isProfileOpen, onOpen: onProfileOpen, 
        onOpenChange: onProfileOpenChange } = useDisclosure();

    // MODAL BOARD
    const { isOpen: isBoardOpen, onOpen: onBoardOpen,
            onOpenChange: onBoardOpenChange } = useDisclosure();

    // PARA CRIAR BOARD 
    const [isBoardPrivate, setIsBoardPrivate] = useState(false);
    const [boardName, setBoardName] = useState("");
    const [boardError, setBoardError] = useState("");

    async function loadProfile()
    {
        try {
            setLoadingProfile(true);
            const data = await getProfile();
            setProfile(data);
            setEditedName(data.name);
        }
        catch(error: any)
        {
            setErrorProfile(error);
        }
        finally
        {
            setLoadingProfile(false)
        }
    }

    async function loadBoards()
    {
        try {
            setLoadingBoard(true);
            const data: Board[] = await getBoards();
            if(data.length === 0)
            {
                setErrorBoards("Você ainda não possui boards");
            }
            setBoards(data);
        }
        catch(error: any)
        {
            setErrorBoards(error.message);
        }
        finally
        {
            setLoadingBoard(false);
        }
    }

    useEffect(() => {
        loadProfile();
        loadBoards();
    }, []);
    
    const handleClickSelectImage = () => {
        if(fileInputRef.current){
            fileInputRef.current.click();
        }
    };

    const handleSaveProfile = async () => {
        if(!profile) return;
        if(!editedName) return;

        const formData = new FormData();
        formData.append("Name", editedName);
        if(selectedFile)
            formData.append("Image", selectedFile);

        const updated = await updateProfile(formData);

        updateUser({
          name: updated.name,
          profileUrl: updated.profileUrl,
        });
    
        await loadProfile();

        // LIMPA PREVIEW E SELECTEDFILE
        setPreviewUrl(null);
        setSelectedFile(null);
        onProfileOpenChange();
    };

    // Evita memory leak
    useEffect(() => {
        return () => {
            if(previewUrl){
                URL.revokeObjectURL(previewUrl);
            };
        }
    }, [previewUrl]);

    // CONTROLA ABERTURA E FECHAMENTO DE MODAL
    function handleModalProfileClose()
    {
        setPreviewUrl(null);
        setSelectedFile(null);
    }
    function handleModalProfileOpen()
    {
            if(profile){
            setEditedName(profile.name);
        }
        onProfileOpen();
    }

    function handleModalBoardClose(){
        setBoardName("");
        setBoardError("");
        setIsBoardPrivate(false);
    }
    function handleModalBoardOpen(){
        onBoardOpen();
    }
    
    async function handleSaveBoard()
    {
        setBoardError("");
        if(!boardName)
        {
            setBoardError("Preencha o nome da Board");
            return;
        }

        var isPublic = !isBoardPrivate;
        var name = boardName;

        await createBoard({ name, isPublic });
        handleModalBoardClose();
        onBoardOpenChange();
        loadBoards();
    }

    function handleLogout()
    {
        navigate("/login");
        logout();
    }

    return(
        <DefaultLayout>
            <div className="flex w-full items-center justify-between mb-5 flex-wrap gap-5">
                <h1 className="text-4xl font-bold">Suas Boards</h1>
                    { errorProfile 
                    ? <Skeleton className="w-48 h-12 rounded-xl"/> :
                    loadingProfile 
                    ? <Skeleton className="w-48 h-12 rounded-xl"/>
                    : 
                    <div className="flex items-center gap-5">
                        <User avatarProps={{ src: `http://localhost:5023${profile?.profileUrl}?t=${Date.now()}`}}
                        name={profile?.name}
                        description="3 Boards"/>
                        <Button  onPress={handleModalProfileOpen} isIconOnly color="primary" variant="shadow"><FaPen /></Button>
                        <Button onPress={handleLogout} startContent={<FiLogOut />} color="danger">Sair</Button>
                    </div>
                    }
            </div>
            <section>
                <Button color="primary" variant="shadow" onPress={handleModalBoardOpen}>Nova Board</Button>
                <div className="flex items-center flex-wrap gap-5 mt-5">
                    {errorBoards && <h1>{errorBoards}</h1>}
                    {loadingBoards ?
                        Array.from({ length: 3}).map((_, i) => (
                            <Skeleton key={i} className="p-32 rounded-md"></Skeleton>
                        ))
                        : boards.map((board) => (
                            <div key={board.id}>
                                <Link href={`/board/${board.id}`}>
                                    <Card className="p-3 rounded-2xl overflow-hidden">
                                        {!board.isPublic && <Card className="p-4 rounded-full absolute"><FaLock className="text-default-800"/></Card>}
                                        <div className="grid grid-cols-3 grid-rows-2 gap-4 h-52 min-w-60">
                                            {board.lastPins.slice(0, 3).map((lastPin, i) => {
                                                if(i === 0){
                                                    return(
                                                        <img key={i} src={`http://localhost:5023${lastPin.thumbnailUrl}`}
                                                        className="col-span-2 row-span-2 w-full h-full object-cover rounded-xl"/>
                                                    );
                                                }
                                                return (
                                                    <img key={i} src={`http://localhost:5023${lastPin.thumbnailUrl}`}
                                                    className="w-full h-full object-cover rounded-xl" />
                                                );
                                            })}
                                        </div>
                                    </Card>
                                </Link>
                                <h4 className="text-2xl">{board.name}</h4>
                                <h4>{board.pinsCount} Pins</h4>
                            </div>
                        ))
                    }
                </div>
            </section>
            <Modal backdrop="blur" isOpen={isProfileOpen} placement="bottom-center" onClose={handleModalProfileClose} onOpenChange={onProfileOpenChange}>
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalBody>
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
                                            <img src={
                                                previewUrl ? previewUrl :
                                                `http://localhost:5023${profile?.profileUrl}?t=${Date.now()}`
                                            } className="w-24 h-24 rounded-full"/>
                                        </Badge>
                                    </div>
                                </div>
                                <Input value={editedName}
                                onChange={(e) => setEditedName(e.target.value)}
                                label="Nome de usuário" />
                            </ModalBody>
                            <ModalFooter>
                                <Button onPress={onClose}>Cancelar</Button>
                                <Button onPress={handleSaveProfile} color="primary" variant="shadow">Salvar</Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
            <Modal backdrop="blur" isOpen={isBoardOpen} placement="bottom-center" onClose={handleModalBoardClose} onOpenChange={onBoardOpenChange}>
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalBody>
                                <Input className="mt-10 mb-2" placeholder="Nome da Board" onChange={(e) => setBoardName(e.target.value)}/>
                                <Checkbox isSelected={isBoardPrivate} onValueChange={setIsBoardPrivate}
                                      classNames={{
                                      base: cn(
                                        "inline-flex w-full max-w-md bg-content1",
                                        "hover:bg-content2 items-center justify-start",
                                        "cursor-pointer rounded-lg gap-2 p-2 border-2 border-transparent",
                                        "data-[selected=true]:border-primary",
                                      ),
                                      label: "w-full",
                                    }}>
                                    <div className="flex gap-2 items-center">
                                        <Card className="p-3 rounded-full"><FaLock className="text-default-500"/></Card>
                                        <p>Tornar Privado</p>
                                    </div>
                                </Checkbox>
                                {boardError && <p className="text-red-500">{boardError}</p>}
                            </ModalBody>
                            <ModalFooter>
                                <Button onPress={onClose}>Cancelar</Button>
                                <Button onPress={handleSaveBoard} color="primary" variant="shadow">Salvar</Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
        </DefaultLayout>
    );
}