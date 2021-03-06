using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Requests;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

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
            CheckGuid(houseId, "house id");

            House house = await GetHouseInternalAsync(email, houseId);
            if (house == null)
            {
                return null;
            }

            return await Context.Rooms.Where(room => room.HouseId == house.Id).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(string email, Guid houseId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(id, "id");

            return await GetRoomInternalAsync(email, houseId, id);
        }

        public async Task<Room> CreateRoomAsync(string email, Guid houseId, Room room)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckString(room.Name, "name");

            House house = await GetHouseInternalAsync(email, houseId);
            if (house == null)
            {
                return null;
            }

            int limit = await GetLimit("rooms", email);
            int roomsByHouseId = await Context.Rooms.CountAsync(r => r.HouseId == houseId);
            if (roomsByHouseId >= limit)
            {
                throw new ConstraintException("You have no rooms left in this house. Upgrade your subscription.");
            }

            int roomsByHouseIdAndName = await Context.Rooms.CountAsync(r => r.HouseId == houseId &&
                                                                            r.Name == room.Name);
            if (roomsByHouseIdAndName > 0)
            {
                throw new DuplicateNameException("You already have a room with the specified name in this house.");
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
            CheckGuid(houseId, "house id");
            CheckGuid(id, "id");

            Room room = await GetRoomInternalAsync(email, houseId, id);
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
            CheckGuid(houseId, "house id");
            CheckGuid(id, "id");

            Room room = await GetRoomInternalAsync(email, houseId, id);
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