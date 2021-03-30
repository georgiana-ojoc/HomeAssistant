using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public class House
    {
        public House()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public User User { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}