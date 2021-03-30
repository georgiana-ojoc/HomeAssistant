using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses/{house_id}/rooms/{room_id}/doors")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorController
    {
        private readonly HomeAssistantContext _context;

        public DoorController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Door>>> Get(int user_id, int house_id, int room_id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == room_id);
            if (room == null)
            {
                return new NotFoundResult();
            }

            return await _context.Doors.Where(d => d.RoomId == room.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Door>> Get(int user_id, int house_id, int room_id, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == room_id);
            if (room == null)
            {
                return new NotFoundResult();
            }

            Door door = await _context.Doors.Where(d => d.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (door == null)
            {
                return new NotFoundResult();
            }

            return door;
        }

        [HttpPost]
        public async Task<ActionResult<Door>> Post(int user_id, int house_id, int room_id, [FromBody] Door door)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == house_id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                    .FirstOrDefaultAsync(r => r.Id == room_id);
                if (room == null)
                {
                    return new NotFoundResult();
                }

                door.RoomId = room.Id;
                Door newDoor = (await _context.Doors.AddAsync(door)).Entity;
                await _context.SaveChangesAsync();
                return newDoor;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int room_id, int id)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == house_id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                    .FirstOrDefaultAsync(r => r.Id == room_id);
                if (room == null)
                {
                    return new NotFoundResult();
                }

                Door door = await _context.Doors.Where(d => d.RoomId == room.Id)
                    .FirstOrDefaultAsync(d => d.Id == id);
                if (door == null)
                {
                    return new NotFoundResult();
                }

                _context.Doors.Remove(door);
                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}