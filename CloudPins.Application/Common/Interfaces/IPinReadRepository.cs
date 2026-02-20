using CloudPins.Application.Pins.GetByBoard;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinReadRepository
{
    Task<PinDetailsDto?> GetByIdAsync(
        Guid pinId, 
        Guid currentUserId,
        CancellationToken ct
    );

    Task<List<PinFeedItemDto>> GetFeedAsync(
        int page,
        int pageSize,
        CancellationToken ct
    );

    Task<List<PinFeedItemDto>> GetFeedByPinAsync(
        Guid pinId,
        int page,
        int pageSize,
        CancellationToken ct
    );

    Task<List<PinBoardItemDto>> GetByBoardAsync(
        Guid boardId,
        int page,
        int pageSize,
        CancellationToken ct
    );
}