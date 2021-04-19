using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public House House { get; set; }
        public ICollection<LightBulb> LightBulbs { get; set; }
        public ICollection<Door> Doors { get; set; }
        public ICollection<Thermostat> Thermostats { get; set; }
    }
}