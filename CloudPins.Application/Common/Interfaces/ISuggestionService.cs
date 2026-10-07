namespace CloudPins.Application.Common.Interfaces;

public interface ISuggestionService
{
    Task<int> ReindexAsync(CancellationToken cancellationToken = default);
}
