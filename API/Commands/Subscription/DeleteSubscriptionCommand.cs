using System;
using MediatR;

namespace API.Commands.Subscription
{
    public class DeleteSubscriptionCommand : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }
    }
}