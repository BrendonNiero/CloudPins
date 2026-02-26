using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.GetFeed;

public class GetSearchFeedQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;

    public GetSearchFeedQueryHandler(IPinReadRepository pinReadRepository)
    {
        _pinReadRepository = pinReadRepository;
    }

    public Task<List<PinFeedItemDto>> Handle(
        SearchFeedQuery query,
        CancellationToken ct
    )
    {
        return _pinReadRepository.GetSearchFeed(
            query.Search,
            query.Page,
            query.PageSize,
            ct
        );
    }
}