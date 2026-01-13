using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Tags;

namespace CloudPins.Application.Tags.Create;

public class CreateTagCommandHandler
{
    private readonly ITagRepository _tagRepository;

    public CreateTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<CreateTagResult> Handle(
        CreateTagCommand command,
        CancellationToken ct
    )
    {
        var tag = new Tag(command.Name);

        await _tagRepository.AddAsync(tag, ct);

        return new CreateTagResult
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }
}