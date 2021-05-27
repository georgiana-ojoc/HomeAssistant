using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API;
using API.Models;
using API.Repositories;
using API.Requests;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace Tests.RepositoryTests
{
    public class SubscriptionRepositoryTest : BaseRepositoryTest
    {
        #region GET_SUBSCRIPTIONS_ASYNC

        [Fact]
        public async void GivenSubscriptions_When_ThenGetSubscriptionsAsyncShouldReturnListOfSubscriptions()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.GetSubscriptionsAsync();

            result.Should().BeOfType<List<Subscription>>();
        }

        #endregion

        #region GET_SUBSCRIPTION_BY_ID_ASYNC

        [Fact]
        public async void
            GivenSubscription_WhenSubscriptionExists_ThenGetSubscriptionByIdAsyncShouldReturnSubscription()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.GetSubscriptionByIdAsync(Guid
                .Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167"));

            result.Should().BeOfType<Subscription>();
        }

        [Fact]
        public async void GivenSubscription_WhenSubscriptionDoesNotExist_ThenGetSubscriptionByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.GetSubscriptionByIdAsync(Guid
                .Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_SUBSCRIPTION_ASYNC

        [Fact]
        public async void
            GivenNewSubscription_WhenSubscriptionIsNotEmpty_ThenCreateSubscriptionAsyncShouldReturnNewSubscription()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.CreateSubscriptionAsync(new Subscription()
            {
                Price = 100
            });

            result.Should().BeOfType<Subscription>();
        }

        [Fact]
        public async void
            GivenNewSubscription_WhenSubscriptionIsEmpty_ThenCreateSubscriptionAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            Func<Task> function = async () => { await repository.CreateSubscriptionAsync(new Subscription()); };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Price should be bigger than 0.");
        }

        [Fact]
        public async void
            GivenNewSubscription_WhenNameExists_ThenCreateSubscriptionAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateSubscriptionAsync(new Subscription()
                {
                    Name = "Basic subscription",
                    Price = 100
                });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a subscription with the specified name.");
        }

        #endregion

        #region PARTIAL_UPDATE_SUBSCRIPTION_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedSubscription_WhenSubscriptionExists_ThenPartialUpdatedSubscriptionAsyncShouldReturnPartialUpdatedSubscription()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            JsonPatchDocument<SubscriptionRequest> subscriptionPatch = new JsonPatchDocument<SubscriptionRequest>();
            subscriptionPatch.Replace(s => s.Price, 1000);

            var result = await repository.PartialUpdateSubscriptionAsync(Guid
                .Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167"), subscriptionPatch);

            result.Should().BeOfType<Subscription>();
            result.Price.Should().Be(1000);
        }

        [Fact]
        public async void
            GivenPartialUpdatedSubscription_WhenSubscriptionDoesNotExist_ThenPartialUpdatedSubscriptionAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            JsonPatchDocument<SubscriptionRequest> subscriptionPatch = new JsonPatchDocument<SubscriptionRequest>();
            subscriptionPatch.Replace(s => s.Price, 1000);

            var result = await repository.PartialUpdateSubscriptionAsync(Guid
                .Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), subscriptionPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_SUBSCRIPTION_ASYNC

        [Fact]
        public async void GivenSubscription_WhenSubscriptionExists_ThenDeleteSubscriptionAsyncShouldReturnSubscription()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.DeleteSubscriptionAsync(Guid
                .Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167"));

            result.Should().BeOfType<Subscription>();
        }

        [Fact]
        public async void GivenSubscription_WhenSubscriptionDoesNotExist_ThenDeleteSubscriptionAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = await GetContextWithData();
            IMapper mapper = GetMapper();
            SubscriptionRepository repository = new SubscriptionRepository(context, mapper);

            var result = await repository.DeleteSubscriptionAsync(Guid
                .Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeNull();
        }

        #endregion
    }
}