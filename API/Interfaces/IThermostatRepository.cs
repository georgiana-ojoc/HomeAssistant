using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IThermostatRepository : IDisposable
    {
        Task<IEnumerable<Thermostat>> GetThermostatsAsync(int userId, int houseId, int roomId);
        Task<Thermostat> GetThermostatByIdAsync(int userId, int houseId, int roomId, int id);
        Task<Thermostat> CreateThermostat(int userId, int houseId, int roomId, Thermostat thermostat);
        Task<Thermostat> DeleteThermostat(int userId, int houseId, int roomId, int id);
        Task<Thermostat> UpdateThermostat(int userId, int houseId, int roomId, Thermostat thermostat);
    }
}