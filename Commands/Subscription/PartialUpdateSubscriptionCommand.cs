using System;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Commands.Subscription
{
    public class PartialUpdateSubscriptionCommand : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<SubscriptionRequest> Patch { get; set; }
    }
}