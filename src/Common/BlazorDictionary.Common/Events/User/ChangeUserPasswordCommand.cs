﻿using MediatR;

namespace BlazorDictionary.Common.Events.User;

public class ChangeUserPasswordCommand : IRequest<bool>
{
    public Guid? UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public ChangeUserPasswordCommand(string newPassword, string oldPassword, Guid? userId)
    {
        NewPassword = newPassword;
        OldPassword = oldPassword;
        UserId = userId;
    }
}
