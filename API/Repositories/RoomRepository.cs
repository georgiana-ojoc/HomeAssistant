using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeAssistantContext _context;

        public RoomRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(string email, int houseId)
        {
            House house = await _context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await _context.Rooms.Where(room => room.HouseId == house.Id).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(string email, int houseId, int id)
        {
            House house = await _context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await _context.Rooms.Where(room => room.HouseId == house.Id)
                .FirstOrDefaultAsync(room => room.Id == id);
        }

        public async Task<Room> CreateRoomAsync(string email, int houseId, Room room)
        {
            House house = await _context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            room.HouseId = house.Id;
            Room newRoom = (await _context.Rooms.AddAsync(room)).Entity;
            await _context.SaveChangesAsync();
            return newRoom;
        }

        public async Task<Room> DeleteRoomAsync(string email, int houseId, int id)
        {
            House house = await _context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return null;
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public void Dispose()
        {
        }
    }
}