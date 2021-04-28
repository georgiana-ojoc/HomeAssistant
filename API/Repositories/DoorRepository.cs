using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Requests;

namespace API.Repositories
{
    public class DoorRepository : BaseRepository, IDoorRepository
    {
        public DoorRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Door>> GetDoorsAsync(string email, Guid houseId, Guid roomId)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");

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
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");

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
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckString(door.Name, "name");

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

            int doors = await Context.Doors.CountAsync(d => d.RoomId == roomId);
            if (doors >= 10)
            {
                throw new ConstraintException(nameof(CreateDoorAsync));
            }

            door.RoomId = room.Id;
            Door newDoor = (await Context.Doors.AddAsync(door)).Entity;
            await Context.SaveChangesAsync();
            return newDoor;
        }

        public async Task<Door> PartialUpdateDoorAsync(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<DoorRequest> doorPatch)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");

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

            DoorRequest doorToPatch = Mapper.Map<DoorRequest>(door);
            doorPatch.ApplyTo(doorToPatch);
            CheckString(doorToPatch.Name, "name");

            Mapper.Map(doorToPatch, door);
            await Context.SaveChangesAsync();
            return door;
        }

        public async Task<Door> DeleteDoorAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");

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
    }
}