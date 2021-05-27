using System;

#nullable disable

namespace Client.Models
{
    public class Thermostat : BaseModel
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public decimal? Temperature { get; set; }
    }
}