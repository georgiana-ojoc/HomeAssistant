using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class HouseRequest
    {
        [Required] public string Name { get; set; }
    }
}