import { apiFetch } from "./api";

export async function unlikePin(pinId: string)
{
    return apiFetch(`/pins/${pinId}/like`, { method: "DELETE"});
}