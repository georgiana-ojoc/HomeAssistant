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
    public class RoomIntegrationTest : BaseControllerTest
    {
        private readonly string _roomsApiUrl;

        private static Dictionary<string, string> GenerateNamePatch(string name)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "name"},
                {"value", name}
            };
        }

        public RoomIntegrationTest()
        {
            _roomsApiUrl = $"{GetApiUrl()}/houses/cae88006-a2d7-4dcd-93fc-0b561e1f1acc/rooms";
        }

        #region GET_ROOMS

        // [Fact]
        // public async Task GivenRooms_WhenHouseExists_ThenGetAsyncShouldReturnOkStatusCode()
        // {
        //     using HttpClient client = await GetClientAsync(GetType().Name);
        //
        //     HttpResponseMessage response = await client.GetAsync(_roomsApiUrl);
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.OK);
        // }

        #endregion

        #region GET_ROOM

        // [Fact]
        // public async Task GivenRoom_WhenRoomExists_ThenGetAsyncShouldReturnRoom()
        // {
        //     using HttpClient client = await GetClientAsync(GetType().Name);
        //     Guid id = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad");
        //
        //     HttpResponseMessage response = await client.GetAsync($"{_roomsApiUrl}/{id}");
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.OK);
        //
        //     Room room = await response.Content.ReadFromJsonAsync<Room>();
        //
        //     room.Should().NotBeNull();
        //     room?.Name.Should().Be("Kitchen");
        // }

        [Fact]
        public async Task GivenRoom_WhenRoomDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_roomsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_ROOM

        // [Fact]
        // public async Task GivenNewRoom_WhenRoomIsNotEmpty_ThenPostAsyncShouldReturnRoom()
        // {
        //     using HttpClient client = await GetClientAsync(GetType().Name);
        //
        //     HttpResponseMessage response = await client.PostAsJsonAsync(_roomsApiUrl, new Room()
        //     {
        //         Name = "Bathroom"
        //     });
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.Created);
        //
        //     Room room = await response.Content.ReadFromJsonAsync<Room>();
        //
        //     room.Should().NotBeNull();
        //     room?.Name.Should().Be("Bathroom");
        // }

        [Fact]
        public async Task GivenNewRoom_WhenRoomIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_roomsApiUrl, new Room());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task GivenNewRoom_WhenRoomNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = await GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 19; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_roomsApiUrl, new Room()
        //         {
        //             Name = "Kitchen"
        //         });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_roomsApiUrl, new Room()
        //     {
        //         Name = "Kitchen"
        //     });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_ROOM

        // [Fact]
        // public async Task GivenPatchedRoom_WhenRoomExists_ThenPatchAsyncShouldReturnPatchedRoom()
        // {
        //     using HttpClient client = await GetClientAsync(GetType().Name);
        //     Guid id = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad");
        //     IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
        //     patchList.Add(GenerateNamePatch("Bathroom"));
        //     string serializedObject = JsonConvert.SerializeObject(patchList);
        //     HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //
        //     HttpResponseMessage response = await client.PatchAsync($"{_roomsApiUrl}/{id}", patchBody);
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.OK);
        //
        //     Room room = await response.Content.ReadFromJsonAsync<Room>();
        //
        //     room.Should().NotBeNull();
        //     room?.Name.Should().Be("Bathroom");
        // }

        [Fact]
        public async Task GivenPatchedRoom_WhenRoomDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateNamePatch("Bathroom"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_roomsApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_ROOM

        // [Fact]
        // public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        // {
        //     using HttpClient client = await GetClientAsync(GetType().Name);
        //     Guid id = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad");
        //
        //     HttpResponseMessage response = await client.DeleteAsync($"{_roomsApiUrl}/{id}");
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        // }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = await GetClientAsync(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_roomsApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}