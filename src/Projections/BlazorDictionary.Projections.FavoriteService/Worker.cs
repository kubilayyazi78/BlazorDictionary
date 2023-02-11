using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Infrastructure;

namespace BlazorDictionary.Projections.FavoriteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger , IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connStr = _configuration.GetConnectionString("SqlServer");

            var favService = new Services.FavoriteService(connStr);

            QueueFactory.CreateBasicConsumer()
                 .EnsureExchange(BlazorConstants.FavExchangeName)
                 .EnsureQueue(BlazorConstants.CreateEntryFavQueueName, BlazorConstants.FavExchangeName)
                 .Receive<CreateEntryFavEvent>(fav =>
                 {
                     favService.CreateEntryFav(fav).GetAwaiter().GetResult();
                     _logger.LogInformation($"Received EntryId {fav.EntryId}");
                 }).StartConsuming(BlazorConstants.CreateEntryFavQueueName);
        }
    }
}