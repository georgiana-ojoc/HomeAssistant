using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Interfaces;
using AutoMapper;
using Hangfire;
using MediatR;
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
            if (!TimeSpan.TryParse(schedule.Time, CultureInfo.InvariantCulture, out TimeSpan time))
            {
                throw new ArgumentException("Time is not valid.");
            }

            if (schedule.Days is 0 or > 127)
            {
                throw new ArgumentException("Days cannot be 0 or bigger than 127.");
            }

            int schedules = await Context.Schedules.CountAsync(s => s.Email == email);
            if (schedules >= 20)
            {
                throw new ConstraintException(nameof(CreateScheduleAsync));
            }

            schedule.Email = email;
            schedule.Time = time.ToString(@"hh\:mm");
            Schedule newSchedule = (await Context.Schedules.AddAsync(schedule)).Entity;
            await Context.SaveChangesAsync();

            try
            {
                RecurringJob.AddOrUpdate(schedule.Id.ToString(),
                    ()=>Helper.ChangeAllInSchedule(schedule.Id),
                    Helper.GetCronExpression(schedule.Time,schedule.Days));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
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
            if (schedule.Days is 0 or > 127)
            {
                throw new ArgumentException("Days cannot be 0 or bigger than 127.");
            }

            Mapper.Map(scheduleToPatch, schedule);
            await Context.SaveChangesAsync();
            RecurringJob.RemoveIfExists(schedule.Id.ToString());
            RecurringJob.AddOrUpdate(schedule.Id.ToString(),
             () => Helper.ChangeAllInSchedule(schedule.Id),
                             Helper.GetCronExpression(schedule.Time,schedule.Days));
            
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