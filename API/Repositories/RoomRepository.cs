using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(HomeAssistantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(string email, Guid houseId)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await Context.Rooms.Where(room => room.HouseId == house.Id).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(string email, Guid houseId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await Context.Rooms.Where(room => room.HouseId == house.Id)
                .FirstOrDefaultAsync(room => room.Id == id);
        }

        public async Task<Room> CreateRoomAsync(string email, Guid houseId, Room room)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            room.HouseId = house.Id;
            Room newRoom = (await Context.Rooms.AddAsync(room)).Entity;
            await Context.SaveChangesAsync();
            return newRoom;
        }

        public async Task<Room> DeleteRoomAsync(string email, Guid houseId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return null;
            }

            Context.Rooms.Remove(room);
            await Context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() >= 0;
        }
    }
}