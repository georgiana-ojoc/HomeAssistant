using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class HouseRequest
    {
        [Required] public string Name { get; set; }
    }
}