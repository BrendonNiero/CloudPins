import DefaultLayout from "@/layouts/default";
import { Link } from "@heroui/link";
import {Skeleton} from "@heroui/skeleton";

export default function Feed()
{
    return(
        <DefaultLayout>
            <ul className="w-full flex items-center justify-center gap-5">
                <Link href="/feed" underline="always" color="foreground">Todos</Link>
                <Link href="/feed" underline="hover" color="foreground">Anime</Link>
                <Link href="/feed" underline="hover" color="foreground">Tecnologia</Link>
                <Link href="/feed" underline="hover" color="foreground">UI/UX</Link>
            </ul>
            <section className="flex justify-center gap-5 flex-wrap mt-5">
                {Array.from({ length: 100}).map((_, i) => (
                    <Skeleton key={i} className="rounded-lg p-48">
                    </Skeleton>
                ))}
            </section>
        </DefaultLayout>
    );
}