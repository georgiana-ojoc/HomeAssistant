using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Shared;
using Shared.Models;
using Shared.Requests;
using Xunit;

namespace Tests.RepositoryTests
{
    public class ScheduleRepositoryTest : BaseRepositoryTest
    {
        #region GET_SCHEDULE_ASYNC

        [Fact]
        public async void GivenEmail_WhenEmailExists_ThenGetSchedulesAsyncShouldReturnListOfSchedules()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            var result = await repository.GetSchedulesAsync("homeassistantgo@outlook.com");

            result.Should().BeOfType<List<Schedule>>();
        }

        [Fact]
        public async void GivenEmail_WhenEmailDoesNotExist_ThenGetSchedulesAsyncShouldReturnEmptyListOfSchedules()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            var result = await repository.GetSchedulesAsync("jane.doe@mail.com");

            result.Count().Should().Be(0);
        }

        [Fact]
        public async void GivenEmail_WhenEmailIsEmpty_ThenGetSchedulesAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            Func<Task> function = async () => { await repository.GetSchedulesAsync(null); };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Email cannot be empty.");
        }

        #endregion

        #region GET_SCHEDULE_BY_ID_ASYNC

        [Fact]
        public async void GivenSchedule_WhenScheduleExists_ThenGetScheduleByIdAsyncShouldReturnSchedule()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            var result = await repository.GetScheduleByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"));

            result.Should().BeOfType<Schedule>();
        }

        [Fact]
        public async void GivenSchedule_WhenScheduleDoesNotExist_ThenGetScheduleByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            var result = await repository.GetScheduleByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_SCHEDULE_ASYNC

        // [Fact]
        // public async void GivenNewSchedule_WhenScheduleIsNotEmpty_ThenCreateScheduleAsyncShouldReturnNewSchedule()
        // {
        //     await using HomeAssistantContext context = GetContextWithData();
        //     IMapper mapper = GetMapper();
        //     Helper helper = new Helper(context);
        //     ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);
        //
        //     var result = await repository.CreateScheduleAsync("homeassistantgo@outlook.com", new Schedule()
        //     {
        //         Name = "Day mode",
        //         Time = "08:00",
        //         Days = 127
        //     });
        //
        //     result.Should().BeOfType<Schedule>();
        // }

        [Fact]
        public async void GivenNewSchedule_WhenDaysIsZero_ThenCreateScheduleAsyncShouldThrowArgumentException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            Func<Task> function = async () =>
            {
                await repository.CreateScheduleAsync("homeassistantgo@outlook.com", new Schedule()
                {
                    Name = "Day mode",
                    Time = "08:00",
                    Days = 0
                });
            };

            await function.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Days cannot be 0 or bigger than 127.");
        }

        [Fact]
        public async void GivenNewSchedule_WhenNameExists_ThenCreateScheduleAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            Func<Task> function = async () =>
            {
                await repository.CreateScheduleAsync("homeassistantgo@outlook.com", new Schedule()
                {
                    Name = "Night mode",
                    Time = "08:00",
                    Days = 1
                });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a schedule with the specified name.");
        }
        
        #endregion

        #region PARTIAL_UPDATE_SCHEDULE_ASYNC

        // [Fact]
        // public async void
        //     GivenPartialUpdatedSchedule_WhenScheduleExists_ThenPartialUpdatedScheduleAsyncShouldReturnPartialUpdatedSchedule()
        // {
        //     await using HomeAssistantContext context = GetContextWithData();
        //     IMapper mapper = GetMapper();
        //     Helper helper = new Helper(context);
        //     ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);
        //
        //     JsonPatchDocument<ScheduleRequest> schedulePatch = new JsonPatchDocument<ScheduleRequest>();
        //     schedulePatch.Replace(s => s.Time, "07:00");
        //
        //     var result = await repository.PartialUpdateScheduleAsync("homeassistantgo@outlook.com",
        //         Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"), schedulePatch);
        //
        //     result.Should().BeOfType<Schedule>();
        //     result.Time.Should().Be("07:00");
        // }

        [Fact]
        public async void
            GivenPartialUpdatedSchedule_WhenScheduleDoesNotExist_ThenPartialUpdatedScheduleAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            JsonPatchDocument<ScheduleRequest> schedulePatch = new JsonPatchDocument<ScheduleRequest>();
            schedulePatch.Replace(s => s.Time, "07:00");

            var result = await repository.PartialUpdateScheduleAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), schedulePatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_SCHEDULE_ASYNC

        // [Fact]
        // public async void GivenSchedule_WhenScheduleExists_ThenDeleteScheduleAsyncShouldReturnSchedule()
        // {
        //     await using HomeAssistantContext context = GetContextWithData();
        //     IMapper mapper = GetMapper();
        //     Helper helper = new Helper(context);
        //     ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);
        //
        //     var result = await repository.DeleteScheduleAsync("homeassistantgo@outlook.com",
        //         Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"));
        //
        //     result.Should().BeOfType<Schedule>();
        // }

        [Fact]
        public async void GivenSchedule_WhenScheduleDoesNotExist_ThenDeleteScheduleAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            Helper helper = new Helper(context);
            ScheduleRepository repository = new ScheduleRepository(context, mapper, helper);

            var result = await repository.DeleteScheduleAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeNull();
        }

        #endregion
    }
}