using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Tags;

namespace CloudPins.Application.Tags.Create;

public class CreateTagCommandHandler
{
    private readonly ITagRepository _tagRepository;
    private readonly ITagReadRepository _tagReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTagCommandHandler(ITagRepository tagRepository, ITagReadRepository tagReadRepository, IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _tagReadRepository = tagReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTagResult> Handle(
        CreateTagCommand command,
        CancellationToken ct
    )
    {
        var exist = await _tagReadRepository.ExistsAsync(command.Name, ct);

        if(exist)
        {
            throw new ConflictException("This tag is already exists.");
        }
        var tag = new Tag(command.Name);

        await _tagRepository.AddAsync(tag, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new CreateTagResult
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }
}