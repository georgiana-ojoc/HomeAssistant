using System;
using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class Room : BaseModel
    {
        public Room()
        {
            LightBulbs = new HashSet<LightBulb>();
            Doors = new HashSet<Door>();
            Thermostats = new HashSet<Thermostat>();
        }

        public Guid HouseId { get; set; }
        public string Name { get; set; }

        internal House House { get; set; }

        internal ICollection<LightBulb> LightBulbs { get; set; }
        internal ICollection<Door> Doors { get; set; }
        internal ICollection<Thermostat> Thermostats { get; set; }
    }
}