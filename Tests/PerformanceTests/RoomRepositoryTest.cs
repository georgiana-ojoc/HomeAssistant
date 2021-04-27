using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;
using Xunit;

namespace Tests.PerformanceTests
{
    public class RoomRepositoryTest : ApiTest
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

        public RoomRepositoryTest()
        {
            _apiHousesUrl = $"{ApiUrl}/houses";
        }

        [Fact]
        public async Task GivenRooms_WhenGetAsync_ThenResponseTimeShouldBeLessThan1500MilliSeconds()
        {
            Guid id = await GetHouseId();

            DateTime start = DateTime.Now;
            await Client.GetAsync($"{_apiHousesUrl}/{id}/rooms");
            DateTime end = DateTime.Now;

            int expected = 1500;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenRooms_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan50MilliSeconds()
        {
            Guid id = await GetHouseId();
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 100; index++)
            {
                DateTime start = DateTime.Now;
                await Client.GetAsync($"{_apiHousesUrl}/{id}/rooms");
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 50;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenRoom_WhenPostAsync_ThenResponseTimeShouldBeLessThan2000MilliSeconds()
        {
            Guid id = await GetHouseId();

            DateTime start = DateTime.Now;
            await Client.PostAsJsonAsync($"{_apiHousesUrl}/{id}/rooms", new House()
            {
                Name = "Apartment"
            });
            DateTime end = DateTime.Now;

            int expected = 2000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenRoom_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan30MilliSeconds()
        {
            Guid id = await GetHouseId();
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 100; index++)
            {
                DateTime start = DateTime.Now;
                await Client.PostAsJsonAsync($"{_apiHousesUrl}/{id}/rooms", new House()
                {
                    Name = "Apartment"
                });
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 30;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }
    }
}