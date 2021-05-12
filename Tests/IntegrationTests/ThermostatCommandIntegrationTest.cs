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
    public class ThermostatCommandIntegrationTest : BaseControllerTest
    {
        private readonly string _thermostatCommandsApiUrl;

        private static Dictionary<string, string> GenerateTemperaturePatch(decimal temperature)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "temperature"},
                {"value", temperature.ToString(CultureInfo.InvariantCulture)}
            };
        }

        public ThermostatCommandIntegrationTest()
        {
            _thermostatCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/thermostat_commands";
        }

        #region GET_THERMOSTAT_COMMANDS

        [Fact]
        public async Task GivenThermostatCommands_WhenScheduleExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_thermostatCommandsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_THERMOSTAT_COMMAND

        [Fact]
        public async Task GivenThermostatCommand_WhenThermostatCommandExists_ThenGetAsyncShouldReturnThermostatCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04");

            HttpResponseMessage response = await client.GetAsync($"{_thermostatCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ThermostatCommand thermostatCommand = await response.Content.ReadFromJsonAsync<ThermostatCommand>();

            thermostatCommand.Should().NotBeNull();
            thermostatCommand?.ThermostatId.Should().Be(Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"));
        }

        [Fact]
        public async Task
            GivenThermostatCommand_WhenThermostatCommandDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_thermostatCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_THERMOSTAT_COMMAND

        [Fact]
        public async Task
            GivenNewThermostatCommand_WhenThermostatCommandIsNotEmpty_ThenPostAsyncShouldReturnThermostatCommand()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_thermostatCommandsApiUrl,
                new ThermostatCommand()
                {
                    ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    Temperature = (decimal) 22.5
                });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            ThermostatCommand thermostatCommand = await response.Content.ReadFromJsonAsync<ThermostatCommand>();

            thermostatCommand.Should().NotBeNull();
            thermostatCommand?.ThermostatId.Should().Be(Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"));
        }

        [Fact]
        public async Task
            GivenNewThermostatCommand_WhenThermostatCommandIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_thermostatCommandsApiUrl,
                new ThermostatCommand());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task
            GivenNewThermostatCommand_WhenThermostatCommandNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            for (int index = 0; index < 9; index++)
            {
                HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_thermostatCommandsApiUrl,
                    new ThermostatCommand()
                    {
                        ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                        Temperature = (decimal) 22.5
                    });

                createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_thermostatCommandsApiUrl,
                new ThermostatCommand()
                {
                    ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    Temperature = (decimal) 22.5
                });

            forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion

        #region PATCH_THERMOSTAT_COMMAND

        [Fact]
        public async Task
            GivenPatchedThermostatCommand_WhenThermostatCommandExists_ThenPatchAsyncShouldReturnPatchedThermostatCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTemperaturePatch((decimal) 20.5));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_thermostatCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ThermostatCommand thermostatCommand = await response.Content.ReadFromJsonAsync<ThermostatCommand>();

            thermostatCommand.Should().NotBeNull();
            thermostatCommand?.Temperature.Should().Be((decimal) 20.5);
        }

        [Fact]
        public async Task
            GivenPatchedThermostatCommand_WhenThermostatCommandDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTemperaturePatch((decimal) 20.5));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_thermostatCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_THERMOSTAT_COMMAND

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04");

            HttpResponseMessage response = await client.DeleteAsync($"{_thermostatCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_thermostatCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}