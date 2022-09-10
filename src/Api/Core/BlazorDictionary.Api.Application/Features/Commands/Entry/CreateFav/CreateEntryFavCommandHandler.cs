using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.Entry;
using BlazorDictionary.Common.Infrastructure;
using MediatR;

namespace BlazorDictionary.Api.Application.Features.Commands.Entry.CreateFav;
public class CreateEntryFavCommandHandler : IRequestHandler<CreateEntryFavCommand, bool>
{

    public Task<bool> Handle(CreateEntryFavCommand request, CancellationToken cancellationToken)
    {
        QueueFactory.SendMessageToExchange(exchangeName: BlazorConstants.FavExchangeName,
            exchangeType: BlazorConstants.DefaultExchangeType,
            queueName: BlazorConstants.CreateEntryFavQueueName,
            obj: new CreateEntryFavEvent()
            {
                EntryId = request.EntryId.Value,
                CreatedBy = request.UserId.Value
            });

        return Task.FromResult(true);
    }
}