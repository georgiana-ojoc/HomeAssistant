using API.Requests;
using MediatR;

namespace API.Commands.Subscription
{
    public class CreateSubscriptionCommand : IRequest<Models.Subscription>
    {
        public SubscriptionRequest Request { get; set; }
    }
}