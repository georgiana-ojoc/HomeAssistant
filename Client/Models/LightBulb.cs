using System;

#nullable disable

namespace Client.Models
{
    public class LightBulb : BaseModel
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int? Color { get; set; }
        public byte? Intensity { get; set; }
    }
}