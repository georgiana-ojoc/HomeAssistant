using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IThermostatRepository : IDisposable
    {
        Task<IEnumerable<Thermostat>> GetThermostatsAsync(string email, int houseId, int roomId);
        Task<Thermostat> GetThermostatByIdAsync(string email, int houseId, int roomId, int id);
        Task<Thermostat> CreateThermostatAsync(string email, int houseId, int roomId, Thermostat thermostat);
        Task<Thermostat> DeleteThermostatAsync(string email, int houseId, int roomId, int id);
    }
}