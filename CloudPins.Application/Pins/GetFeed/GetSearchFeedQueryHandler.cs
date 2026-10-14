using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudPins.Application.Pins.GetFeed;

public class GetSearchFeedQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;
    private readonly ILogger<GetSearchFeedQueryHandler> _logger;

    public GetSearchFeedQueryHandler(
        IPinReadRepository pinReadRepository,
        ILogger<GetSearchFeedQueryHandler> logger)
    {
        _pinReadRepository = pinReadRepository;
        _logger = logger;
    }

    public Task<List<PinFeedItemDto>> Handle(
        SearchFeedQuery query,
        CancellationToken ct
    )
    {
        _logger.LogInformation(
            "Searching pins in PostgreSQL fallback. Page {Page}, PageSize {PageSize}",
            query.Page,
            query.PageSize);

        return _pinReadRepository.GetSearchFeed(
            query.Search,
            query.Page,
            query.PageSize,
            ct
        );
    }
}
