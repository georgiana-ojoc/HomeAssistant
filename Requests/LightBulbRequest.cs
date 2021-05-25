using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class LightBulbRequest
    {
        [Required] public string Name { get; set; }
        public int? Color { get; set; }
        public byte? Intensity { get; set; }
    }
}