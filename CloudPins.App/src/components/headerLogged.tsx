import { Input } from "@heroui/input";
import { Form } from "@heroui/form";
import { ThemeSwitch } from "./theme-switch";
import { Link } from "@heroui/link";
import { IoSearch } from "react-icons/io5";
import { useEffect, useState } from "react";
import { ProfileDetail } from "@/types/profileDetail";
import { getProfile } from "@/services/profileService";
import { Skeleton } from "@heroui/skeleton";
import { useNavigate } from "react-router-dom";


export default function HeaderLogged()
{
    const [loading, setLoading] = useState(true);
    const [profile, setProfile] = useState<ProfileDetail>();
    const [error, setError] = useState(false);

    const [search, setSearch] = useState("");
    const navigate = useNavigate();

    function toSlug(value: string){
        return value.toLowerCase()
        .trim().replace(/\s+/g, "-");
    }

    function handleSearch()
    {
        if(!search.trim()) return;
        navigate(`/search/${toSlug(search)}`);
    }

    useEffect(() => {
        async function loadProfile()
        {
            try {
                setLoading(true);
                const data = await getProfile();
                setProfile(data);
            }
            catch
            {
                setError(true);
            }
            finally
            {
                setLoading(false);
            }
        }
        loadProfile();
    }, []);
    return(
        <div className="flex items-center justify-between gap-5">
            <Link href="/feed">
                <img src="/cloudpins.png" className="h-10 min-w-10 md:h-12 md:min-w-12 rounded-xl"/>
            </Link>
            <Form onSubmit={handleSearch} className="w-full flex items-center justify-center">
                <Input 
                value={search}
                onValueChange={setSearch}
                placeholder="Pesquisar" 
                startContent={<IoSearch />} 
                className="w-full md:max-w-[600px] lg:max-w-[800px]" />
            </Form>
            <div className="flex items-center gap-5">
                <ThemeSwitch />
                {
                    error ? <Skeleton className="h-12 w-12 rounded-full" /> :
                    loading ? <Skeleton className="h-12 w-12 rounded-full" /> :
                        <Link href="/profile">
                        <img className="h-10 min-w-10 md:h-12 md:min-w-12 rounded-full" src={`http://localhost:5023${profile?.profileUrl}`} />
                        </Link>
                }
            </div>
        </div>
    );
}