using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Subscription.Handlers
{
    public class GetSubscriptionByIdQueryHandler : Handler, IRequestHandler<GetSubscriptionByIdQuery,
        Models.Subscription>
    {
        private readonly ISubscriptionRepository _repository;

        public GetSubscriptionByIdQueryHandler(Identity identity, ISubscriptionRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Subscription> Handle(GetSubscriptionByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetSubscriptionByIdAsync(request.Id);
        }
    }
}