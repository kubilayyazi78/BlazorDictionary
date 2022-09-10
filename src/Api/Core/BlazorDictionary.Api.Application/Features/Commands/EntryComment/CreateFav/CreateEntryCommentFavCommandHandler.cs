using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.EntryComment;
using BlazorDictionary.Common.Infrastructure;
using MediatR;

namespace BlazorDictionary.Api.Application.Features.Commands.EntryComment.CreateFav;

public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand, bool>
{

    public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName: BlazorConstants.FavExchangeName,
            exchangeType: BlazorConstants.DefaultExchangeType,
            queueName: BlazorConstants.CreateEntryCommentFavQueueName,
            obj: new CreateEntryCommentFavEvent()
            {
                CreatedBy = request.UserId,
                EntryCommentId = request.EntryCommentId
            });

        return await Task.FromResult(true);
    }
}
