using System.Collections.Generic;
using MediatR;

namespace HomeAssistantAPI.Queries.Subscription
{
    public class GetSubscriptionsQuery : IRequest<IEnumerable<Models.Subscription>>
    {
    }
}