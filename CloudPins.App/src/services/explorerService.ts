import { apiFetch } from "./api";

export async function getFeedExplorer(id: string, page: number, pageSize: number)
{
    return apiFetch(`/pins/feed/${id}?page=${page}&pageSize=${pageSize}`);
}