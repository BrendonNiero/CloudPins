const API_URL = "http://localhost:5023"; // CloudPins.Api

export async function apiFetch(
    endpoint: string,
    options?: RequestInit
){
    const token = localStorage.getItem("token");

    const response = await fetch(`${API_URL}${endpoint}`, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...(token && { Authorization: `Bearer ${token}` }),
            ...options?.headers
        }
    });

    if(!response.ok) throw new Error("Erro na requisição");

    return response.json();
}