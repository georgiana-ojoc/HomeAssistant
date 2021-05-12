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
    public class ThermostatCommandPerformanceTest : BaseControllerTest
    {
        private readonly string _thermostatCommandsApiUrl;

        public ThermostatCommandPerformanceTest()
        {
            _thermostatCommandsApiUrl =
                $"{GetApiUrl()}/schedules/377a7b7b-2b63-4317-bff6-e52ef5eb51da/thermostat_commands";
        }

        [Fact]
        public async Task GivenThermostatCommands_WhenGetAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.GetAsync(_thermostatCommandsApiUrl);
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostatCommands_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.GetAsync(_thermostatCommandsApiUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostatCommand_WhenPostAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.PostAsJsonAsync(_thermostatCommandsApiUrl, new ThermostatCommand()
            {
                ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                Temperature = (decimal) 22.5
            });
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostatCommand_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.PostAsJsonAsync(_thermostatCommandsApiUrl, new ThermostatCommand()
                {
                    ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    Temperature = (decimal) 22.5
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