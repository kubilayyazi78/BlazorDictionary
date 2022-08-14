using AutoMapper;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common;
using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.Infrastructure;
using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Application.Features.Commands.User.Update;

public class UpdateUserCommandHandle : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;

    public UpdateUserCommandHandle(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await userRepository.GetByIdAsync(request.Id);

        var dbEmailAddress = dbUser.EmailAddress;

        var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

        if (dbUser is null)
        {
            throw new DatabaseValidationException("User not found");
        }
        mapper.Map(request, dbUser);

        var rows = await userRepository.UpdateAsync(dbUser);

        if (emailChanged && rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = dbEmailAddress,
                NewEmailAddress = request.EmailAddress
            };

            QueueFactory.SendMessageToExchange(exchangeName: BlazorConstants.UserExchangeName,
                exchangeType: BlazorConstants.DefaultExchangeType,
                queueName: BlazorConstants.UserEmailChangedQueueName,
                obj: @event);

            dbUser.EmailConfirmed = false;
            await userRepository.UpdateAsync(dbUser);
        }

        return dbUser.Id;
    }
}
