using System;
using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class ThermostatCommandRequest
    {
        [Required] public Guid ThermostatId { get; set; }
        [Required] public decimal Temperature { get; set; }
    }
}