using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Subscription.Handlers
{
    public class PartialUpdateSubscriptionCommandHandler : Handler,
        IRequestHandler<PartialUpdateSubscriptionCommand, Models.Subscription>
    {
        private readonly ISubscriptionRepository _repository;

        public PartialUpdateSubscriptionCommandHandler(Identity identity, ISubscriptionRepository repository) :
            base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Subscription> Handle(PartialUpdateSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            CheckEmail();

            return await _repository.PartialUpdateSubscriptionAsync(request.Id, request.Patch);
        }
    }
}