const API_URL = "http://localhost:5023"; // CloudPins.Api

export async function apiFetch(
    endpoint: string,
    options?: RequestInit
){
    const token = localStorage.getItem("token");

    const isFormData = options?.body instanceof FormData;

    try {
        const response = await fetch(`${API_URL}${endpoint}`, {
        ...options,
        headers: {
            ...(token && { Authorization: `Bearer ${token}` }),
            ...(!isFormData && { "Content-Type": "application/json" }),
            ...options?.headers
        }
    });

    if(!response.ok){
        let message = "Erro inesperado.";
        switch(response.status)
        {
            case 400:
                message = "Requisição inválida";
                break;
            case 401:
                message = "Acesso negado."
                break;
            case 403:
                message = "Acesso negado."
                break;
            case 404:
                message = "Recurso não encontrado."
                break;
            case 500:
                message = "Erro interno do servidor."
                break;
        }
        throw {
            status: response.status,
            message,
        };

    }
    if(response.status === 204) return null;

    return await response.json();
    }
    catch(error: any)
    {
        if(!error.status){
            throw {
                status: 0,
                message: "Não foi possível conectar com a API."
            };
        }
        throw error;
    }
}