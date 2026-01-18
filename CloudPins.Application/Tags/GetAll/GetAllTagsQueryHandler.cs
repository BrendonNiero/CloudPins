using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Tags.GetAll;

public class GetAllTagsQueryHanddler
{
    private readonly ITagReadRepository _tagReadRepository;

    public GetAllTagsQueryHanddler(ITagReadRepository tagReadRepository)
    {
        _tagReadRepository = tagReadRepository;
    }

    public async Task<List<TagDto>> Handle(
        CancellationToken ct
    )
    {
        return await _tagReadRepository.GetAllAsync(ct);
    }
}