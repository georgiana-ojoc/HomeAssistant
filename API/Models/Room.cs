using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public class Room
    {
        public Room()
        {
            LightBulbs = new HashSet<LightBulb>();
            Doors = new HashSet<Door>();
            Thermostats = new HashSet<Thermostat>();
        }

        public int Id { get; set; }
        public int HouseId { get; set; }
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public House House { get; set; }

        public ICollection<LightBulb> LightBulbs { get; set; }
        public ICollection<Door> Doors { get; set; }
        public ICollection<Thermostat> Thermostats { get; set; }
    }
}