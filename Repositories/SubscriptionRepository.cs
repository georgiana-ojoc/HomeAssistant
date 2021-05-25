using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace HomeAssistantAPI.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private static void CheckPrice(int price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price should be bigger than 0.");
            }
        }

        private async Task<Subscription> GetSubscriptionInternalAsync(Guid id)
        {
            return await Context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
        {
            return await Context.Subscriptions.ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(Guid id)
        {
            CheckGuid(id, "id");

            return await GetSubscriptionInternalAsync(id);
        }

        public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
        {
            CheckPrice(subscription.Price);

            int subscriptionsByName = await Context.Subscriptions.CountAsync(s => s.Name == subscription.Name);
            if (subscriptionsByName > 0)
            {
                throw new DuplicateNameException("You already have a subscription with the specified name.");
            }

            Subscription newSubscription = (await Context.Subscriptions.AddAsync(subscription)).Entity;
            await Context.SaveChangesAsync();
            return newSubscription;
        }

        public async Task<Subscription> PartialUpdateSubscriptionAsync(Guid id,
            JsonPatchDocument<SubscriptionRequest> subscriptionPatch)
        {
            CheckGuid(id, "id");

            Subscription subscription = await GetSubscriptionInternalAsync(id);
            if (subscription == null)
            {
                return null;
            }

            SubscriptionRequest subscriptionToPatch = Mapper.Map<SubscriptionRequest>(subscription);
            subscriptionPatch.ApplyTo(subscriptionToPatch);
            CheckPrice(subscriptionToPatch.Price);

            Mapper.Map(subscriptionToPatch, subscription);
            await Context.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription> DeleteSubscriptionAsync(Guid id)
        {
            CheckGuid(id, "id");

            Subscription subscription = await GetSubscriptionInternalAsync(id);
            if (subscription == null)
            {
                return null;
            }

            Context.Subscriptions.Remove(subscription);
            await Context.SaveChangesAsync();
            return subscription;
        }
    }
}