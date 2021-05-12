using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models;
using Shared.Requests;

namespace API.Interfaces
{
    public interface ILightBulbCommandRepository
    {
        Task<IEnumerable<LightBulbCommand>> GetLightBulbCommandsAsync(string email, Guid scheduleId);
        Task<LightBulbCommand> GetLightBulbCommandByIdAsync(string email, Guid scheduleId, Guid id);

        Task<LightBulbCommand> CreateLightBulbCommandAsync(string email, Guid scheduleId, LightBulbCommand
            lightBulbCommand);

        Task<LightBulbCommand> PartialUpdateLightBulbCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<LightBulbCommandRequest> lightBulbCommandPatch);

        Task<LightBulbCommand> DeleteLightBulbCommandAsync(string email, Guid scheduleId, Guid id);
    }
}