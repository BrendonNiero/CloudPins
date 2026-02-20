export type PinDetail = 
{
    id: string;
    boardId: string;
    ownerId: string;
    imageUrl: string;
    thumbnailUrl: string;
    title: string;
    description: string;
    tagIds: string[];
    likesCount: number;
    isLiked: boolean;
    createdAt: string;
};