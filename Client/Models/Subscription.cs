using System;
using System.Collections.Generic;

#nullable disable

namespace Client.Models
{
    public class Subscription : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? Houses { get; set; }
        public int? Rooms { get; set; }
        public int? LightBulbs { get; set; }
        public int? Doors { get; set; }
        public int? Thermostats { get; set; }
        public int? Schedules { get; set; }
        public int? LightBulbCommands { get; set; }
        public int? DoorCommands { get; set; }
        public int? ThermostatCommands { get; set; }

        public List<KeyValuePair<String, String>> GetFields()
        {
            List<KeyValuePair<String, String>> list = new()
            {
                new KeyValuePair<String, String>("Houses:\t", Houses.ToString()),
                new KeyValuePair<String, String>("Rooms:\t", Rooms.ToString()),
                new KeyValuePair<String, String>("Light bulbs:\t", LightBulbs.ToString()),
                new KeyValuePair<String, String>("Doors:\t", Doors.ToString()),
                new KeyValuePair<String, String>("Thermostats:\t", Thermostats.ToString()),
                new KeyValuePair<String, String>("Schedules:\t", Schedules.ToString()),
                new KeyValuePair<String, String>("Light bulb commands:\t", LightBulbCommands.ToString()),
                new KeyValuePair<String, String>("Door commands:\t", DoorCommands.ToString()),
                new KeyValuePair<String, String>("Thermostat commands:\t", ThermostatCommands.ToString())
            };
            return list;
        }
    }
}