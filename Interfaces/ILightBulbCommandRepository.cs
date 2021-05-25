using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using HomeAssistantAPI.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Interfaces
{
    public interface ILightBulbCommandRepository
    {
        Task<IEnumerable<LightBulbCommandResponse>> GetLightBulbCommandsAsync(string email, Guid scheduleId);
        Task<LightBulbCommand> GetLightBulbCommandByIdAsync(string email, Guid scheduleId, Guid id);

        Task<LightBulbCommand> CreateLightBulbCommandAsync(string email, Guid scheduleId, LightBulbCommand
            lightBulbCommand);

        Task<LightBulbCommand> PartialUpdateLightBulbCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<LightBulbCommandRequest> lightBulbCommandPatch);

        Task<LightBulbCommand> DeleteLightBulbCommandAsync(string email, Guid scheduleId, Guid id);
    }
}