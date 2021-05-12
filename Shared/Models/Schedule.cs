using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Time { get; set; }
        [Required] public byte Days { get; set; }

        internal ICollection<LightBulbCommand> LightBulbCommands { get; set; }
        internal ICollection<DoorCommand> DoorCommands { get; set; }
        internal ICollection<ThermostatCommand> ThermostatCommands { get; set; }
    }
}