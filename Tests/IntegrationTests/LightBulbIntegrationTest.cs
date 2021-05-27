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
    public class LightBulbIntegrationTest : BaseControllerTest
    {
        private readonly string _lightBulbsApiUrl;

        private static Dictionary<string, string> GenerateIntensityPatch(byte intensity)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "intensity"},
                {"value", intensity.ToString()}
            };
        }

        public LightBulbIntegrationTest()
        {
            _lightBulbsApiUrl = $"{GetApiUrl()}/houses/cae88006-a2d7-4dcd-93fc-0b561e1f1acc/rooms/" +
                                "f6ed4eb2-ac66-429b-8199-8757888bb0ad/light_bulbs";
        }

        #region GET_LIGHT_BULBS

        [Fact]
        public async Task GivenLightBulbs_WhenRoomExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_lightBulbsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_LIGHT_BULB

        [Fact]
        public async Task GivenLightBulb_WhenLightBulbExists_ThenGetAsyncShouldReturnLightBulb()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a");

            HttpResponseMessage response = await client.GetAsync($"{_lightBulbsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            LightBulb lightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();

            lightBulb.Should().NotBeNull();
            lightBulb?.Name.Should().Be("Lamp");
        }

        [Fact]
        public async Task GivenLightBulb_WhenLightBulbDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_lightBulbsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_LIGHT_BULB

        [Fact]
        public async Task GivenNewLightBulb_WhenLightBulbIsNotEmpty_ThenPostAsyncShouldReturnLightBulb()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_lightBulbsApiUrl, new LightBulb()
            {
                Name = "Chandelier"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            LightBulb lightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();

            lightBulb.Should().NotBeNull();
            lightBulb?.Name.Should().Be("Chandelier");
        }

        [Fact]
        public async Task GivenNewLightBulb_WhenLightBulbIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_lightBulbsApiUrl, new LightBulb());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task
            GivenNewLightBulb_WhenLightBulbNumberHasBeenReached_ThenPostAsyncShouldReturnPaymentRequiredStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            await client.PostAsJsonAsync(_lightBulbsApiUrl, new LightBulb()
            {
                Name = "Chandelier"
            });

            HttpResponseMessage response = await client.PostAsJsonAsync(_lightBulbsApiUrl, new LightBulb()
            {
                Name = "Light bulb"
            });

            response.StatusCode.Should().Be(HttpStatusCode.PaymentRequired);
        }

        #endregion

        #region PATCH_LIGHT_BULB

        [Fact]
        public async Task GivenPatchedLightBulb_WhenLightBulbExists_ThenPatchAsyncShouldReturnPatchedLightBulb()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateIntensityPatch(255));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_lightBulbsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            LightBulb lightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();

            lightBulb.Should().NotBeNull();
            lightBulb?.Intensity.Should().Be(255);
        }

        [Fact]
        public async Task GivenPatchedLightBulb_WhenLightBulbDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateIntensityPatch(255));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_lightBulbsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_LIGHT_BULB

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a");

            HttpResponseMessage response = await client.DeleteAsync($"{_lightBulbsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_lightBulbsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}