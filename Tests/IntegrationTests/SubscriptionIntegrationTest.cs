using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests.IntegrationTests
{
    public class SubscriptionIntegrationTest : BaseControllerTest
    {
        private readonly string _subscriptionsApiUrl;

        private static Dictionary<string, string> GenerateNamePatch(string name)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "name"},
                {"value", name}
            };
        }

        public SubscriptionIntegrationTest()
        {
            _subscriptionsApiUrl = $"{GetApiUrl()}/subscriptions";
        }

        #region GET_SUBSCRIPTIONS

        [Fact]
        public async Task GivenSubscriptions_When_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_subscriptionsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_SUBSCRIPTION

        [Fact]
        public async Task GivenSubscription_WhenSubscriptionExists_ThenGetAsyncShouldReturnSubscription()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167");

            HttpResponseMessage response = await client.GetAsync($"{_subscriptionsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Subscription subscription = await response.Content.ReadFromJsonAsync<Subscription>();

            subscription.Should().NotBeNull();
            subscription?.Name.Should().Be("Basic subscription");
        }

        [Fact]
        public async Task GivenSubscription_WhenSubscriptionDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_subscriptionsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_SUBSCRIPTION

        [Fact]
        public async Task GivenNewSubscription_WhenSubscriptionContainsPrice_ThenPostAsyncShouldReturnSubscription()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_subscriptionsApiUrl, new Subscription()
            {
                Price = 100
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            Subscription subscription = await response.Content.ReadFromJsonAsync<Subscription>();

            subscription.Should().NotBeNull();
            subscription?.Price.Should().Be(100);
        }

        [Fact]
        public async Task GivenNewSubscription_WhenSubscriptionIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_subscriptionsApiUrl, new Subscription());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region PATCH_SUBSCRIPTION

        [Fact]
        public async Task
            GivenPatchedSubscription_WhenSubscriptionExists_ThenPatchAsyncShouldReturnPatchedSubscription()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateNamePatch("Premium subscription"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_subscriptionsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Subscription subscription = await response.Content.ReadFromJsonAsync<Subscription>();

            subscription.Should().NotBeNull();
            subscription?.Name.Should().Be("Premium subscription");
        }

        [Fact]
        public async Task
            GivenPatchedSubscription_WhenSubscriptionDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateNamePatch("Premium subscription"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_subscriptionsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_SUBSCRIPTION

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167");

            HttpResponseMessage response = await client.DeleteAsync($"{_subscriptionsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_subscriptionsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}