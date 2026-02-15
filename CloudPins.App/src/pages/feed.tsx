import DefaultLayout from "@/layouts/default";
import { getFeed } from "@/services/pinsService";
import { Link } from "@heroui/link";
import {Skeleton} from "@heroui/skeleton";
import { useState, useEffect } from "react";

type Pin = {
    id: string;
    thumbnailUrl: string;
};

export default function Feed()
{
    const [feed, setFeed] = useState<Pin[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        async function loadFeed(){
            try {
                const data = await getFeed();
                setFeed(data);
            }
            catch(error: any)
            {
                setError(error.message);
            }
            finally
            {
                setLoading(false);
            }
        }

        loadFeed();
    }, []);


    return(
        <DefaultLayout>
            <ul className="w-full flex items-center justify-center gap-5">
                <Link href="/feed" underline="always" color="foreground">Todos</Link>
                <Link href="/feed" underline="hover" color="foreground">Anime</Link>
                <Link href="/feed" underline="hover" color="foreground">Tecnologia</Link>
                <Link href="/feed" underline="hover" color="foreground">UI/UX</Link>
            </ul>
            {error && (
                <section className="h-full text-center mt-64 w-full flex items-center justify-center">
                    <h3 className="text-4xl">{error}</h3>
                </section>
            )}
            <section className="flex justify-center gap-5 flex-wrap mt-5">
                {loading ? 
                Array.from({ length: 20}).map((_, i) => (
                    <Skeleton key={i} className="rounded-lg p-48" />
                ))
            : feed.map((pin) => (
                <img src={`http://localhost:5023${pin.thumbnailUrl}`} 
                className="rounded-lg w-64 h-64 object-cover" />
            ))}
            </section>
        </DefaultLayout>
    );
}