using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Shared.Requests;

namespace API.Repositories
{
    public class UserLimitRepository : BaseRepository, IUserLimitRepository
    {
        public UserLimitRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<UserLimit>> GetUserLimitsAsync(string email)
        {
            CheckString(email, "email");

            return await Context.UserLimits.Where(userLimit => userLimit.Email == email).ToListAsync();
        }

        public async Task<UserLimit> GetUserLimitByIdAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            return await GetUserLimitInternalAsync(email, id);
        }

        public async Task<UserLimit> CreateUserLimitAsync(string email, UserLimit userLimit)
        {
            CheckString(email, "email");
            
            userLimit.Email = email;
            UserLimit newUserLimit = (await Context.UserLimits.AddAsync(userLimit)).Entity;
            await Context.SaveChangesAsync();
            return newUserLimit;
        }

        public async Task<UserLimit> PartialUpdateUserLimitAsync(string email, Guid id,
            JsonPatchDocument<UserLimitRequest> userLimitPatch)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            UserLimit userLimit = await GetUserLimitInternalAsync(email, id);
            if (userLimit == null)
            {
                return null;
            }

            UserLimitRequest userLimitToPatch = Mapper.Map<UserLimitRequest>(userLimit);
            userLimitPatch.ApplyTo(userLimitToPatch);

            Mapper.Map(userLimitToPatch, userLimit);
            await Context.SaveChangesAsync();
            return userLimit;
        }

        public async Task<UserLimit> DeleteUserLimitAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            UserLimit userLimit = await GetUserLimitInternalAsync(email, id);
            if (userLimit == null)
            {
                return null;
            }

            Context.UserLimits.Remove(userLimit);
            await Context.SaveChangesAsync();
            return userLimit;
        }
    }
}