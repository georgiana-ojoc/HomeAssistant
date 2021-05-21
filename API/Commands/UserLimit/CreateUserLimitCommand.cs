using MediatR;
using Shared.Requests;

namespace API.Commands.UserLimit
{
    public class CreateUserLimitCommand : IRequest<Shared.Models.UserLimit>
    {
        public UserLimitRequest Request { get; set; }
    }
}