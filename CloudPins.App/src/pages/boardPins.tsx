import ErrorMensage from "@/components/errorMensage";
import DefaultLayout from "@/layouts/default";
import { getPinsFromBoard } from "@/services/boardService";
import { Pin } from "@/types/pin";
import { Button } from "@heroui/button";
import { Link } from "@heroui/link";
import { Skeleton } from "@heroui/skeleton";
import { useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { FaPlus } from "react-icons/fa";
import { Modal, ModalBody, ModalContent, ModalFooter, useDisclosure  } from "@heroui/modal";
import { createPin } from "@/services/pinsService";
import { Tag } from "@/types/tag";
import { getTags } from "@/services/tagsService";
import { Input } from "@heroui/input";
import { Select, SelectItem } from "@heroui/select";
import { Card } from "@heroui/card";
import { Chip } from "@heroui/chip";

export default function BoardPins()
{
    const { id } = useParams<{ id: string }>();
    const [loading, setLoading] = useState(false);
    const [loadingTags, setLoadingTags] = useState(false);
    const [pins, setPins] = useState<Pin[]>([]);
    const [tags, setTags] = useState<Tag[]>();
    const [error, setError] = useState("");

    // PARA CRIAR NOVO PIN
    const [pinTitle, setPinTitle] = useState("");
    const [pinDescription, setPinDescription] = useState("");
    const [tagsPin, setTagsPin] = useState<string[]>([]);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const fileInputRef = useRef<HTMLInputElement | null>(null);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);

    // MODAL CRIAR PIN
    const { isOpen, onOpen, onOpenChange} = useDisclosure();

    async function loadPins()
    {
        try 
        {
            setLoading(true);
            const data = await getPinsFromBoard(id!);
            if(!data){
                setError("Essa sua Board está vazia.")
            }
            setPins(data.pins);
        }
        catch(error: any)
        {
            setError("Não foi possível puxar os pins dessa board.");
        }
        finally
        {
            setLoading(false);
        }
    }

    async function loadTags()
    {
        try
        {
            setLoadingTags(true);
            const data: Tag[] = await getTags();
            setTags(data);
        }
        catch(error: any)
        {
            //
        }
        finally
        {
            setLoadingTags(false);
        }
    }

    useEffect(() => {
        loadPins();
        loadTags();
    }, []);

    // FUNÇÕES AUXILIARES PARA CRIAÇÃO DE PIN

    const handleClickSelectImage = () => {
        if(fileInputRef.current){
            fileInputRef.current.click();
        }
    };

    async function handleSaveCreatePin(){
        const formData = new FormData();
        formData.append("Title", pinTitle);
        if(selectedFile)
            formData.append("Image", selectedFile);

        await createPin(formData);
        await loadPins();

        // LIMPANDO VARIÁVEIS DE CRIAÇÃO DE PIN
        setPreviewUrl(null);
        setSelectedFile(null);
        setPinTitle("");
        onOpenChange();
    }

    function handleModalCreatePinClose()
    {
        setPreviewUrl(null);
        setSelectedFile(null);
        setPinTitle("");
    }

    return(
        <DefaultLayout>
            { error &&  <ErrorMensage error={error}/>}
            {!error &&
            <>
                <Button onPress={onOpenChange}
                    startContent={<FaPlus />} variant="shadow" color="primary">Criar novo Pin</Button>
                <section className="columns-2 sm:col-end-3 md:columns-3 lg:columns-5 gap-3 space-y-3 mt-5">
                    { loading ?
                    Array.from({ length: 20}).map((_, i) => (
                        <Skeleton key={i} className="h-80 w-full rounded-lg break-inside-avoid"/>
                    )) : 
                    pins.map((pin) => (
                        <Link key={pin.id} href={`/explorar/${pin.id}`}>
                                <img src={`http://localhost:5023${pin.thumbnailUrl}`}
                                    className="w-full rounded-lg break-inside-avoid"/>
                            </Link>
                        ))
                    }
                </section>
                <Modal backdrop="blur" isOpen={isOpen} placement="bottom-center" 
                    onClose={handleModalCreatePinClose} onOpenChange={onOpenChange}>
                    <ModalContent>
                        {(onClose) => (
                            <>
                                <ModalBody>
                                    <Card onClick={handleClickSelectImage}
                                        className="mt-10 hover:opacity-80 transition ease-in-out cursor-pointer flex items-center justify-center p-10">
                                        <h2 className="text-2xl font-bold">Adicione sua imagem aqui</h2>
                                        <p>Somente JPEG, PNG ou WEBP serão aceitos.</p>
                                    </Card>
                                    <div className="mt-5 flex gap-3 flex-wrap">
                                        {tags?.map((tag) => (
                                            <Chip key={tag.id} onClose={(() => console.log(tag.name))}>{tag.name}</Chip>
                                        ))}
                                    </div>
                                    <Select placeholder="Selecione Tags">
                                        { (tags ?? []).map((tag) => <SelectItem key={tag.id}>{tag.name}</SelectItem>)}
                                    </Select>
                                </ModalBody>
                                <ModalFooter>
                                    <Button onPress={onClose}>Cancelar</Button>
                                    <Button onPress={onClose} color="primary" variant="shadow">Salvar</Button>
                                </ModalFooter>
                            </>
                        )}
                    </ModalContent>
                </Modal>
            </>
            }
        </DefaultLayout>
    );
}