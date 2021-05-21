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
    public class HouseIntegrationTest : BaseControllerTest
    {
        private readonly string _housesApiUrl;

        private static Dictionary<string, string> GenerateNamePatch(string name)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "name"},
                {"value", name}
            };
        }

        public HouseIntegrationTest()
        {
            _housesApiUrl = $"{GetApiUrl()}/houses";
        }

        #region GET_HOUSES

        [Fact]
        public async Task GivenHouses_WhenUserExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_housesApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_HOUSE

        [Fact]
        public async Task GivenHouse_WhenHouseExists_ThenGetAsyncShouldReturnHouse()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc");

            HttpResponseMessage response = await client.GetAsync($"{_housesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            House house = await response.Content.ReadFromJsonAsync<House>();

            house.Should().NotBeNull();
            house?.Name.Should().Be("Apartment");
        }

        [Fact]
        public async Task GivenHouse_WhenHouseDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_housesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_HOUSE

        [Fact]
        public async Task GivenNewHouse_WhenHouseIsNotEmpty_ThenPostAsyncShouldReturnHouse()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_housesApiUrl, new House()
            {
                Name = "Lake house"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            House house = await response.Content.ReadFromJsonAsync<House>();

            house.Should().NotBeNull();
            house?.Name.Should().Be("Lake house");
        }

        [Fact]
        public async Task GivenNewHouse_WhenHouseIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_housesApiUrl, new House());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Fact]
        // public async Task GivenNewHouse_WhenHouseNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        // {
        //     using HttpClient client = GetClient(GetType().Name);
        //
        //     for (int index = 0; index < 4; index++)
        //     {
        //         HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_housesApiUrl, new House()
        //         {
        //             Name = "Apartment"
        //         });
        //
        //         createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        //     }
        //
        //     HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_housesApiUrl, new House()
        //     {
        //         Name = "Apartment"
        //     });
        //
        //     forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        // }

        #endregion

        #region PATCH_HOUSE

        [Fact]
        public async Task GivenPatchedHouse_WhenHouseExists_ThenPatchAsyncShouldReturnPatchedHouse()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateNamePatch("Lake house"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_housesApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            House house = await response.Content.ReadFromJsonAsync<House>();

            house.Should().NotBeNull();
            house?.Name.Should().Be("Lake house");
        }

        [Fact]
        public async Task GivenPatchedHouse_WhenHouseDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateNamePatch("Lake house"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_housesApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_HOUSE

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc");

            HttpResponseMessage response = await client.DeleteAsync($"{_housesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_housesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}