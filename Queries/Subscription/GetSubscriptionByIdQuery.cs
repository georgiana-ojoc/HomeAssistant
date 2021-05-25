using System;
using MediatR;

namespace HomeAssistantAPI.Queries.Subscription
{
    public class GetSubscriptionByIdQuery : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }
    }
}