using System;
using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class Schedule : BaseModel
    {
        public Schedule()
        {
            LightBulbCommands = new HashSet<LightBulbCommand>();
            DoorCommands = new HashSet<DoorCommand>();
            ThermostatCommands = new HashSet<ThermostatCommand>();
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public byte Frequency { get; set; }

        internal ICollection<LightBulbCommand> LightBulbCommands { get; set; }
        internal ICollection<DoorCommand> DoorCommands { get; set; }
        internal ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}