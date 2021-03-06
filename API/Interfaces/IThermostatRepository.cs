using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Interfaces
{
    public interface IThermostatRepository
    {
        Task<IEnumerable<Thermostat>> GetThermostatsAsync(string email, Guid houseId, Guid roomId);
        Task<Thermostat> GetThermostatByIdAsync(string email, Guid houseId, Guid roomId, Guid id);
        Task<Thermostat> CreateThermostatAsync(string email, Guid houseId, Guid roomId, Thermostat thermostat);

        Task<Thermostat> PartialUpdateThermostatAsync(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<ThermostatRequest> thermostatPatch);

        Task<Thermostat> DeleteThermostatAsync(string email, Guid houseId, Guid roomId, Guid id);
    }
}