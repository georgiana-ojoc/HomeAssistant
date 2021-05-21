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
    public class DoorIntegrationTest : BaseControllerTest
    {
        private readonly string _doorsApiUrl;

        private static Dictionary<string, string> GenerateLockedPatch(bool locked)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "locked"},
                {"value", locked.ToString()}
            };
        }

        public DoorIntegrationTest()
        {
            _doorsApiUrl = $"{GetApiUrl()}/houses/cae88006-a2d7-4dcd-93fc-0b561e1f1acc/rooms/" +
                           "f6ed4eb2-ac66-429b-8199-8757888bb0ad/doors";
        }

        #region GET_DOORS

        [Fact]
        public async Task GivenDoors_WhenRoomExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_doorsApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_DOOR

        [Fact]
        public async Task GivenDoor_WhenDoorExists_ThenGetAsyncShouldReturnDoor()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba");

            HttpResponseMessage response = await client.GetAsync($"{_doorsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Door door = await response.Content.ReadFromJsonAsync<Door>();

            door.Should().NotBeNull();
            door?.Name.Should().Be("Balcony door");
        }

        [Fact]
        public async Task GivenDoor_WhenDoorDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_doorsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_DOOR

        [Fact]
        public async Task GivenNewDoor_WhenDoorIsNotEmpty_ThenPostAsyncShouldReturnDoor()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_doorsApiUrl, new Door()
            {
                Name = "Door"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            Door door = await response.Content.ReadFromJsonAsync<Door>();

            door.Should().NotBeNull();
            door?.Name.Should().Be("Door");
        }

        [Fact]
        public async Task GivenNewDoor_WhenDoorIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_doorsApiUrl, new Door());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task
        //     GivenNewDoor_WhenDoorNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 9; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_doorsApiUrl, new Door()
        //         {
        //             Name = "Balcony door"
        //         });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_doorsApiUrl, new Door()
        //     {
        //         Name = "Balcony door"
        //     });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_DOOR

        [Fact]
        public async Task GivenPatchedDoor_WhenDoorExists_ThenPatchAsyncShouldReturnPatchedDoor()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLockedPatch(true));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_doorsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Door door = await response.Content.ReadFromJsonAsync<Door>();

            door.Should().NotBeNull();
            door?.Locked.Should().Be(true);
        }

        [Fact]
        public async Task GivenPatchedDoor_WhenDoorDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLockedPatch(true));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_doorsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_DOOR

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba");

            HttpResponseMessage response = await client.DeleteAsync($"{_doorsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_doorsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}