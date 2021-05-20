using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Shared.Models;
using Xunit;

namespace Tests.IntegrationTests
{
    public class ThermostatIntegrationTest : BaseControllerTest
    {
        private readonly string _thermostatsApiUrl;

        private static Dictionary<string, string> GenerateTemperaturePatch(decimal temperature)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "temperature"},
                {"value", temperature.ToString(CultureInfo.InvariantCulture)}
            };
        }

        public ThermostatIntegrationTest()
        {
            _thermostatsApiUrl = $"{GetApiUrl()}/houses/cae88006-a2d7-4dcd-93fc-0b561e1f1acc/rooms/" +
                                 "f6ed4eb2-ac66-429b-8199-8757888bb0ad/thermostats";
        }

        #region GET_THERMOSTATS

        [Fact]
        public async Task GivenThermostats_WhenRoomExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_thermostatsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_THERMOSTAT

        [Fact]
        public async Task GivenThermostat_WhenThermostatExists_ThenGetAsyncShouldReturnThermostat()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e");

            HttpResponseMessage response = await client.GetAsync($"{_thermostatsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Thermostat thermostat = await response.Content.ReadFromJsonAsync<Thermostat>();

            thermostat.Should().NotBeNull();
            thermostat?.Name.Should().Be("Wall thermostat");
        }

        [Fact]
        public async Task GivenThermostat_WhenThermostatDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_thermostatsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_THERMOSTAT

        [Fact]
        public async Task GivenNewThermostat_WhenThermostatIsNotEmpty_ThenPostAsyncShouldReturnThermostat()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat()
            {
                Name = "Wireless thermostat"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            Thermostat thermostat = await response.Content.ReadFromJsonAsync<Thermostat>();

            thermostat.Should().NotBeNull();
            thermostat?.Name.Should().Be("Wireless thermostat");
        }

        [Fact]
        public async Task GivenNewThermostat_WhenThermostatIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task
        //     GivenNewThermostat_WhenThermostatNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 9; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat()
        //         {
        //             Name = "Wall thermostat"
        //         });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat()
        //     {
        //         Name = "Wall thermostat"
        //     });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_THERMOSTAT

        [Fact]
        public async Task GivenPatchedThermostat_WhenThermostatExists_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTemperaturePatch((decimal) 12.5));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_thermostatsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Thermostat thermostat = await response.Content.ReadFromJsonAsync<Thermostat>();

            thermostat.Should().NotBeNull();
            thermostat?.Temperature.Should().Be((decimal) 12.5);
        }

        [Fact]
        public async Task
            GivenPatchedThermostat_WhenThermostatDoesNotExist_ThenPatchAsyncShouldReturnPatchedThermostat()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTemperaturePatch((decimal) 12.5));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_thermostatsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_THERMOSTAT

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e");

            HttpResponseMessage response = await client.DeleteAsync($"{_thermostatsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_thermostatsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}