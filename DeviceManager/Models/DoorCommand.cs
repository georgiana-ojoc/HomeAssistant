#nullable disable

namespace DeviceManager.Models
{
    public class DoorCommand
    {
        public int Id { get; set; }
        public int DoorId { get; set; }
        public int ScheduleId { get; set; }
        public bool Locked { get; set; }

        public Door Door { get; set; }
        public Schedule Schedule { get; set; }
    }
}