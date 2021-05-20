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
    public class DoorCommandIntegrationTest : BaseControllerTest
    {
        private readonly string _doorCommandsApiUrl;

        private static Dictionary<string, string> GenerateLockedPatch(bool locked)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "locked"},
                {"value", locked.ToString()}
            };
        }

        public DoorCommandIntegrationTest()
        {
            _doorCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/door_commands";
        }

        #region GET_DOOR_COMMANDS

        [Fact]
        public async Task GivenDoorCommands_WhenScheduleExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_doorCommandsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_DOOR_COMMAND

        [Fact]
        public async Task GivenDoorCommand_WhenDoorCommandExists_ThenGetAsyncShouldReturnDoorCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b");

            HttpResponseMessage response = await client.GetAsync($"{_doorCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DoorCommand doorCommand = await response.Content.ReadFromJsonAsync<DoorCommand>();

            doorCommand.Should().NotBeNull();
            doorCommand?.DoorId.Should().Be(Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"));
        }

        [Fact]
        public async Task
            GivenDoorCommand_WhenDoorCommandDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_doorCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_DOOR_COMMAND

        [Fact]
        public async Task
            GivenNewDoorCommand_WhenDoorCommandIsNotEmpty_ThenPostAsyncShouldReturnDoorCommand()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_doorCommandsApiUrl,
                new DoorCommand()
                {
                    DoorId = Guid.Parse("3968c3e5-daee-4096-a6d4-11b640216591"),
                    Locked = true
                });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            DoorCommand doorCommand = await response.Content.ReadFromJsonAsync<DoorCommand>();

            doorCommand.Should().NotBeNull();
            doorCommand?.DoorId.Should().Be(Guid.Parse("3968c3e5-daee-4096-a6d4-11b640216591"));
        }

        [Fact]
        public async Task
            GivenNewDoorCommand_WhenDoorCommandIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_doorCommandsApiUrl,
                new DoorCommand());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task
        //     GivenNewDoorCommand_WhenDoorCommandNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 9; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_doorCommandsApiUrl,
        //             new DoorCommand()
        //             {
        //                 DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
        //                 Locked = true
        //             });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_doorCommandsApiUrl,
        //         new DoorCommand()
        //         {
        //             DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
        //             Locked = true
        //         });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_DOOR_COMMAND

        [Fact]
        public async Task
            GivenPatchedDoorCommand_WhenDoorCommandExists_ThenPatchAsyncShouldReturnPatchedDoorCommand()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLockedPatch(false));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_doorCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DoorCommand doorCommand = await response.Content.ReadFromJsonAsync<DoorCommand>();

            doorCommand.Should().NotBeNull();
            doorCommand?.Locked.Should().Be(false);
        }

        [Fact]
        public async Task
            GivenPatchedDoorCommand_WhenDoorCommandDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLockedPatch(false));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_doorCommandsApiUrl}/{id}",
                patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_DOOR_COMMAND

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b");

            HttpResponseMessage response = await client.DeleteAsync($"{_doorCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_doorCommandsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}