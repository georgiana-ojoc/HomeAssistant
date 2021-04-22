using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;
using Xunit;

namespace Tests.PerformanceTests
{
    public class HouseControllerTest : ApiTest
    {
        private readonly string _apiHousesUrl;

        public HouseControllerTest()
        {
            _apiHousesUrl = $"{ApiUrl}/houses";
        }

        [Fact]
        public async Task GivenHouses_WhenGetAsync_ThenResponseTimeShouldBeLessThan1500MilliSeconds()
        {
            DateTime start = DateTime.Now;
            await Client.GetAsync(_apiHousesUrl);
            DateTime end = DateTime.Now;

            int expected = 1500;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenHouses_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan50MilliSeconds()
        {
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 100; index++)
            {
                DateTime start = DateTime.Now;
                await Client.GetAsync(_apiHousesUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 50;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenHouse_WhenPostAsync_ThenResponseTimeShouldBeLessThan2000MilliSeconds()
        {
            DateTime start = DateTime.Now;
            await Client.PostAsJsonAsync(_apiHousesUrl, new House()
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
        public async Task GivenHouse_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan30MilliSeconds()
        {
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 100; index++)
            {
                DateTime start = DateTime.Now;
                await Client.PostAsJsonAsync(_apiHousesUrl, new House()
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