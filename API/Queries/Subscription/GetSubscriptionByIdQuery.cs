using System;
using MediatR;

namespace API.Queries.Subscription
{
    public class GetSubscriptionByIdQuery : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }
    }
}