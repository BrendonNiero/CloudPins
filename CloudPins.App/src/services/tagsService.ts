import { apiFetch } from "./api";

export async function getTags()
{
    return apiFetch("/tags");
}