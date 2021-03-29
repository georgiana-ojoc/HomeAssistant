using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DeviceManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DeviceManager.Controllers
{
    [ApiController]
    [Route("users/{id_user}/houses/{id_house}/rooms")]
    public class RoomController
    {
        private readonly HomeAssistantContext _context;

        public RoomController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Room>> Get(int id_user, int id_house)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                List<Room> rooms = _context.Rooms.Where(h => h.HouseId == house.Id).ToList();
                return rooms;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<Room> Get(int id_user, int id_house, int id)
        {
            try
            {
                User user = _context.Users.Find(id_user);
                House house = _context.Houses.Where(h => h.UserId == user.Id)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms.Where(r => r.HouseId == house.Id)
                    .FirstOrDefault(r => r.Id == id);
                return room;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<Room> Post(int id_user, int id_house, [FromBody] Room room)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                room.HouseId = house.Id;
                Room newRoom = _context.Rooms.Add(room).Entity;
                await _context.SaveChangesAsync();
                return newRoom;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id_user, int id_house, int id)
        {
            try
            {
                User user = _context.Users.Find(id_user);
                Room Room = _context.Rooms.Where(h => h.HouseId == user.Id)
                    .FirstOrDefault(h => h.Id == id);
                _context.Rooms.Remove(Room);
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