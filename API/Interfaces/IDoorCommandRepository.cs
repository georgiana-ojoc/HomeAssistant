using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Requests;
using API.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Interfaces
{
    public interface IDoorCommandRepository
    {
        Task<IEnumerable<DoorCommandResponse>> GetDoorCommandsAsync(string email, Guid scheduleId);
        Task<DoorCommand> GetDoorCommandByIdAsync(string email, Guid scheduleId, Guid id);

        Task<DoorCommand> CreateDoorCommandAsync(string email, Guid scheduleId, DoorCommand doorCommand);

        Task<DoorCommand> PartialUpdateDoorCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<DoorCommandRequest> doorCommandPatch);

        Task<DoorCommand> DeleteDoorCommandAsync(string email, Guid scheduleId, Guid id);
    }
}