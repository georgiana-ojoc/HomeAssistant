using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{id_user}/houses/{id_house}/rooms/{id_room}/doors")]
    public class DoorController
    {
        private readonly HomeAssistantContext _context;

        public DoorController(HomeAssistantContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<List<Door>> Get(int id_user, int id_house, int id_room)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                 List<Door> door =  _context.Doors.Where(lb => lb.RoomId == room.Id).ToList();
                return door;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<Door> Get(int id_user, int id_house, int id_room, int id)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                Door door = _context.Doors.Where(lb => lb.RoomId == room.Id)
                    .FirstOrDefault(lb => lb.Id == id);
                return door;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<Door> Post(int id_user, int id_house, int id_room, [FromBody] Door door)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                door.RoomId = room.Id;
                Door _door_ = _context.Doors.Add(door).Entity;
                await _context.SaveChangesAsync();
                return _door_;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id_user, int id_house, int id_room, int id)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                Door door = _context.Doors.Where(lb => lb.RoomId == room.Id)
                    .FirstOrDefault(lb => lb.Id == id);

                _context.Doors.Remove(door);
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}