import DefaultLayout from "@/layouts/default";
import { getBoards } from "@/services/boardService";
import { getProfile } from "@/services/profileService";
import { Board } from "@/types/board";
import { ProfileDetail } from "@/types/profileDetail";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { Link } from "@heroui/link";
import { Skeleton } from "@heroui/skeleton";
import { User } from "@heroui/user";
import { useEffect, useState } from "react";
import { FaPen } from "react-icons/fa";


export default function Profile()
{
    const [boards, setBoards] = useState<Board[]>([]);
    const [loadingBoards, setLoadingBoard] = useState(true);
    const [errorBoards, setErrorBoards] = useState("");

    const [profile, setProfile] = useState<ProfileDetail>();
    const [loadingProfile, setLoadingProfile] = useState(true);
    const [errorProfile, setErrorProfile] = useState("");

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

        async function loadProfile()
        {
            try {
                setLoadingProfile(true);
                const data = await getProfile();
                setProfile(data);
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
        loadProfile();
        loadBoards();
    }, []);
    return(
        <DefaultLayout>
            <div className="flex w-full items-center justify-between mb-5">
                <h1 className="text-4xl font-bold">Suas Boards</h1>
                    { errorProfile 
                    ? <h1>deu pau</h1>:
                    loadingProfile 
                    ? <Skeleton />
                    : 
                    <div className="flex items-center gap-5">
                        <User avatarProps={{ src: `http://localhost:5023${profile?.profileUrl}`}}
                        name={profile?.name}
                        description="3 Boards"/>
                        <Button isIconOnly color="primary" variant="shadow"><FaPen /></Button>
                    </div>
                    }
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