import { Input } from "@heroui/input";
import { Form } from "@heroui/form";
import { ThemeSwitch } from "./theme-switch";
import { Link } from "@heroui/link";
import { IoSearch } from "react-icons/io5";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/contexts/authContext";
import { getSearchSuggestions } from "@/services/pinsService";

export default function HeaderLogged()
{
    const { user } = useAuth();
    const [search, setSearch] = useState("");
    const [suggestions, setSuggestions] = useState<string[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        if (search.trim().length < 2) {
            setSuggestions([]);
            return;
        }

        const timeoutId = window.setTimeout(async () => {
            try {
                const result = await getSearchSuggestions(search.trim());
                setSuggestions(Array.isArray(result) ? result : []);
            } catch {
                setSuggestions([]);
            }
        }, 300);

        return () => window.clearTimeout(timeoutId);
    }, [search]);

    function toSlug(value: string){
        return value.toLowerCase()
        .trim().replace(/\s+/g, "-");
    }

    function handleSearch()
    {
        if(!search.trim()) return;
        setSuggestions([]);
        navigate(`/search/${toSlug(search)}`);
    }

    function handleSuggestionClick(suggestion: string)
    {
        setSearch(suggestion);
        setSuggestions([]);
        navigate(`/search/${toSlug(suggestion)}`);
    }

    return(
        <div className="flex items-center justify-between gap-5">
            <Link href="/feed">
                <img src="/cloudpins.png" className="h-10 min-w-10 md:h-12 md:min-w-12 rounded-xl"/>
            </Link>
            <Form onSubmit={handleSearch} className="relative flex w-full items-center justify-center">
                <Input 
                value={search}
                onValueChange={setSearch}
                placeholder="Pesquisar" 
                startContent={<IoSearch />} 
                className="w-full md:max-w-[600px] lg:max-w-[800px]" />

                {suggestions.length > 0 && (
                    <div className="absolute top-full z-50 mt-2 w-full max-w-[800px] overflow-hidden rounded-lg bg-content1 shadow-lg">
                        {suggestions.map((suggestion) => (
                            <button
                                key={suggestion}
                                type="button"
                                onClick={() => handleSuggestionClick(suggestion)}
                                className="block w-full px-4 py-3 text-left hover:bg-content2"
                            >
                                {suggestion}
                            </button>
                        ))}
                    </div>
                )}
            </Form>
            <div className="flex items-center gap-5">
                <ThemeSwitch />
                {
                    <Link href="/profile">
                        <img className="h-10 min-w-10 md:h-12 md:min-w-12 rounded-full" src={`http://localhost:5023${user?.profileUrl}`} />
                    </Link>
                }
            </div>
        </div>
    );
}
