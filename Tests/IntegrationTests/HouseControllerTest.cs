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
    public class HouseControllerTest : ApiTest
    {
        private readonly string _apiHousesUrl;

        private static Dictionary<string, string> GenerateNamePatch(string name)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "name"},
                {"value", name}
            };
        }

        public HouseControllerTest()
        {
            _apiHousesUrl = $"{ApiUrl}/houses";
        }

        [Fact]
        public async Task GivenHouses_When_ThenGetAsyncShouldReturnOkResponse()
        {
            var response = await Client.GetAsync(_apiHousesUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenId_WhenNotExists_ThenGetAsyncShouldReturnNotFound()
        {
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            var response = await Client.GetAsync($"{_apiHousesUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenHouse_WhenEmpty_ThenPostAsyncShouldReturnHouse()
        {
            var response = await Client.PostAsJsonAsync(_apiHousesUrl, new House()
            {
                Name = "Apartment"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var house = await response.Content.ReadFromJsonAsync<House>();
            house.Should().NotBeNull();
            house?.Name.Should().Be("Apartment");
        }

        [Fact]
        public async Task GivenHouse_WhenNewName_ThenPatchAsyncShouldReturnUpdatedHouse()
        {
            var postResponse = await Client.PostAsJsonAsync(_apiHousesUrl, new House()
            {
                Name = "Apartment"
            });


            var house = await postResponse.Content.ReadFromJsonAsync<House>();
            if (house != null)
            {
                Guid id = house.Id;
                IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                patchList.Add(GenerateNamePatch("Lake house"));
                string serializedObject = JsonConvert.SerializeObject(patchList);
                HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var patchResponse = await Client.PatchAsync($"{_apiHousesUrl}/{id}", patchBody);

                patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                var updatedHouse = await patchResponse.Content.ReadFromJsonAsync<House>();
                updatedHouse.Should().NotBeNull();
                updatedHouse?.Name.Should().Be("Lake house");
            }
        }

        [Fact]
        public async Task GivenId_WhenExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            var postResponse = await Client.PostAsJsonAsync(_apiHousesUrl, new House()
            {
                Name = "Apartment"
            });

            var house = await postResponse.Content.ReadFromJsonAsync<House>();
            if (house != null)
            {
                Guid id = house.Id;

                var deleteResponse = await Client.DeleteAsync($"{_apiHousesUrl}/{id}");

                deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task GivenId_WhenNotExists_ThenDeleteAsyncShouldReturnNotFound()
        {
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            var response = await Client.DeleteAsync($"{_apiHousesUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}