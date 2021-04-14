using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IHouseRepository : IDisposable
    {
        Task<IEnumerable<House>> GetHousesAsync(string email);
        Task<House> GetHouseByIdAsync(string email, int id);
        Task<House> CreateHouseAsync(string email, House house);
        Task<House> DeleteHouseAsync(string email, int id);
    }
}