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

        [System.Text.Json.Serialization.JsonIgnore]
        public Room Room { get; set; }

        public ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}