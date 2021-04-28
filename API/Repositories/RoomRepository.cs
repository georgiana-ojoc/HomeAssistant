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
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(string email, Guid houseId)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");

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
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(id, "id");

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
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckString(room.Name, "name");

            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            int rooms = await Context.Rooms.CountAsync(r => r.HouseId == houseId);
            if (rooms >= 20)
            {
                throw new ConstraintException(nameof(CreateRoomAsync));
            }

            room.HouseId = house.Id;
            Room newRoom = (await Context.Rooms.AddAsync(room)).Entity;
            await Context.SaveChangesAsync();
            return newRoom;
        }

        public async Task<Room> PartialUpdateRoomAsync(string email, Guid houseId, Guid id,
            JsonPatchDocument<RoomRequest> roomPatch)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(id, "id");

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

            RoomRequest roomToPatch = Mapper.Map<RoomRequest>(room);
            roomPatch.ApplyTo(roomToPatch);
            CheckString(roomToPatch.Name, "name");

            Mapper.Map(roomToPatch, room);
            await Context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> DeleteRoomAsync(string email, Guid houseId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(id, "id");

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
    }
}