using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HomeAssistantAPI.Models
{
    public class Thermostat : BaseModel
    {
        public Thermostat()
        {
            ThermostatCommands = new HashSet<ThermostatCommand>();
        }

        [Required] public Guid RoomId { get; set; }
        [Required] public string Name { get; set; }
        public bool? Status { get; set; }
        public decimal? Temperature { get; set; }

        internal Room Room { get; set; }
        internal ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}