using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class DoorRepository : IDoorRepository
    {
        private readonly HomeAssistantContext _context;

        public DoorRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Door>> GetDoorsAsync(int userId, int houseId, int roomId)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await _context.Doors.Where(d => d.RoomId == room.Id).ToListAsync();
        }

        public async Task<Door> GetDoorByIdAsync(int userId, int houseId, int roomId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Door door = await _context.Doors.Where(d => d.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);

            return door;
        }

        public async Task<Door> CreateDoorAsync(int userId, int houseId, int roomId, Door door)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            door.RoomId = room.Id;
            Door newDoor = (await _context.Doors.AddAsync(door)).Entity;
            await _context.SaveChangesAsync();
            return newDoor;
        }

        public async Task<Door> DeleteDoorAsync(int userId, int houseId, int roomId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Door door = await _context.Doors.Where(d => d.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (door == null)
            {
                return null;
            }

            _context.Doors.Remove(door);
            await _context.SaveChangesAsync();
            return door;
        }

        public Task<Door> UpdateDoorAsync(int userId, int houseId, int roomId, Door door)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            // _context?.Dispose();
        }
    }
}