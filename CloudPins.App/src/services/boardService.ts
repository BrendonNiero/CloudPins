import { apiFetch } from "./api";

export async function getBoards()
{
    return apiFetch("/boards");
}