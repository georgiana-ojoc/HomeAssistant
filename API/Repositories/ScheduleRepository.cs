using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Shared.Requests;

namespace API.Repositories
{
    public class ScheduleRepository : BaseRepository, IScheduleRepository
    {
        public ScheduleRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
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

            return await GetScheduleInternalAsync(email, id);
        }

        public async Task<Schedule> CreateScheduleAsync(string email, Schedule schedule)
        {
            CheckString(email, "email");
            CheckString(schedule.Name, "name");

            int schedules = await Context.Schedules.CountAsync(s => s.Email == email);
            if (schedules >= 20)
            {
                throw new ConstraintException(nameof(CreateScheduleAsync));
            }

            schedule.Email = email;
            Schedule newSchedule = (await Context.Schedules.AddAsync(schedule)).Entity;
            await Context.SaveChangesAsync();
            
            //TO DO
            //RecurringJob.AddOrUpdate(schedule.Id.ToString(),() => Console.WriteLine("aaa"),Cron.Minutely);
            
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

            Mapper.Map(scheduleToPatch, schedule);
            await Context.SaveChangesAsync();
            RecurringJob.RemoveIfExists(schedule.Id.ToString());
            //TO DO
            //RecurringJob.AddOrUpdate(schedule.Id.ToString(),,);
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