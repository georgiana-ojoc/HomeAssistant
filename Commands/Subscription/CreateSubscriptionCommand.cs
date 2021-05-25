using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.Subscription
{
    public class CreateSubscriptionCommand : IRequest<Models.Subscription>
    {
        public SubscriptionRequest Request { get; set; }
    }
}