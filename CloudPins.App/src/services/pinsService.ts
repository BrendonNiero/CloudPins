import { apiFetch } from "./api";

export async function getFeed(page: number, pageSize: number)
{
    return apiFetch(`/pins/feed?page=${page}&pageSize=${pageSize}`);
}

export async function getSearchFeed(search: string)
{
    return apiFetch
        (`/search/${encodeURIComponent(search)}`);
}

export async function createPin(formData: FormData)
{
    return apiFetch("/pins", { method: "POST", body: formData });
}

export async function getSearchSuggestions(search: string)
{
    return apiFetch(
        `/search/autocomplete?query=${encodeURIComponent(search)}`
    );
}