using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Pins.GetAll;

namespace CloudPins.Application.Pins.GetFeed;

public class GetPinsFeedQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;

    public GetPinsFeedQueryHandler(IPinReadRepository pinReadRepository)
    {
        _pinReadRepository = pinReadRepository;
    }

    public Task<List<PinFeedItemDto>> Handle(
        GetPinsFeedQuery query,
        Guid currenUserId,
        CancellationToken ct
    )
    {
        return _pinReadRepository.GetFeedAsync(
            currenUserId,
            query.Page,
            query.PageSize,
            ct
        );
    }
}