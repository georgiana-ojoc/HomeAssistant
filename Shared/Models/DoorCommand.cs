using System;

#nullable disable

namespace Shared.Models
{
    public class DoorCommand : BaseModel
    {
        public Guid DoorId { get; set; }
        public Guid ScheduleId { get; set; }
        public bool Locked { get; set; }

        internal Door Door { get; set; }
        internal Schedule Schedule { get; set; }
    }
}