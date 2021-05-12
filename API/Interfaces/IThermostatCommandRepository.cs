using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models;
using Shared.Requests;

namespace API.Interfaces
{
    public interface IThermostatCommandRepository
    {
        Task<IEnumerable<ThermostatCommand>> GetThermostatCommandsAsync(string email, Guid scheduleId);
        Task<ThermostatCommand> GetThermostatCommandByIdAsync(string email, Guid scheduleId, Guid id);

        Task<ThermostatCommand> CreateThermostatCommandAsync(string email, Guid scheduleId, ThermostatCommand
            thermostatCommand);

        Task<ThermostatCommand> PartialUpdateThermostatCommandAsync(string email, Guid scheduleId, Guid id,
            JsonPatchDocument<ThermostatCommandRequest> thermostatCommandPatch);

        Task<ThermostatCommand> DeleteThermostatCommandAsync(string email, Guid scheduleId, Guid id);
    }
}