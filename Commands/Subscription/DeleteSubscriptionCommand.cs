using System;
using MediatR;

namespace HomeAssistantAPI.Commands.Subscription
{
    public class DeleteSubscriptionCommand : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }
    }
}