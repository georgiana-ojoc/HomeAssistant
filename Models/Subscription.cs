using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace HomeAssistantAPI.Models
{
    public class Subscription : BaseModel
    {
        public Subscription()
        {
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        [Required] public int Price { get; set; }
        public int? Houses { get; set; }
        public int? Rooms { get; set; }
        public int? LightBulbs { get; set; }
        public int? Doors { get; set; }
        public int? Thermostats { get; set; }
        public int? Schedules { get; set; }
        public int? LightBulbCommands { get; set; }
        public int? DoorCommands { get; set; }
        public int? ThermostatCommands { get; set; }

        internal ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}