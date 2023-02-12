using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Events.EntryComment;
using BlazorDictionary.Common.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace BlazorDictionary.Projections.FavoriteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
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
                })
                .StartConsuming(BlazorConstants.CreateEntryFavQueueName);

            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(BlazorConstants.FavExchangeName)
                .EnsureQueue(BlazorConstants.DeleteEntryFavQueueName, BlazorConstants.FavExchangeName)
                .Receive<DeleteEntryFavEvent>(fav =>
                {
                    favService.DeleteEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Received EntryId {fav.EntryId}");
                })
                .StartConsuming(BlazorConstants.DeleteEntryFavQueueName);



            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(BlazorConstants.FavExchangeName)
                .EnsureQueue(BlazorConstants.CreateEntryCommentFavQueueName, BlazorConstants.FavExchangeName)
                .Receive<CreateEntryCommentFavEvent>(fav =>
                {
                    favService.CreateEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Create EntryComment Received EntryCommentId {fav.EntryCommentId}");
                })
                .StartConsuming(BlazorConstants.CreateEntryCommentFavQueueName);


            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(BlazorConstants.FavExchangeName)
                .EnsureQueue(BlazorConstants.DeleteEntryCommentFavQueueName, BlazorConstants.FavExchangeName)
                .Receive<DeleteEntryCommentFavEvent>(fav =>
                {
                    favService.DeleteEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Received EntryCommentId {fav.EntryCommentId}");
                })
                .StartConsuming(BlazorConstants.DeleteEntryCommentFavQueueName);
        }
    }
}