import DefaultLayout from "@/layouts/default";
import { getBoards } from "@/services/boardService";
import { Board } from "@/types/board";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { Link } from "@heroui/link";
import { Skeleton } from "@heroui/skeleton";
import { User } from "@heroui/user";
import { useEffect, useState } from "react";

export default function Profile()
{
    const [boards, setBoards] = useState<Board[]>([]);
    const [loadingBoards, setLoadingBoard] = useState(true);
    const [errorBoards, setErrorBoards] = useState("");


    useEffect(() => {
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
        loadBoards();
    }, []);
    return(
        <DefaultLayout>
            <div className="flex w-full items-center justify-between mb-5">
                <h1 className="text-4xl font-bold">Suas Boards</h1>
                <div className="flex items-center gap-5">
                    <User avatarProps={{ src: "https://i.pinimg.com/736x/31/52/59/31525978bcee3d8f47f11f78be4df9c6.jpg"}}
                        name="Brendon Berzins"
                        description="3 Boards"/>
                    <Button color="primary" variant="shadow">Editar</Button>
                </div>
            </div>
            <section>
                <Button color="primary" variant="shadow">Nova Board</Button>
                <div className="flex items-center gap-5 mt-5">
                    {errorBoards && <h1>{errorBoards}</h1>}
                    {loadingBoards ?
                        Array.from({ length: 3}).map((_, i) => (
                            <Skeleton key={i} className="p-32 rounded-md"></Skeleton>
                        ))
                        : boards.map((board) => (
                            <div key={board.id}>
                                <Link href="/">
                                    <Card className="p-3 rounded-2xl overflow-hidden">
                                        <div className="grid grid-cols-3 grid-rows-2 gap-4 h-52">
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
        </DefaultLayout>
    );
}