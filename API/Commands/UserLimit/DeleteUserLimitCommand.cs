using System;
using MediatR;

namespace API.Commands.UserLimit
{
    public class DeleteUserLimitCommand : IRequest<Shared.Models.UserLimit>
    {
        public Guid Id { get; set; }
    }
}