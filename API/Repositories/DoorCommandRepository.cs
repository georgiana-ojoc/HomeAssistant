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
    public class DoorCommandRepository : BaseRepository, IDoorCommandRepository
    {
        public DoorCommandRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private async Task<DoorCommand> GetDoorCommandInternalAsync(string email, Guid scheduleId, Guid id)
        {
            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            return await Context.DoorCommands.Where(doorCommand => doorCommand.ScheduleId == schedule.Id)
                .FirstOrDefaultAsync(doorCommand => doorCommand.Id == id);
        }

        public async Task<IEnumerable<DoorCommandResponse>> GetDoorCommandsAsync(string email, Guid scheduleId)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            var results = await Context.DoorCommands
                .Join(Context.Doors,
                    dc => dc.DoorId,
                    d => d.Id,
                    (dc, d) => new
                    {
                        dc.Id,
                        dc.ScheduleId,
                        dc.DoorId,
                        DoorName = d.Name,
                        dc.Locked
                    }).Where(dc => dc.ScheduleId == scheduleId).ToListAsync();

            IList<DoorCommandResponse> doorCommandResponses = new List<DoorCommandResponse>();
            foreach (var result in results)
            {
                doorCommandResponses.Add(new DoorCommandResponse()
                {
                    Id = result.Id,
                    ScheduleId = result.ScheduleId,
                    DoorId = result.DoorId,
                    DoorName = result.DoorName,
                    Locked = result.Locked
                });
            }

            return doorCommandResponses;
        }

        public async Task<DoorCommand> GetDoorCommandByIdAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            return await GetDoorCommandInternalAsync(email, scheduleId, id);
        }

        public async Task<DoorCommand> CreateDoorCommandAsync(string email, Guid scheduleId,
            DoorCommand doorCommand)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(doorCommand.DoorId, "door_id");

            Schedule schedule = await GetScheduleInternalAsync(email, scheduleId);
            if (schedule == null)
            {
                return null;
            }

            Door door = await Context.Doors.FirstOrDefaultAsync(d => d.Id == doorCommand.DoorId);
            if (door == null)
            {
                return null;
            }

            int doorCommands = await Context.DoorCommands.CountAsync(dc => dc.ScheduleId == scheduleId);
            if (doorCommands >= 10)
            {
                throw new ConstraintException(nameof(CreateDoorCommandAsync));
            }

            doorCommand.ScheduleId = schedule.Id;
            DoorCommand newDoorCommand = (await Context.DoorCommands.AddAsync(doorCommand)).Entity;
            await Context.SaveChangesAsync();
            return newDoorCommand;
        }

        public async Task<DoorCommand> PartialUpdateDoorCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<DoorCommandRequest> doorCommandPatch)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            DoorCommand doorCommand = await GetDoorCommandInternalAsync(email, scheduleId, id);
            if (doorCommand == null)
            {
                return null;
            }

            DoorCommandRequest doorCommandToPatch = Mapper.Map<DoorCommandRequest>(doorCommand);
            doorCommandPatch.ApplyTo(doorCommandToPatch);
            CheckGuid(doorCommandToPatch.DoorId, "door_id");
            Door door = await Context.Doors.FirstOrDefaultAsync(d => d.Id == doorCommand.DoorId);
            if (door == null)
            {
                return null;
            }

            Mapper.Map(doorCommandToPatch, doorCommand);
            await Context.SaveChangesAsync();
            return doorCommand;
        }

        public async Task<DoorCommand> DeleteDoorCommandAsync(string email, Guid scheduleId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(scheduleId, "schedule_id");
            CheckGuid(id, "id");

            DoorCommand doorCommand = await GetDoorCommandInternalAsync(email, scheduleId, id);
            if (doorCommand == null)
            {
                return null;
            }

            Context.DoorCommands.Remove(doorCommand);
            await Context.SaveChangesAsync();
            return doorCommand;
        }
    }
}