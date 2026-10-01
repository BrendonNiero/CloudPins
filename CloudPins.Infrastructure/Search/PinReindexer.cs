using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Pins;
using CloudPins.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Search;

public sealed class PinReindexer
{
    private readonly CloudPinsDbContext _context;
    private readonly IPinSearchService _pinSearchService;

    public PinReindexer(
        CloudPinsDbContext context,
        IPinSearchService pinSearchService)
    {
        _context = context;
        _pinSearchService = pinSearchService;
    }

    public async Task<int> ReindexAsync(
        CancellationToken ct = default)
    {
        var pins = await _context.Pins
            .AsNoTracking()
            .ToListAsync(ct);

        var pinTags = await _context.Set<PinTag>()
            .AsNoTracking()
            .ToListAsync(ct);

        var tags = await _context.Tags
            .AsNoTracking()
            .ToDictionaryAsync(
                tag => tag.Id,
                tag => tag.Name,
                ct);

        var tagNamesByPinId = pinTags
            .GroupBy(pinTag => pinTag.PinId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Where(pinTag => tags.ContainsKey(pinTag.TagId))
                    .Select(pinTag => tags[pinTag.TagId])
                    .ToArray());

        foreach (var pin in pins)
        {
            var tagNames = tagNamesByPinId.TryGetValue(
                pin.Id,
                out var names)
                ? names
                : Array.Empty<string>();

            await _pinSearchService.IndexAsync(
                pin,
                tagNames,
                ct);
        }

        return pins.Count;
    }
}