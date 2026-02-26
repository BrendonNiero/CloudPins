import ErrorMensage from "@/components/errorMensage";
import DefaultLayout from "@/layouts/default";
import { getSearchFeed } from "@/services/pinsService";
import { Pin } from "@/types/pin";
import { Link } from "@heroui/link";
import { Skeleton } from "@heroui/skeleton";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

export default function FindPins()
{
    const  { search } = useParams<{ search: string}>();
    const [error, setError] = useState("");
    const [pins, setPins] = useState<Pin[]>([]);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
        async function loadPins()
        {
            setLoading(true);
            try
            {
                const data = await getSearchFeed(search!);
                if(!data || data.length == 0)
                {
                    setError("Não encontramos nenhum pin relacionado.");
                }
                setPins(data)
            }
            catch(error: any)
            {
                setError("Erro no catch");
            }
            finally
            {
                setLoading(false);
            }
        }

    loadPins();
    }, []);
    return(
        <DefaultLayout>
            {error && <ErrorMensage error={error} />}
            {!error &&
            <>
                <section className="columns-2 sm:col-end-3 md:columns-3 lg:columns-5 gap-3 space-y-3 mt-5">
                    {loading ? 
                    Array.from({ length: 20}).map((_, i) => (
                        <Skeleton key={i} className="h-80 w-full rounded-lg break-inside-avoid"/>
                    )) :
                    pins.map((pin) => (
                         <Link key={pin.id} href={`/explorar/${pin.id}`}>
                            <img src={`http://localhost:5023${pin.thumbnailUrl}`}
                                className="w-full rounded-lg break-inside-avoid"/>
                        </Link>
                    ))}
                </section>
            </>
            }
        </DefaultLayout>
    );
}