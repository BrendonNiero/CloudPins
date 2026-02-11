import DefaultLayout from "@/layouts/default";
import { Button } from "@heroui/button";
import { Card } from "@heroui/card";
import { User } from "@heroui/user";

export default function Profile()
{
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
                    {Array.from({ length: 3}).map((_, i) => (
                        <Card key={i} className="p-32">

                        </Card>
                    ))}
                </div>
            </section>
        </DefaultLayout>
    );
}