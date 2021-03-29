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
        [HttpGet]
        public async Task<List<Room>> Get(int id_user, int id_house)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                List<Room> rooms = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id).ToList();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id_user);
                House house = homeAssistantContext.Houses.Where(h => h.UserId == user.Id).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(r => r.HouseId == house.Id).Where(r => r.Id == id)
                    .FirstOrDefault();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                room.HouseId = house.Id;
                Room _room_ = homeAssistantContext.Rooms.Add(room).Entity;
                await homeAssistantContext.SaveChangesAsync();
                return _room_;
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id_user);
                Room Room = homeAssistantContext.Rooms.Where(h => h.HouseId == user.Id).Where(h => h.Id == id)
                    .FirstOrDefault();
                homeAssistantContext.Rooms.Remove(Room);
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