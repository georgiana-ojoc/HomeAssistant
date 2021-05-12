using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Shared.Requests;

namespace API.Repositories
{
    public class LightBulbCommandRepository : BaseRepository, ILightBulbCommandRepository
    {
        public LightBulbCommandRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private async Task<LightBulbCommand> GetLightBulbCommandInternalAsync(string email, Guid scheduleId, Guid id)
        {
            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            return await Context.LightBulbCommands.Where(lightBulbCommand => lightBulbCommand.ScheduleId == schedule.Id)
                .FirstOrDefaultAsync(lightBulbCommand => lightBulbCommand.Id == id);
        }

        public async Task<IEnumerable<LightBulbCommand>> GetLightBulbCommandsAsync(string email, Guid scheduleId)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            return await Context.LightBulbCommands.Where(lightBulbCommand => lightBulbCommand.ScheduleId == schedule.Id)
                .ToListAsync();
        }

        public async Task<LightBulbCommand> GetLightBulbCommandByIdAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            return await GetLightBulbCommandInternalAsync(email, scheduleId, id);
        }

        public async Task<LightBulbCommand> CreateLightBulbCommandAsync(string email, Guid scheduleId,
            LightBulbCommand lightBulbCommand)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(lightBulbCommand.LightBulbId, "light_bulb_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            LightBulb lightBulb = await Context.LightBulbs.FirstOrDefaultAsync(lb => lb.Id ==
                lightBulbCommand.LightBulbId);
            if (lightBulb == null)
            {
                return null;
            }

            int lightBulbCommands = await Context.LightBulbCommands.CountAsync(lbc => lbc.ScheduleId ==
                scheduleId);
            if (lightBulbCommands >= 10)
            {
                throw new ConstraintException(nameof(CreateLightBulbCommandAsync));
            }

            lightBulbCommand.ScheduleId = schedule.Id;
            LightBulbCommand newLightBulbCommand = (await Context.LightBulbCommands.AddAsync(lightBulbCommand)).Entity;
            await Context.SaveChangesAsync();
            return newLightBulbCommand;
        }

        public async Task<LightBulbCommand> PartialUpdateLightBulbCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<LightBulbCommandRequest> lightBulbCommandPatch)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            LightBulbCommand lightBulbCommand = await GetLightBulbCommandInternalAsync(email, scheduleId, id);
            if (lightBulbCommand == null)
            {
                return null;
            }

            LightBulbCommandRequest lightBulbCommandToPatch = Mapper.Map<LightBulbCommandRequest>(lightBulbCommand);
            lightBulbCommandPatch.ApplyTo(lightBulbCommandToPatch);
            CheckGuid(lightBulbCommandToPatch.LightBulbId, "light_bulb_id");
            LightBulb lightBulb = await Context.LightBulbs.FirstOrDefaultAsync(lb => lb.Id ==
                lightBulbCommand.LightBulbId);
            if (lightBulb == null)
            {
                return null;
            }

            Mapper.Map(lightBulbCommandToPatch, lightBulbCommand);
            await Context.SaveChangesAsync();
            return lightBulbCommand;
        }

        public async Task<LightBulbCommand> DeleteLightBulbCommandAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            LightBulbCommand lightBulbCommand = await GetLightBulbCommandInternalAsync(email, scheduleId, id);
            if (lightBulbCommand == null)
            {
                return null;
            }

            Context.LightBulbCommands.Remove(lightBulbCommand);
            await Context.SaveChangesAsync();
            return lightBulbCommand;
        }
    }
}