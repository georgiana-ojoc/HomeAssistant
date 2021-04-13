using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeAssistantContext _context;

        public RoomRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(int userId, int houseId)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await _context.Rooms.Where(r => r.HouseId == house.Id).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int userId, int houseId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
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

            return room;
        }

        public async Task<Room> CreateRoom(int userId, int houseId, Room room)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
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

        public async Task<Room> DeleteRoom(int userId, int houseId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
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

        public Task<Room> UpdateRoom(int userId, int houseId, Room room)
        {
            throw new System.NotImplementedException();
        }
        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}