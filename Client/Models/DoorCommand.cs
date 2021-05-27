using System;

#nullable disable

namespace Client.Models
{
    public class DoorCommand : BaseModel
    {
        public Guid DoorId { get; set; }
        public Guid ScheduleId { get; set; }
        public bool Locked { get; set; }
    }
}