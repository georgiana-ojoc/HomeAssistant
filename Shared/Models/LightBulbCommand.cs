using System;

#nullable disable

namespace Shared.Models
{
    public class LightBulbCommand : BaseModel
    {
        public Guid LightBulbId { get; set; }
        public Guid ScheduleId { get; set; }
        public int Color { get; set; }
        public byte Intensity { get; set; }

        public LightBulb LightBulb { get; set; }
        public Schedule Schedule { get; set; }
    }
}