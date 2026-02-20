import { apiFetch } from "./api";

export async function getProfile()
{
    return apiFetch("/user");
}

export async function updateProfile(name: string, profileImage: ImageData)
{
    return apiFetch("/user", { method: "PUT", body: JSON.stringify(name)})
}