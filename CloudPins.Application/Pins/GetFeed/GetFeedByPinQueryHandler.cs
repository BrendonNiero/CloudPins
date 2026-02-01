using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.GetFeed;

public class GetFeedByPinQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;

    public GetFeedByPinQueryHandler(IPinReadRepository pinReadRepository)
    {
        _pinReadRepository = pinReadRepository;
    }

    public Task<List<PinFeedItemDto>> Handle(
        FeedByPinQuery query,
        CancellationToken ct
    )
    {
        return _pinReadRepository.GetFeedByPinAsync(
            query.PinId,
            query.Page,
            query.PageSize,
            ct
        );
    }
}