import { apiFetch } from "./api";

export async function getBoards()
{
    return apiFetch("/boards");
}

export async function getPinsFromBoard(boardId: string)
{
    return apiFetch(`/boards/${boardId}`);
}