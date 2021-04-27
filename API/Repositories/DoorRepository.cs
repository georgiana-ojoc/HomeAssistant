using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class DoorRepository : Repository, IDoorRepository
    {
        public DoorRepository(HomeAssistantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Door>> GetDoorsAsync(string email, Guid houseId, Guid roomId)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.Doors.Where(door => door.RoomId == room.Id).ToListAsync();
        }

        public async Task<Door> GetDoorByIdAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.Doors.Where(door => door.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Door> CreateDoorAsync(string email, Guid houseId, Guid roomId, Door door)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            door.RoomId = room.Id;
            Door newDoor = (await Context.Doors.AddAsync(door)).Entity;
            await Context.SaveChangesAsync();
            return newDoor;
        }

        public async Task<Door> DeleteDoorAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Door door = await Context.Doors.Where(d => d.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (door == null)
            {
                return null;
            }

            Context.Doors.Remove(door);
            await Context.SaveChangesAsync();
            return door;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() >= 0;
        }
    }
}