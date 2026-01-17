using CloudPins.Application.Pins.GetByBoard;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinReadRepository
{
    Task<PinDetailsDto?> GetByIdAsync(Guid pinId, CancellationToken ct);

    Task<List<PinFeedItemDto>> GetFeedAsync(
        Guid userId,
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