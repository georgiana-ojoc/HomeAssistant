using System.Collections.Generic;

#nullable disable

namespace DeviceManager.Models
{
    public class Thermostat
    {
        public Thermostat()
        {
            ThermostatCommands = new HashSet<ThermostatCommand>();
        }

        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public decimal? Temperature { get; set; }

        public Room Room { get; set; }
        public ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}