using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public class Door
    {
        public Door()
        {
            DoorCommands = new HashSet<DoorCommand>();
        }

        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public bool Locked { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Room Room { get; set; }

        public ICollection<DoorCommand> DoorCommands { get; set; }
    }
}