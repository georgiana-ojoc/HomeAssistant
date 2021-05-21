using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models;
using Shared.Requests;

namespace API.Interfaces
{
    public interface IUserLimitRepository
    {
        Task<IEnumerable<UserLimit>> GetUserLimitsAsync(string email);
        Task<UserLimit> GetUserLimitByIdAsync(string email, Guid id);
        Task<UserLimit> CreateUserLimitAsync(string email, UserLimit userLimit);
        Task<UserLimit> PartialUpdateUserLimitAsync(string email, Guid id, JsonPatchDocument<UserLimitRequest> userLimitPatch);
        Task<UserLimit> DeleteUserLimitAsync(string email, Guid id);
    }
}