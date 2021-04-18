using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class House : BaseModel
    {
        public House()
        {
            Rooms = new HashSet<Room>();
        }

        public string Email { get; set; }
        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}