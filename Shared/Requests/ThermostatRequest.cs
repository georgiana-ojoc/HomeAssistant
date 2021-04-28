using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class ThermostatRequest
    {
        [Required] public string Name { get; set; }
        public decimal? Temperature { get; set; }
    }
}