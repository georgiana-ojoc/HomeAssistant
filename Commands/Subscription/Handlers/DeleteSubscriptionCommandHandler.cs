using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Subscription.Handlers
{
    public class DeleteSubscriptionCommandHandler : Handler, IRequestHandler<DeleteSubscriptionCommand,
        Models.Subscription>
    {
        private readonly ISubscriptionRepository _repository;

        public DeleteSubscriptionCommandHandler(Identity identity, ISubscriptionRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Subscription> Handle(DeleteSubscriptionCommand request, CancellationToken
            cancellationToken)
        {
            CheckEmail();

            return await _repository.DeleteSubscriptionAsync(request.Id);
        }
    }
}