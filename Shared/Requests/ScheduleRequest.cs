using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class ScheduleRequest
    {
        [Required] public string Name { get; set; }
        [Required] public TimeSpan Time { get; set; }
        [Required] public byte Days { get; set; }
    }
}