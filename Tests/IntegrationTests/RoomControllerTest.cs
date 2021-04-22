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
    public class RoomControllerTest : ApiTest
    {
        private readonly string _apiHousesUrl;

        private async Task<Guid> GetHouseId()
        {
            var postResponse = await Client.PostAsJsonAsync(_apiHousesUrl, new House()
            {
                Name = "Apartment"
            });

            var house = await postResponse.Content.ReadFromJsonAsync<House>();
            return house?.Id ?? Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
        }

        private static Dictionary<string, string> GenerateNamePatch(string name)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "name"},
                {"value", name}
            };
        }

        public RoomControllerTest()
        {
            _apiHousesUrl = $"{ApiUrl}/houses";
        }

        [Fact]
        public async Task GivenRooms_WhenNotExist_ThenGetAsyncShouldReturnEmptyList()
        {
            Guid id = await GetHouseId();

            var response = await Client.GetAsync($"{_apiHousesUrl}/{id}/rooms");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<List<Room>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GivenId_WhenNotExists_ThenGetAsyncShouldReturnNotFound()
        {
            Guid houseId = await GetHouseId();
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            var response = await Client.GetAsync($"{_apiHousesUrl}/{houseId}/rooms/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenRoom_WhenEmpty_ThenPostAsyncShouldReturnHouse()
        {
            Guid id = await GetHouseId();

            var response = await Client.PostAsJsonAsync($"{_apiHousesUrl}/{id}/rooms", new Room()
            {
                Name = "Kitchen"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var room = await response.Content.ReadFromJsonAsync<Room>();
            room.Should().NotBeNull();
            room?.Name.Should().Be("Kitchen");
        }

        [Fact]
        public async Task GivenRoom_WhenNewName_ThenPatchAsyncShouldReturnUpdatedRoom()
        {
            Guid houseId = await GetHouseId();
            var postResponse = await Client.PostAsJsonAsync($"{_apiHousesUrl}/{houseId}/rooms", new Room()
            {
                Name = "Kitchen"
            });


            var room = await postResponse.Content.ReadFromJsonAsync<Room>();
            if (room != null)
            {
                Guid id = room.Id;
                IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                patchList.Add(GenerateNamePatch("Bathroom"));
                string serializedObject = JsonConvert.SerializeObject(patchList);
                HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var patchResponse = await Client.PatchAsync($"{_apiHousesUrl}/{houseId}/rooms/{id}", patchBody);

                patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                var updatedRoom = await patchResponse.Content.ReadFromJsonAsync<Room>();
                updatedRoom.Should().NotBeNull();
                updatedRoom?.Name.Should().Be("Bathroom");
            }
        }

        [Fact]
        public async Task GivenId_WhenExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            Guid houseId = await GetHouseId();
            var postResponse = await Client.PostAsJsonAsync($"{_apiHousesUrl}/{houseId}/rooms", new Room()
            {
                Name = "Kitchen"
            });


            var room = await postResponse.Content.ReadFromJsonAsync<Room>();
            if (room != null)
            {
                Guid id = room.Id;
                var deleteResponse = await Client.DeleteAsync($"{_apiHousesUrl}/{houseId}/rooms/{id}");

                deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task GivenId_WhenNotExists_ThenDeleteAsyncShouldReturnNotFound()
        {
            Guid houseId = await GetHouseId();
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            var response = await Client.DeleteAsync($"{_apiHousesUrl}/{houseId}/rooms/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}