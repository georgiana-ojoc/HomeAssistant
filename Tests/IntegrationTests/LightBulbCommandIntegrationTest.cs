using System;
using System.Collections.Generic;
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
    public class LightBulbCommandIntegrationTest : BaseControllerTest
    {
        private readonly string _lightBulbCommandsApiUrl;

        private static Dictionary<string, string> GenerateIntensityPatch(byte intensity)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "intensity"},
                {"value", intensity.ToString()}
            };
        }

        public LightBulbCommandIntegrationTest()
        {
            _lightBulbCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/light_bulb_commands";
        }

        #region GET_LIGHT_BULB_COMMANDS

        [Fact]
        public async Task GivenLightBulbCommands_WhenScheduleExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_lightBulbCommandsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_LIGHT_BULB_COMMAND

        [Fact]
        public async Task GivenLightBulbCommand_WhenLightBulbCommandExists_ThenGetAsyncShouldReturnLightBulbCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("31048472-de9f-427e-b7af-7a3416928652");

            HttpResponseMessage response = await client.GetAsync($"{_lightBulbCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            LightBulbCommand lightBulbCommand = await response.Content.ReadFromJsonAsync<LightBulbCommand>();

            lightBulbCommand.Should().NotBeNull();
            lightBulbCommand?.LightBulbId.Should().Be(Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"));
        }

        [Fact]
        public async Task
            GivenLightBulbCommand_WhenLightBulbCommandDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_lightBulbCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_LIGHT_BULB_COMMAND

        [Fact]
        public async Task
            GivenNewLightBulbCommand_WhenLightBulbCommandIsNotEmpty_ThenPostAsyncShouldReturnLightBulbCommand()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_lightBulbCommandsApiUrl,
                new LightBulbCommand()
                {
                    LightBulbId = Guid.Parse("0365f802-bb3a-487a-997b-0d34c270a385"),
                    Color = 6556210,
                    Intensity = 50
                });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            LightBulbCommand lightBulbCommand = await response.Content.ReadFromJsonAsync<LightBulbCommand>();

            lightBulbCommand.Should().NotBeNull();
            lightBulbCommand?.LightBulbId.Should().Be(Guid.Parse("0365f802-bb3a-487a-997b-0d34c270a385"));
        }

        [Fact]
        public async Task
            GivenNewLightBulbCommand_WhenLightBulbCommandIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_lightBulbCommandsApiUrl,
                new LightBulbCommand());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task
        //     GivenNewLightBulbCommand_WhenLightBulbCommandNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 9; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_lightBulbCommandsApiUrl,
        //             new LightBulbCommand()
        //             {
        //                 LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
        //                 Color = 6556210,
        //                 Intensity = 50
        //             });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_lightBulbCommandsApiUrl,
        //         new LightBulbCommand()
        //         {
        //             LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
        //             Color = 6556210,
        //             Intensity = 50
        //         });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_LIGHT_BULB_COMMAND

        [Fact]
        public async Task
            GivenPatchedLightBulbCommand_WhenLightBulbCommandExists_ThenPatchAsyncShouldReturnPatchedLightBulbCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("31048472-de9f-427e-b7af-7a3416928652");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateIntensityPatch(100));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_lightBulbCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            LightBulbCommand lightBulbCommand = await response.Content.ReadFromJsonAsync<LightBulbCommand>();

            lightBulbCommand.Should().NotBeNull();
            lightBulbCommand?.Intensity.Should().Be(100);
        }

        [Fact]
        public async Task
            GivenPatchedLightBulbCommand_WhenLightBulbCommandDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateIntensityPatch(100));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_lightBulbCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_LIGHT_BULB_COMMAND

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("31048472-de9f-427e-b7af-7a3416928652");

            HttpResponseMessage response = await client.DeleteAsync($"{_lightBulbCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_lightBulbCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}