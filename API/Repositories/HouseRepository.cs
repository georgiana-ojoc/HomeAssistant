using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private readonly HomeAssistantContext _context;

        public HouseRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<House>> GetHousesAsync(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            return await _context.Houses.Where(h => h.UserId == user.Id).ToListAsync();
        }

        public async Task<House> GetHouseByIdAsync(int userId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return null;
            }

            return house;
        }

        public async Task<House> CreateHouse(int userId, House house)
        {
            User user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            house.UserId = user.Id;
            House newHouse = (await _context.Houses.AddAsync(house)).Entity;
            await _context.SaveChangesAsync();
            return newHouse;
        }

        public async Task<House> DeleteHouse(int userId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return null;
            }

            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
            return house;
        }

        public Task<House> UpdateHouse(int userId, House house)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}