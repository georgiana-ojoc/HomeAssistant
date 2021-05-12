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
    public class DoorCommandPerformanceTest : BaseControllerTest
    {
        private readonly string _doorCommandsApiUrl;

        public DoorCommandPerformanceTest()
        {
            _doorCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/door_commands";
        }

        [Fact]
        public async Task GivenDoorCommands_WhenGetAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.GetAsync(_doorCommandsApiUrl);
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenDoorCommands_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.GetAsync(_doorCommandsApiUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenDoorCommand_WhenPostAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.PostAsJsonAsync(_doorCommandsApiUrl, new DoorCommand()
            {
                DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
                Locked = true
            });
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenDoorCommand_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.PostAsJsonAsync(_doorCommandsApiUrl, new DoorCommand()
                {
                    DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
                    Locked = true
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