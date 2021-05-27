using System;

#nullable disable

namespace Client.Models
{
    public class Room : BaseModel
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
    }
}