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
using Shared.Responses;

namespace API.Repositories
{
    public class ThermostatCommandRepository : BaseRepository, IThermostatCommandRepository
    {
        public ThermostatCommandRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private async Task<ThermostatCommand> GetThermostatCommandInternalAsync(string email, Guid scheduleId, Guid id)
        {
            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            return await Context.ThermostatCommands.Where(thermostatCommand => thermostatCommand.ScheduleId ==
                                                                               schedule.Id)
                .FirstOrDefaultAsync(thermostatCommand => thermostatCommand.Id == id);
        }

        public async Task<IEnumerable<ThermostatCommandResponse>> GetThermostatCommandsAsync(string email,
            Guid scheduleId)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            var results = await Context.ThermostatCommands
                .Join(Context.Thermostats,
                    tc => tc.ThermostatId,
                    t => t.Id,
                    (tc, t) => new
                    {
                        tc.Id,
                        tc.ScheduleId,
                        tc.ThermostatId,
                        ThermostatName = t.Name,
                        tc.Temperature
                    }).Where(tc => tc.ScheduleId == scheduleId).ToListAsync();

            IList<ThermostatCommandResponse> thermostatCommandResponses = new List<ThermostatCommandResponse>();
            foreach (var result in results)
            {
                thermostatCommandResponses.Add(new ThermostatCommandResponse()
                {
                    Id = result.Id,
                    ScheduleId = result.ScheduleId,
                    ThermostatId = result.ThermostatId,
                    ThermostatName = result.ThermostatName,
                    Temperature = result.Temperature
                });
            }

            return thermostatCommandResponses;
        }

        public async Task<ThermostatCommand> GetThermostatCommandByIdAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            return await GetThermostatCommandInternalAsync(email, scheduleId, id);
        }

        public async Task<ThermostatCommand> CreateThermostatCommandAsync(string email, Guid scheduleId,
            ThermostatCommand thermostatCommand)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(thermostatCommand.ThermostatId, "thermostat_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            Thermostat thermostat = await Context.Thermostats.FirstOrDefaultAsync(t => t.Id ==
                thermostatCommand.ThermostatId);
            if (thermostat == null)
            {
                return null;
            }

            UserLimit userLimit = Context.UserLimits.Where(u => u.Email == email).FirstOrDefault();
            int limit = 10;
            if (userLimit != null)
                limit = userLimit.CommandLimit;
            int thermostatCommands = await Context.ThermostatCommands.CountAsync(tc => tc.ScheduleId ==
                scheduleId);
            if (thermostatCommands >= limit)
            {
                throw new ConstraintException(nameof(CreateThermostatCommandAsync));
            }

            thermostatCommand.ScheduleId = schedule.Id;
            ThermostatCommand newThermostatCommand =
                (await Context.ThermostatCommands.AddAsync(thermostatCommand)).Entity;
            await Context.SaveChangesAsync();
            return newThermostatCommand;
        }

        public async Task<ThermostatCommand> PartialUpdateThermostatCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<ThermostatCommandRequest> thermostatCommandPatch)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            ThermostatCommand thermostatCommand = await GetThermostatCommandInternalAsync(email, scheduleId, id);
            if (thermostatCommand == null)
            {
                return null;
            }

            ThermostatCommandRequest thermostatCommandToPatch = Mapper.Map<ThermostatCommandRequest>(thermostatCommand);
            thermostatCommandPatch.ApplyTo(thermostatCommandToPatch);
            CheckGuid(thermostatCommandToPatch.ThermostatId, "thermostat_id");
            Thermostat thermostat = await Context.Thermostats.FirstOrDefaultAsync(t => t.Id ==
                thermostatCommand.ThermostatId);
            if (thermostat == null)
            {
                return null;
            }

            Mapper.Map(thermostatCommandToPatch, thermostatCommand);
            await Context.SaveChangesAsync();
            return thermostatCommand;
        }

        public async Task<ThermostatCommand> DeleteThermostatCommandAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            ThermostatCommand thermostatCommand = await GetThermostatCommandInternalAsync(email, scheduleId, id);
            if (thermostatCommand == null)
            {
                return null;
            }

            Context.ThermostatCommands.Remove(thermostatCommand);
            await Context.SaveChangesAsync();
            return thermostatCommand;
        }
    }
}