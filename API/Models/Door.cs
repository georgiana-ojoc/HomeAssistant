using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace API.Models
{
    public class Door : BaseModel
    {
        public Door()
        {
            DoorCommands = new HashSet<DoorCommand>();
        }

        [Required] public Guid RoomId { get; set; }
        [Required] public string Name { get; set; }
        public bool? Status { get; set; }
        public bool? Locked { get; set; }

        internal Room Room { get; set; }
        internal ICollection<DoorCommand> DoorCommands { get; set; }
    }
}