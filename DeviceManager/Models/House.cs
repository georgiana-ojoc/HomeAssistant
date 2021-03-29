using System.Collections.Generic;

#nullable disable

namespace DeviceManager.Models
{
    public class House
    {
        public House()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}