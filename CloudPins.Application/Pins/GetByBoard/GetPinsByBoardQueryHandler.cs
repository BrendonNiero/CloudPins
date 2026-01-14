using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.GetByBoard;

public class GetPinsByBoardQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;

    public GetPinsByBoardQueryHandler(IPinReadRepository pinReadRepository)
    {
        _pinReadRepository = pinReadRepository;
    }

    public Task<List<PinBoardItemDto>> Handle(
        GetPinsByBoardQuery query,
        CancellationToken ct
    )
    {
        return _pinReadRepository.GetByBoardAsync(query.BoardId, ct);
    }
}