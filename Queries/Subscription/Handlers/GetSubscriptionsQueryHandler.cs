using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Subscription.Handlers
{
    public class GetSubscriptionsQueryHandler : Handler, IRequestHandler<GetSubscriptionsQuery,
        IEnumerable<Models.Subscription>>
    {
        private readonly ISubscriptionRepository _repository;

        public GetSubscriptionsQueryHandler(Identity identity, ISubscriptionRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Subscription>> Handle(GetSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetSubscriptionsAsync();
        }
    }
}