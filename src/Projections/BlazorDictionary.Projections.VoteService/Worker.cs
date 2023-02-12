using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Events.EntryComment;
using BlazorDictionary.Common.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace BlazorDictionary.Projections.VoteService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly IConfiguration configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connStr = configuration.GetConnectionString("SqlServer");

        var voteService = new Services.VoteService(connStr);

        QueueFactory.CreateBasicConsumer()
            .EnsureExchange(BlazorConstants.VoteExchangeName)
            .EnsureQueue(BlazorConstants.CreateEntryVoteQueueName, BlazorConstants.VoteExchangeName)
            .Receive<CreateEntryVoteEvent>(vote =>
            {
                voteService.CreateEntryVote(vote).GetAwaiter().GetResult();
                logger.LogInformation("Create Entry Received EntryId: {0}, VoteType: {1}", vote.EntryId, vote.VoteType);
            })
            .StartConsuming(BlazorConstants.CreateEntryVoteQueueName);

        QueueFactory.CreateBasicConsumer()
            .EnsureExchange(BlazorConstants.VoteExchangeName)
            .EnsureQueue(BlazorConstants.DeleteEntryVoteQueueName, BlazorConstants.VoteExchangeName)
            .Receive<DeleteEntryVoteEvent>(vote =>
            {
                voteService.DeleteEntryVote(vote.EntryId, vote.CreatedBy).GetAwaiter().GetResult();
                logger.LogInformation("Delete Entry Received EntryId: {0}", vote.EntryId);
            })
            .StartConsuming(BlazorConstants.DeleteEntryVoteQueueName);


        QueueFactory.CreateBasicConsumer()
                .EnsureExchange(BlazorConstants.VoteExchangeName)
                .EnsureQueue(BlazorConstants.CreateEntryCommentVoteQueueName, BlazorConstants.VoteExchangeName)
                .Receive<CreateEntryCommentVoteEvent>(vote =>
                {
                    voteService.CreateEntryCommentVote(vote).GetAwaiter().GetResult();
                    logger.LogInformation("Create Entry Comment Received EntryCommentId: {0}, VoteType: {1}", vote.EntryCommentId, vote.VoteType);
                })
                .StartConsuming(BlazorConstants.CreateEntryCommentVoteQueueName);

        QueueFactory.CreateBasicConsumer()
                .EnsureExchange(BlazorConstants.VoteExchangeName)
                .EnsureQueue(BlazorConstants.DeleteEntryCommentVoteQueueName, BlazorConstants.VoteExchangeName)
                .Receive<DeleteEntryCommentVoteEvent>(vote =>
                {
                    voteService.DeleteEntryCommentVote(vote.EntryCommentId, vote.CreatedBy).GetAwaiter().GetResult();
                    logger.LogInformation("Delete Entry Comment Received EntryCommentId: {0}", vote.EntryCommentId);
                })
                .StartConsuming(BlazorConstants.DeleteEntryCommentVoteQueueName);
    }
}