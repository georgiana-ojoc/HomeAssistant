using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class DoorRequest
    {
        [Required] public string Name { get; set; }
        public bool? Locked { get; set; }
    }
}