#nullable disable

namespace API.Models
{
    public class LightBulbCommand
    {
        public int Id { get; set; }
        public int LightBulbId { get; set; }
        public int ScheduleId { get; set; }
        public int Color { get; set; }
        public byte Intensity { get; set; }

        public LightBulb LightBulb { get; set; }
        public Schedule Schedule { get; set; }
    }
}