using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class DoorRequest
    {
        [Required] public string Name { get; set; }
        public bool? Locked { get; set; }
    }
}