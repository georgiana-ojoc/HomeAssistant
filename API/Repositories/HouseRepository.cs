using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private readonly HomeAssistantContext _context;

        public HouseRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<House>> GetHousesAsync(string email)
        {
            return await _context.Houses.Where(house => house.Email == email).ToListAsync();
        }

        public async Task<House> GetHouseByIdAsync(string email, int id)
        {
            return await _context.Houses.Where(house => house.Email == email)
                .FirstOrDefaultAsync(house => house.Id == id);
        }

        public async Task<House> CreateHouseAsync(string email, House house)
        {
            house.Email = email;
            House newHouse = (await _context.Houses.AddAsync(house)).Entity;
            await _context.SaveChangesAsync();
            return newHouse;
        }

        public async Task<House> DeleteHouseAsync(string email, int id)
        {
            House house = await _context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return null;
            }

            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
            return house;
        }

        public void Dispose()
        {
            // TODO 
        }
    }
}