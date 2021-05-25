using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace HomeAssistantAPI.Repositories
{
    public class ScheduleRepository : BaseRepository, IScheduleRepository
    {
        private readonly Helper _helper;

        private static void CheckTime(string time)
        {
            if (!TimeSpan.TryParse(time, CultureInfo.InvariantCulture, out _))
            {
                throw new ArgumentException("Time is not valid.");
            }
        }

        private static void CheckDays(byte days)
        {
            if (days is 0 or > 127)
            {
                throw new ArgumentException("Days cannot be 0 or bigger than 127.");
            }
        }

        public ScheduleRepository(HomeAssistantContext context, IMapper mapper, Helper helper) : base(context, mapper)
        {
            _helper = helper;
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesAsync(string email)
        {
            CheckString(email, "email");

            return await Context.Schedules.Where(schedule => schedule.Email == email).ToListAsync();
        }

        public async Task<Schedule> GetScheduleByIdAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            await _helper.ChangeLightBulbsAsync(id);

            return await GetScheduleInternalAsync(email, id);
        }

        public async Task<Schedule> CreateScheduleAsync(string email, Schedule schedule)
        {
            CheckString(email, "email");
            CheckString(schedule.Name, "name");
            CheckTime(schedule.Time);
            CheckDays(schedule.Days);

            int limit = await GetLimit("schedules", email);
            int schedules = await Context.Schedules.CountAsync(s => s.Email == email);
            if (schedules >= limit)
            {
                throw new ConstraintException(nameof(CreateScheduleAsync));
            }

            int schedulesByEmailAndName = await Context.Schedules.CountAsync(s => s.Email == email &&
                s.Name == schedule.Name);
            if (schedulesByEmailAndName > 0)
            {
                throw new DuplicateNameException("You already have a schedule with the specified name.");
            }

            schedule.Email = email;
            schedule.Time = TimeSpan.Parse(schedule.Time).ToString(@"hh\:mm");
            Schedule newSchedule = (await Context.Schedules.AddAsync(schedule)).Entity;
            await Context.SaveChangesAsync();

            RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => _helper.Change(schedule.Id),
                Helper.GetCronExpression(schedule.Time, schedule.Days), TimeZoneInfo.Local);

            return newSchedule;
        }

        public async Task<Schedule> PartialUpdateScheduleAsync(string email, Guid id,
            JsonPatchDocument<ScheduleRequest> schedulePatch)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            Schedule schedule = await GetScheduleInternalAsync(email, id);
            if (schedule == null)
            {
                return null;
            }

            ScheduleRequest scheduleToPatch = Mapper.Map<ScheduleRequest>(schedule);
            schedulePatch.ApplyTo(scheduleToPatch);
            CheckString(scheduleToPatch.Name, "name");
            CheckTime(schedule.Time);
            CheckDays(schedule.Days);
            schedule.Time = TimeSpan.Parse(schedule.Time).ToString(@"hh\:mm");

            Mapper.Map(scheduleToPatch, schedule);
            await Context.SaveChangesAsync();

            RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => _helper.Change(schedule.Id),
                Helper.GetCronExpression(schedule.Time, schedule.Days), TimeZoneInfo.Local);

            return schedule;
        }

        public async Task<Schedule> DeleteScheduleAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            Schedule schedule = await GetScheduleInternalAsync(email, id);
            if (schedule == null)
            {
                return null;
            }

            Context.Schedules.Remove(schedule);
            await Context.SaveChangesAsync();

            RecurringJob.RemoveIfExists(schedule.Id.ToString());

            return schedule;
        }
    }
}