using System;

#nullable disable

namespace Shared.Models
{
    public class DoorCommand : BaseModel
    {
        public Guid DoorId { get; set; }
        public Guid ScheduleId { get; set; }
        public bool Locked { get; set; }

        public Door Door { get; set; }
        public Schedule Schedule { get; set; }
    }
}