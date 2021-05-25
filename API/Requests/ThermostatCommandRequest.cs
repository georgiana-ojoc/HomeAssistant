using System;
using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class ThermostatCommandRequest
    {
        [Required] public Guid ThermostatId { get; set; }
        [Required] public decimal Temperature { get; set; }
    }
}