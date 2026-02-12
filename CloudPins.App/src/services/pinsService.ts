import { apiFetch } from "./api";

export async function getFeed()
{
    return apiFetch("/pins/feed");
}