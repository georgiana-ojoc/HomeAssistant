using System;
using API.Requests;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Commands.Subscription
{
    public class PartialUpdateSubscriptionCommand : IRequest<Models.Subscription>
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<SubscriptionRequest> Patch { get; set; }
    }
}