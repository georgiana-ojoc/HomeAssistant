using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public class Schedule
    {
        public Schedule()
        {
            LightBulbCommands = new HashSet<LightBulbCommand>();
            DoorCommands = new HashSet<DoorCommand>();
            ThermostatCommands = new HashSet<ThermostatCommand>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public byte Frequency { get; set; }

        public User User { get; set; }
        public ICollection<LightBulbCommand> LightBulbCommands { get; set; }
        public ICollection<DoorCommand> DoorCommands { get; set; }
        public ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}