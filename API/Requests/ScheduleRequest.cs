using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class ScheduleRequest
    {
        [Required] public string Name { get; set; }
        [Required] public string Time { get; set; }
        [Required] public byte Days { get; set; }
    }
}