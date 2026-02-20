import { apiFetch } from "./api";

export async function likePin(pinId: string)
{
    return apiFetch(`/pins/${pinId}/like`, { method: "POST" });
}