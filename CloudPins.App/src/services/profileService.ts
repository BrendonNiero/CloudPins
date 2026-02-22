import { apiFetch } from "./api";

export async function getProfile()
{
    return apiFetch("/user");
}

export async function updateProfile(formData: FormData)
{
    return apiFetch("/user", { method: "PUT", body: formData })
}