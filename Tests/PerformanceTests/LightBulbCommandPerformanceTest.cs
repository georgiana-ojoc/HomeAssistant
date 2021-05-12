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
    public class LightBulbCommandPerformanceTest : BaseControllerTest
    {
        private readonly string _lightBulbCommandsApiUrl;

        public LightBulbCommandPerformanceTest()
        {
            _lightBulbCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/light_bulb_commands";
        }

        [Fact]
        public async Task GivenLightBulbCommands_WhenGetAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.GetAsync(_lightBulbCommandsApiUrl);
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenLightBulbCommands_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.GetAsync(_lightBulbCommandsApiUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenLightBulbCommand_WhenPostAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.PostAsJsonAsync(_lightBulbCommandsApiUrl, new LightBulbCommand()
            {
                LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
                Color = 6556210,
                Intensity = 50
            });
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenLightBulbCommand_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.PostAsJsonAsync(_lightBulbCommandsApiUrl, new LightBulbCommand()
                {
                    LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
                    Color = 6556210,
                    Intensity = 50
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