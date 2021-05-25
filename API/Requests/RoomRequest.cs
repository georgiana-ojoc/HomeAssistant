using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class RoomRequest
    {
        [Required] public string Name { get; set; }
    }
}