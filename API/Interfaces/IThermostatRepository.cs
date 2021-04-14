using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IThermostatRepository : IDisposable
    {
        Task<IEnumerable<Thermostat>> GetThermostatsAsync(int userId, int houseId, int roomId);
        Task<Thermostat> GetThermostatByIdAsync(int userId, int houseId, int roomId, int id);
        Task<Thermostat> CreateThermostatAsync(int userId, int houseId, int roomId, Thermostat thermostat);
        Task<Thermostat> DeleteThermostatAsync(int userId, int houseId, int roomId, int id);
        Task<Thermostat> UpdateThermostatAsync(int userId, int houseId, int roomId, Thermostat thermostat);
    }
}