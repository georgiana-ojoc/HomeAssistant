using System;
using Shared.Models;

#nullable disable

namespace Shared.Responses
{
    public class ThermostatCommandResponse : BaseModel
    {
        public Guid ScheduleId { get; set; }
        public Guid ThermostatId { get; set; }
        public string ThermostatName { get; set; }
        public decimal Temperature { get; set; }
    }
}