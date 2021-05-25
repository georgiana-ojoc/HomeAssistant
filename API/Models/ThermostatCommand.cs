using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace API.Models
{
    public class ThermostatCommand : BaseModel
    {
        [Required] public Guid ThermostatId { get; set; }
        [Required] public Guid ScheduleId { get; set; }
        [Required] public decimal Temperature { get; set; }

        internal Thermostat Thermostat { get; set; }
        internal Schedule Schedule { get; set; }
    }
}