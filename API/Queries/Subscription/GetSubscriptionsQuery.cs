using System.Collections.Generic;
using MediatR;

namespace API.Queries.Subscription
{
    public class GetSubscriptionsQuery : IRequest<IEnumerable<Models.Subscription>>
    {
    }
}