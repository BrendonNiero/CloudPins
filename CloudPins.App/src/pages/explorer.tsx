import DefaultLayout from "@/layouts/default";
import { getPinDetail } from "@/services/pinDetailService";
import { PinDetail } from "@/types/pinDetail";
import { Button } from "@heroui/button";
import { useCallback, useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { FaHeart } from "react-icons/fa";
import { Skeleton } from "@heroui/skeleton";
import { Pin } from "@/types/pin";
import { getFeedExplorer } from "@/services/explorerService";
import { Link } from "@heroui/link";
import { likePin } from "@/services/likePinService";
import { unlikePin } from "@/services/unlikePinService";
import { FaBookmark } from "react-icons/fa";


export default function Explorer()
{
    const { id } = useParams<{ id: string }>();

    const [pinDetail, setPinDetail] = useState<PinDetail>();
    const [feed, setFeed] = useState<Pin[]>([]);
    const [loadingDetail, setLoadingDetail] = useState(true);
    const [loadingFeed, setFeedLoading] = useState(true);
    const [error, setError] = useState("");
    const [feedError, setFeedError] = useState("");
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    const observer = useRef<IntersectionObserver | null>(null);

    const lastElementRef = useCallback(
        (node: HTMLDivElement | null) => 
        {
            if(loadingFeed) return;

            if(observer.current) observer.current.disconnect();
            
            observer.current = new IntersectionObserver((entries) =>
            {
                if(entries[0].isIntersecting && hasMore){
                    setPage((prev) => prev + 1);
                }
            });

            if(node) observer.current.observe(node);
        },
        [loadingFeed, hasMore]
    );

    async function loadPinDetail()
    {
        try {
            setLoadingDetail(true);
            const data: PinDetail = await getPinDetail(id!);

            if(!data) setError("Erro ao carregar detalhes do Pin");

            setPinDetail(data);
        }
        catch(error: any)
        {
            setError(error);
        }
        finally
        {
            setLoadingDetail(false);
        }
    }

    useEffect(() => {
        async function loadExplorerFeed()
        {
            try {
                setFeedLoading(true);
                if(!id){
                    setFeedError("Não foi possivel pegar seu feed");
                    return;
                }
                const data = await getFeedExplorer(id, page, 20);
                if(data.lenght === 0)
                {
                    setHasMore(false);
                }

                setFeed((prev) => [...prev, ...data]);
            }
            catch(error: any)
            {
                setFeedError(error);
            }
            finally
            {
                setFeedLoading(false);
            }
        }

        loadPinDetail();
        loadExplorerFeed();
    }, [id, page]);

    useEffect(() => {
        window.scrollTo({
            top: 0,
            behavior: "smooth"
        })
    }, [id]);

    async function HandleLikePin()
    {
        if(pinDetail?.isLiked) return;

        if(pinDetail != null && id != null)
        {
            
            await likePin(id);
            loadPinDetail();
        }
    }

    async function HandleUnlikePin()
    {
        if(!pinDetail?.isLiked) return;

        if(pinDetail != null && id != null)
        {
            await unlikePin(id);
            loadPinDetail();
        }
    }


    return(
        <DefaultLayout>
            <section className="columns-2 sm:col-end-3 md:columns-4 gap-3 space-y-3">
                {!loadingDetail ?
                    <div className="p-3 sm:p-4 lg:p-6  border border-default-500 w-full rounded-xl break-inside-avoid">
                        <img src={`http://localhost:5023${pinDetail?.imageUrl}`} className="rounded-lg" />
                        <h1 className="text-xl sm:text-4xl font-bold">{pinDetail?.title}</h1>
                        <p>{pinDetail?.description}</p>
                        <div className="flex items-center justify-between mt-4">
                            <div className="flex items-center gap-2">
                                {pinDetail?.isLiked ? 
                                    <Button isIconOnly color="danger" onPress={HandleUnlikePin}>
                                        <FaHeart />
                                    </Button> 
                                    : 
                                    <Button isIconOnly onPress={HandleLikePin}>
                                        <FaHeart />
                                    </Button>
                                }
                                <span className="text-3xl">{pinDetail?.likesCount}</span>
                            </div>
                            <Button isIconOnly  isDisabled color="primary" variant="shadow">
                                <FaBookmark />
                            </Button>
                        </div>
                    </div>
                 : 
                    <div className="p-8 border border-default-500 w-full rounded-xl flex flex-col gap-3">
                        <Skeleton className="h-[400px] w-full rounded-md"/>
                        <Skeleton className="p-6 rounded-md w-full" />
                        <Skeleton className="p-3 rounded-md w-full" />
                    </div>
                }   
                    { loadingFeed ? Array.from({ length: 20}).map((_, i) => (
                        <Skeleton key={i} className="h-80 w-full rounded-lg break-inside-avoid"/>
                    )) :
                    feed.map((pin) => (
                        <Link key={pin.id} href={`/explorar/${pin.id}`}>
                            <img src={`http://localhost:5023${pin.thumbnailUrl}`} 
                            className="w-full rounded-lg break-inside-avoid" />
                        </Link>
                    ))}
                </section>
                { hasMore && !error && (
                    <div ref={lastElementRef} className="flex justify-center my-10">
                        {loadingFeed && <Skeleton className="h-10 w-40 rounded-lg" />}
                    </div>
                )}
        </DefaultLayout>
    );
}