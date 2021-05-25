using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetSubscriptionsAsync();
        Task<Subscription> GetSubscriptionByIdAsync(Guid id);
        Task<Subscription> CreateSubscriptionAsync(Subscription subscription);

        Task<Subscription> PartialUpdateSubscriptionAsync(Guid id, JsonPatchDocument<SubscriptionRequest>
            subscriptionPatch);

        Task<Subscription> DeleteSubscriptionAsync(Guid id);
    }
}