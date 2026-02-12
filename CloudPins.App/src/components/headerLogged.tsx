import { Input } from "@heroui/input";
import { ThemeSwitch } from "./theme-switch";
import { Link } from "@heroui/link";
import { IoSearch } from "react-icons/io5";


export default function HeaderLogged()
{
    return(
        <div className="flex items-center justify-between">
            <Link href="/feed">
                <img src="/cloudpins.png" className="h-12 w-12 rounded-xl"/>
            </Link>
            <Input placeholder="Pesquisar" startContent={<IoSearch />} className="max-w-[800px]" />
            <div className="flex items-center gap-5">
                <ThemeSwitch />
                <Link href="/profile">
                    <img className="h-12 rounded-full" src="https://i.pinimg.com/736x/31/52/59/31525978bcee3d8f47f11f78be4df9c6.jpg" />
                </Link>
            </div>
        </div>
    );
}