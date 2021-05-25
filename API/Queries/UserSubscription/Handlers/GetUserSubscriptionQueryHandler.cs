using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.UserSubscription.Handlers
{
    public class GetUserSubscriptionQueryHandler : Handler, IRequestHandler<GetUserSubscriptionQuery,
        Models.UserSubscription>
    {
        private readonly IUserSubscriptionRepository _repository;

        public GetUserSubscriptionQueryHandler(Identity identity, IUserSubscriptionRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.UserSubscription> Handle(GetUserSubscriptionQuery request, CancellationToken
            cancellationToken)
        {
            return await _repository.GetUserSubscriptionAsync(Identity.Email);
        }
    }
}