using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models;
using Shared.Requests;

namespace API.Interfaces
{
    public interface IDoorRepository
    {
        Task<IEnumerable<Door>> GetDoorsAsync(string email, Guid houseId, Guid roomId);
        Task<Door> GetDoorByIdAsync(string email, Guid houseId, Guid roomId, Guid id);
        Task<Door> CreateDoorAsync(string email, Guid houseId, Guid roomId, Door door);

        Task<Door> PartialUpdateDoorAsync(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<DoorRequest> doorPatch);

        Task<Door> DeleteDoorAsync(string email, Guid houseId, Guid roomId, Guid id);
    }
}