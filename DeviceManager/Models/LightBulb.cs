using System.Collections.Generic;

#nullable disable

namespace DeviceManager.Models
{
    public class LightBulb
    {
        public LightBulb()
        {
            LightBulbCommands = new HashSet<LightBulbCommand>();
        }

        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int Color { get; set; }
        public byte Intensity { get; set; }

        public Room Room { get; set; }
        public ICollection<LightBulbCommand> LightBulbCommands { get; set; }
    }
}