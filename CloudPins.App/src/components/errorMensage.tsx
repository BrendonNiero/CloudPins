type ErrorMensageProps = 
{
    error: string;
}

export default function ErrorMensage({ error }: ErrorMensageProps )
{
    return(
        <section className="h-full text-center mt-64 w-full flex items-center justify-center">
            <h3 className="text-4xl">{error}</h3>
        </section>
    );
}