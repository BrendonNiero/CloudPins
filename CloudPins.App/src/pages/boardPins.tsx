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

export default function BoardPins()
{
    const { id } = useParams<{ id: string }>();
    const [loading, setLoading] = useState(false);
    const [pins, setPins] = useState<Pin[]>([]);
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

    useEffect(() => {
        loadPins();
    }, []);

    // FUNÇÕES AUXILIARES PARA CRIAÇÃO DE PIN

    const handleClickSelectImage = () => {
        if(fileInputRef.current){
            fileInputRef.current.click();
        }
    };

    const handleCreatePin = async () => {
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
    return(
        <DefaultLayout>
            { error &&  <ErrorMensage error={error}/>}
            {!error &&
            <>
                <Button startContent={<FaPlus />} variant="shadow" color="primary">Criar novo Pin</Button>
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
            </>
            }
        </DefaultLayout>
    );
}