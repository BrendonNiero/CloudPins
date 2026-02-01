using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Likes;

namespace CloudPins.Application.Pins.LikePin;

public class LikePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LikePinCommandHandler(
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

        var alreadyLiked = await _likeRepository
            .ExistsAsync(userId, pinId);

        if(alreadyLiked) return;

        var like = new Like(userId, pinId);
        
        await _likeRepository.AddAsync(like);
        pin.AddLike();

        await _unitOfWork.SaveChangesAsync(ct);
    }
}