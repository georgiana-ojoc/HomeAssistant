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
    public class ScheduleIntegrationTest : BaseControllerTest
    {
        private readonly string _schedulesApiUrl;

        private static Dictionary<string, string> GenerateTimePatch(string time)
        {
            return new()
            {
                {"operation", "replace"},
                {"path", "time"},
                {"value", time}
            };
        }

        public ScheduleIntegrationTest()
        {
            _schedulesApiUrl = $"{GetApiUrl()}/schedules";
        }

        #region GET_SCHEDULES

        [Fact]
        public async Task GivenSchedules_WhenUserExists_ThenGetAsyncShouldReturnOkStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.GetAsync(_schedulesApiUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region GET_SCHEDULE

        [Fact]
        public async Task GivenSchedule_WhenScheduleExists_ThenGetAsyncShouldReturnSchedule()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da");

            HttpResponseMessage response = await client.GetAsync($"{_schedulesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Schedule schedule = await response.Content.ReadFromJsonAsync<Schedule>();

            schedule.Should().NotBeNull();
            schedule?.Name.Should().Be("Night mode");
        }

        [Fact]
        public async Task GivenSchedule_WhenScheduleDoesNotExist_ThenGetAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.GetAsync($"{_schedulesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST_SCHEDULE

        [Fact]
        public async Task GivenNewSchedule_WhenScheduleIsNotEmpty_ThenPostAsyncShouldReturnSchedule()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_schedulesApiUrl, new Schedule()
            {
                Name = "Night mode",
                Time = "22:00",
                Days = 127
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            Schedule schedule = await response.Content.ReadFromJsonAsync<Schedule>();

            schedule.Should().NotBeNull();
            schedule?.Name.Should().Be("Night mode");
        }

        [Fact]
        public async Task GivenNewSchedule_WhenScheduleIsEmpty_ThenPostAsyncShouldReturnBadRequestStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            HttpResponseMessage response = await client.PostAsJsonAsync(_schedulesApiUrl, new Schedule());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task
            GivenNewSchedule_WhenScheduleNumberHasBeenReached_ThenPostAsyncShouldReturnForbiddenStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);

            for (int index = 0; index < 19; index++)
            {
                HttpResponseMessage createdResponse = await client.PostAsJsonAsync(_schedulesApiUrl, new Schedule()
                {
                    Name = "Night mode",
                    Time = "22:00",
                    Days = 127
                });

                createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            }

            HttpResponseMessage forbiddenResponse = await client.PostAsJsonAsync(_schedulesApiUrl, new Schedule()
            {
                Name = "Night mode",
                Time = "22:00",
                Days = 127
            });

            forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion

        #region PATCH_SCHEDULE

        [Fact]
        public async Task GivenPatchedSchedule_WhenScheduleExists_ThenPatchAsyncShouldReturnPatchedSchedule()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTimePatch("23:00"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_schedulesApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Schedule schedule = await response.Content.ReadFromJsonAsync<Schedule>();

            schedule.Should().NotBeNull();
            schedule?.Time.Should().Be("23:00");
        }

        [Fact]
        public async Task GivenPatchedSchedule_WhenScheduleDoesNotExist_ThenPatchAsyncShouldReturnNotFoundStatusCode()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateTimePatch("23:00"));
            string serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PatchAsync($"{_schedulesApiUrl}/{id}", patchBody);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE_SCHEDULE

        [Fact]
        public async Task GivenId_WhenIdExists_ThenDeleteAsyncShouldReturnNoContent()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da");

            HttpResponseMessage response = await client.DeleteAsync($"{_schedulesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenId_WhenIdDoesNotExist_ThenDeleteAsyncShouldReturnNotFound()
        {
            using HttpClient client = GetClient(GetType().Name);
            Guid id = Guid.Parse("a918cdd5-b15b-4d04-9839-8a74e676dfea");

            HttpResponseMessage response = await client.DeleteAsync($"{_schedulesApiUrl}/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}