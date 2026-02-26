import ErrorMensage from "@/components/errorMensage";
import DefaultLayout from "@/layouts/default";
import { getFeed } from "@/services/pinsService";
import { getTags } from "@/services/tagsService";
import { Pin } from "@/types/pin";
import { Tag } from "@/types/tag";
import { Link } from "@heroui/link";
import {Skeleton} from "@heroui/skeleton";
import { useState, useEffect, useRef, useCallback } from "react";

export default function Feed()
{
    const [feed, setFeed] = useState<Pin[]>([]);
    const [loading, setLoading] = useState(true);
    const [tags, setTags] = useState<Tag[]>([]);
    const [error, setError] = useState("");
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    const observer = useRef<IntersectionObserver | null>(null);

    const lastElementRef = useCallback(
        (node: HTMLDivElement | null) => 
        {
            if(loading) return;

            if(observer.current) observer.current.disconnect();

            observer.current = new IntersectionObserver((entries) => 
            {
                if(entries[0].isIntersecting && hasMore){
                    setPage((prev) => prev + 1);
                }
            });

            if(node) observer.current.observe(node);
        },
        [loading, hasMore]
    );

    useEffect(() => {
        async function loadTags()
        {
            const data = await getTags();
            if(data)
            {
                setTags(data);
            }
        }
        async function loadFeed(){
            try {
                setLoading(true);
                const data = await getFeed(page, 20);

                if(data.length === 0){
                    setHasMore(false);
                }
                setFeed((prev) => [...prev, ...data]);
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
        loadTags();
    }, [page]);


    return(
        <DefaultLayout>
            <ul className="w-full flex items-center justify-center gap-5">
                <Link href="/feed" underline="always" color="foreground">Todos</Link>
                {tags.map((tag) => (
                    <Link href={`/search/${tag.name}`} color="foreground">{tag.name}</Link>
                ))}
            </ul>
            {error && (
                <ErrorMensage error={error}/>
            )}
            <section className="columns-2 sm:col-end-3 md:columns-4 lg:columns-5 gap-3 space-y-3 mt-5">
                {loading ? 
                Array.from({ length: 20}).map((_, i) => (
                    <Skeleton key={i} className="h-80 w-full rounded-lg break-inside-avoid" />
                ))
            : feed.map((pin) => (
                <Link key={pin.id} href={`/explorar/${pin.id}`}>
                <img src={`http://localhost:5023${pin.thumbnailUrl}`} 
                className="w-full rounded-lg break-inside-avoid" />
                </Link>
            ))}
            </section>
            { hasMore && !error && (
              <div ref={lastElementRef} className="flex justify-center my-10">
                {loading && <Skeleton className="h-10 w-40 rounded-lg" />}
              </div>
            )}
        </DefaultLayout>
    );
}