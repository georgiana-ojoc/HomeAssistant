using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace API.Models
{
    public class House : BaseModel
    {
        public House()
        {
            Rooms = new HashSet<Room>();
        }

        [Required] public string Email { get; set; }
        [Required] public string Name { get; set; }

        internal ICollection<Room> Rooms { get; set; }
    }
}