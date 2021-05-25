using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class ThermostatRequest
    {
        [Required] public string Name { get; set; }
        public decimal? Temperature { get; set; }
    }
}