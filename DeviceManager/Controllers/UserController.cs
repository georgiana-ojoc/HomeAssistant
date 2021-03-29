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
    [Route("/users")]
    public class UserController
    {
        [HttpGet]
        public async Task<List<User>> Get()
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            return await homeAssistantContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            return await homeAssistantContext.Users.FindAsync(id);
        }

        [HttpPost]
        public async Task<User> Post([FromBody] User user)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user1 = homeAssistantContext.Users.Add(user).Entity;
                await homeAssistantContext.SaveChangesAsync();
                return user1;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                User user = homeAssistantContext.Users.Find(id);
                homeAssistantContext.Users.Remove(user);
                await homeAssistantContext.SaveChangesAsync();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}