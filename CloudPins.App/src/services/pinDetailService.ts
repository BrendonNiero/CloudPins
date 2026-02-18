import { apiFetch } from "./api";

export async function getPinDetail(id: string)
{
    return apiFetch(`/pins/${id}`);
}