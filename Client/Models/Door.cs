using System;

#nullable disable

namespace Client.Models
{
    public class Door : BaseModel
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public bool? Locked { get; set; }
    }
}