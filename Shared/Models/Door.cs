using System;
using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class Door : BaseModel
    {
        public Door()
        {
            DoorCommands = new HashSet<DoorCommand>();
        }

        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public bool? Locked { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Room Room { get; set; }

        public ICollection<DoorCommand> DoorCommands { get; set; }
    }
}