using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Shared.Models
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
        
        public List<KeyValuePair<String, String>> GetFields()
        {
            List<KeyValuePair<String, String>> list = new()
            {
                new KeyValuePair<String, String>("Houses: ", Houses.ToString()),
                new KeyValuePair<String, String>("Rooms: ", Rooms.ToString()),
                new KeyValuePair<String, String>("LightBulbs: ", LightBulbs.ToString()),
                new KeyValuePair<String, String>("Doors: ", Doors.ToString()),
                new KeyValuePair<String, String>("Thermostats: ", Thermostats.ToString()),
                new KeyValuePair<String, String>("Schedules: ", Schedules.ToString()),
                new KeyValuePair<String, String>("LightBulbCommands: ", LightBulbCommands.ToString()),
                new KeyValuePair<String, String>("DoorCommands: ", DoorCommands.ToString()),
                new KeyValuePair<String, String>("ThermostatCommands: ", ThermostatCommands.ToString())
            };
            return list;
        }
    }
}