using System;
using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class Thermostat : BaseModel
    {
        public Thermostat()
        {
            ThermostatCommands = new HashSet<ThermostatCommand>();
        }

        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public decimal? Temperature { get; set; }

        internal Room Room { get; set; }
        internal ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}