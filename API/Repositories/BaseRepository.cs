using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly HomeAssistantContext Context;
        protected readonly IMapper Mapper;

        protected BaseRepository(HomeAssistantContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected void CheckString(string field, string name)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentNullException(name);
            }
        }

        protected void CheckGuid(Guid field, string name)
        {
            if (field == Guid.Empty)
            {
                throw new ArgumentNullException(name);
            }
        }

        protected async Task<House> GetHouseInternalAsync(string email, Guid id)
        {
            return await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        protected async Task<Room> GetRoomInternalAsync(string email, Guid houseId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            return await Context.Rooms.Where(room => room.HouseId == house.Id)
                .FirstOrDefaultAsync(room => room.Id == id);
        }
    }
}