using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Pins.GetByBoard;

namespace CloudPins.Application.Pins.UnlikePin;

public class UnlikePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnlikePinCommandHandler(
        IPinRepository pinRepository,
        ILikeRepository likeRepository,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _likeRepository = likeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(Guid pinId, Guid userId, CancellationToken ct)
    {
        var pin = await _pinRepository.GetByIdAsync(pinId, ct);
        if(pin is null)
            throw new NotFoundException("Pin not found.");

        var liked = await _likeRepository.ExistsAsync(userId, pinId);
        if(!liked) return;

        await _likeRepository.RemoveAsync(userId, pinId);
        pin.RemoveLike();

        await _unitOfWork.SaveChangesAsync(ct);
    }
}