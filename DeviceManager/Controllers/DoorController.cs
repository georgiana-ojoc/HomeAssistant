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
    [Route("users/{id_user}/houses/{id_house}/rooms/{id_room}/doors")]
    public class DoorController
    {
        [HttpGet]
        public async Task<List<Door>> Get(int id_user, int id_house, int id_room)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                List<Door> door = homeAssistantContext.Doors.Where(lb => lb.RoomId == room.Id).ToList();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                Door door = homeAssistantContext.Doors.Where(lb => lb.RoomId == room.Id).Where(lb => lb.Id == id)
                    .FirstOrDefault();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                door.RoomId = room.Id;
                Door _door_ = homeAssistantContext.Doors.Add(door).Entity;
                await homeAssistantContext.SaveChangesAsync();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                Door door = homeAssistantContext.Doors.Where(lb => lb.RoomId == room.Id).Where(lb => lb.Id == id)
                    .FirstOrDefault();

                homeAssistantContext.Doors.Remove(door);
                homeAssistantContext.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}