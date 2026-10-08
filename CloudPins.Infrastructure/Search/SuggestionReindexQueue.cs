using System.Threading.Channels;
using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CloudPins.Infrastructure.Search;

public sealed class SuggestionReindexQueue : BackgroundService, ISuggestionReindexQueue
{
    private readonly Channel<bool> _queue = Channel.CreateBounded<bool>(
        new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropWrite,
            SingleReader = true,
            SingleWriter = false
        });

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SuggestionReindexQueue> _logger;

    public SuggestionReindexQueue(
        IServiceScopeFactory scopeFactory,
        ILogger<SuggestionReindexQueue> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void Enqueue()
    {
        _queue.Writer.TryWrite(true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var _ in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider
                    .GetRequiredService<ISuggestionService>();

                await service.ReindexAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Could not reindex autocomplete suggestions in background.");
            }
        }
    }
}
