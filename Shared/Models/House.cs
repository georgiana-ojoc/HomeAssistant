using System.Collections.Generic;

#nullable disable

namespace Shared.Models
{
    public class House
    {
        public House()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}