using BlazorDictionary.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Common.Models.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public LoginUserCommand()
        {

        }
        public LoginUserCommand(string password, string emailAddress)
        {
            Password = password;
            EmailAddress = emailAddress;
        }
      
    }
}
