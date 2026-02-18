type thumbnailUrl = 
{ 
    thumbnailUrl: string;
}

export type Board = 
{
    id: string;
    name: string;
    isPublic: boolean;
    lastPins: thumbnailUrl[];
    pinsCount: number;
    createdAt: string;
}