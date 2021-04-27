using System;

#nullable disable

namespace Shared.Models
{
    public class ThermostatCommand : BaseModel
    {
        public Guid ThermostatId { get; set; }
        public Guid ScheduleId { get; set; }
        public decimal Temperature { get; set; }

        internal Thermostat Thermostat { get; set; }
        internal Schedule Schedule { get; set; }
    }
}