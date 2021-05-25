using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class RoomRequest
    {
        [Required] public string Name { get; set; }
    }
}