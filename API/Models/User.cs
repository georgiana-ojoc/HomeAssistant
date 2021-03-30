using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public class User
    {
        public User()
        {
            Houses = new HashSet<House>();
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<House> Houses { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}