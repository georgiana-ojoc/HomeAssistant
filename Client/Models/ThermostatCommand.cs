using System;

#nullable disable

namespace Client.Models
{
    public class ThermostatCommand : BaseModel
    {
        public Guid ThermostatId { get; set; }
        public Guid ScheduleId { get; set; }
        public decimal Temperature { get; set; }
    }
}