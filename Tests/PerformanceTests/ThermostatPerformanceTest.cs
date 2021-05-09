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
    public class ThermostatPerformanceTest : BaseControllerTest
    {
        private readonly string _thermostatsApiUrl;

        public ThermostatPerformanceTest()
        {
            _thermostatsApiUrl = $"{GetApiUrl()}/houses/cae88006-a2d7-4dcd-93fc-0b561e1f1acc/rooms/" +
                                 "f6ed4eb2-ac66-429b-8199-8757888bb0ad/thermostats";
        }

        [Fact]
        public async Task GivenThermostats_WhenGetAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.GetAsync(_thermostatsApiUrl);
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostats_WhenGetAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.GetAsync(_thermostatsApiUrl);
                DateTime end = DateTime.Now;

                responseTimes.Add((start, end));
            }

            int expected = 5;
            int actual = (int) responseTimes.Select(time => (time.End - time.Start).TotalMilliseconds).Average();

            Assert.True(actual < expected,
                $"Expected milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostat_WhenPostAsync_ThenResponseTimeShouldBeLessThan1000MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);

            DateTime start = DateTime.Now;
            await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat()
            {
                Name = "Wall thermostat"
            });
            DateTime end = DateTime.Now;
            int expected = 1000;
            int actual = (int) (end - start).TotalMilliseconds;

            Assert.True(actual < expected,
                $"Expected total milliseconds of less than {expected}, but was {actual}.");
        }

        [Fact]
        public async Task GivenThermostat_WhenPostAsync_ThenAverageResponseTimeShouldBeLessThan5MilliSeconds()
        {
            using HttpClient client = GetClient(GetType().Name);
            List<(DateTime Start, DateTime End)> responseTimes = new List<(DateTime Start, DateTime End)>();

            for (int index = 0; index < 1000; index++)
            {
                DateTime start = DateTime.Now;
                await client.PostAsJsonAsync(_thermostatsApiUrl, new Thermostat()
                {
                    Name = "Wall thermostat"
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