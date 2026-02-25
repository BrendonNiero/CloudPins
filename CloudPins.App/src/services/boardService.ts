import { apiFetch } from "./api";

export async function getBoards()
{
    return apiFetch("/boards");
}

export async function getPinsFromBoard(boardId: string)
{
    return apiFetch(`/boards/${boardId}`);
}

export async function createBoard(data: { name: string, isPublic: boolean })
{
    return apiFetch("/boards",{
        method: "POST",
        body: JSON.stringify(data)
    }
    );
}