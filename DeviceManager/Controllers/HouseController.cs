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
    [Route("users/{id_user}/houses")]
    public class HouseController
    {
        [HttpGet]
        public async Task<List<House>> Get(int id_user)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id_user);
                List<House> houses = homeAssistantContext.Houses.Where(h => h.UserId == user.Id).ToList();
                return houses;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<House> Get(int id_user, int id)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id_user);
                House house = homeAssistantContext.Houses.Where(h => h.UserId == user.Id).Where(h => h.Id == id)
                    .FirstOrDefault();
                return house;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<House> Post(int id_user, [FromBody] House House)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House.UserId = id_user;
                House House1 = homeAssistantContext.Houses.Add(House).Entity;
                await homeAssistantContext.SaveChangesAsync();
                return House1;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id_user, int id)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id_user);
                House house = homeAssistantContext.Houses.Where(h => h.UserId == user.Id).Where(h => h.Id == id)
                    .FirstOrDefault();
                homeAssistantContext.Houses.Remove(house);
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