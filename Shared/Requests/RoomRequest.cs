using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class RoomRequest
    {
        [Required] public string Name { get; set; }
    }
}