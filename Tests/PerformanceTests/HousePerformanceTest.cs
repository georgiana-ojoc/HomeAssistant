using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;
using Xunit;

namespace Tests.PerformanceTests
{
    public class HousePerformanceTest : BaseControllerTest
    {
        private readonly string _housesApiUrl;

        public HousePerformanceTest()
        {
            _housesApiUrl = $"{GetApiUrl()}/houses";
        }

        [Fact]
        public async Task GivenHouses_WhenGetAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.GetAsync(_housesApiUrl);
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenHouses_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.GetAsync(_housesApiUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenHouse_WhenPostAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.PostAsJsonAsync(_housesApiUrl, new House()
            {
                Name = "Apartment"
            });
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenHouse_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.PostAsJsonAsync(_housesApiUrl, new House()
                {
                    Name = "Apartment"
                });
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }
    }
}