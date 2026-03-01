import { apiFetch } from "./api";

export async function login(email: string, password: string) {
    return apiFetch("/auth/login", {
        method: "POST",
        body: JSON.stringify({ email, password })
    });
}

export async function createProfile(formData: FormData)
{
    return apiFetch("/auth/register", { method: "POST", body: formData });
}
