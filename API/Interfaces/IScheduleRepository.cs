using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetSchedulesAsync(string email);
        Task<Schedule> GetScheduleByIdAsync(string email, Guid id);
        Task<Schedule> CreateScheduleAsync(string email, Schedule schedule);

        Task<Schedule> PartialUpdateScheduleAsync(string email, Guid id, JsonPatchDocument<ScheduleRequest>
            schedulePatch);

        Task<Schedule> DeleteScheduleAsync(string email, Guid id);
    }
}