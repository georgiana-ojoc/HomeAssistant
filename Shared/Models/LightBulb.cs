using System;
using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class LightBulb : BaseModel
    {
        public LightBulb()
        {
            LightBulbCommands = new HashSet<LightBulbCommand>();
        }

        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int? Color { get; set; }
        public byte? Intensity { get; set; }

        internal Room Room { get; set; }
        internal ICollection<LightBulbCommand> LightBulbCommands { get; set; }
    }
}