using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using HomeAssistantAPI.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Interfaces
{
    public interface IThermostatCommandRepository
    {
        Task<IEnumerable<ThermostatCommandResponse>> GetThermostatCommandsAsync(string email, Guid scheduleId);
        Task<ThermostatCommand> GetThermostatCommandByIdAsync(string email, Guid scheduleId, Guid id);

        Task<ThermostatCommand> CreateThermostatCommandAsync(string email, Guid scheduleId, ThermostatCommand
            thermostatCommand);

        Task<ThermostatCommand> PartialUpdateThermostatCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<ThermostatCommandRequest> thermostatCommandPatch);

        Task<ThermostatCommand> DeleteThermostatCommandAsync(string email, Guid scheduleId, Guid id);
    }
}